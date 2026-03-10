using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SqlVersionService.Application.Abstractions;
using SqlVersionService.Infrastructure.Models;
using SqlVersionService.Infrastructure.Abstractions;

namespace SqlVersionService.Application.Services
{
    public sealed class SqlConnectionSessionService : ISqlConnectionSessionService
    {
        private readonly ISqlConnectionRegistry _registry;
        private readonly ILogger<SqlConnectionSessionService> _logger;
        private readonly SqlSessionOptions _options;

        public SqlConnectionSessionService(
            ISqlConnectionRegistry registry,
            ILogger<SqlConnectionSessionService> logger,
            SqlSessionOptions options)
        {
            _registry = registry;
            _logger = logger;
            _options = options;
        }

        public async Task<Guid> OpenAsync(string connectionString, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is required.", nameof(connectionString));

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            var now = DateTime.UtcNow;
            var connectionId = Guid.NewGuid();
            var session = new ConnectionSession(connectionId, connection, now);

            _registry.Add(session);

            _logger.LogInformation(
                "SQL connection opened. ConnectionId: {ConnectionId}, TimeoutSeconds: {TimeoutSeconds}",
                connectionId,
                _options.SessionTimeoutSeconds);

            return connectionId;
        }

        public async Task<string> GetVersionAsync(Guid connectionId, CancellationToken cancellationToken)
        {
            if (!_registry.TryGet(connectionId, out var session))
            {
                _logger.LogWarning("Connection not found. ConnectionId: {ConnectionId}", connectionId);
                throw new InvalidOperationException($"Connection '{connectionId}' was not found.");
            }

            if (IsExpired(session))
            {
                CloseInternal(session);
                _registry.TryRemove(connectionId, out _);

                _logger.LogWarning("Connection expired. ConnectionId: {ConnectionId}", connectionId);
                throw new InvalidOperationException($"Connection '{connectionId}' has expired.");
            }

            session.Touch(DateTime.UtcNow);

            using (var command = session.Connection.CreateCommand())
            {
                command.CommandText = "SELECT @@VERSION";
                var result = await command.ExecuteScalarAsync(cancellationToken);
                var version = result?.ToString();

                _logger.LogInformation("SQL version fetched. ConnectionId: {ConnectionId}", connectionId);

                return version;
            }
        }

        public Task<bool> CloseAsync(Guid connectionId, CancellationToken cancellationToken)
        {
            if (!_registry.TryRemove(connectionId, out var session))
            {
                _logger.LogWarning("Close requested for unknown ConnectionId: {ConnectionId}", connectionId);
                return Task.FromResult(false);
            }

            CloseInternal(session);

            _logger.LogInformation("SQL connection closed. ConnectionId: {ConnectionId}", connectionId);

            return Task.FromResult(true);
        }

        public int GetSessionTimeoutSeconds()
        {
            return _options.SessionTimeoutSeconds;
        }

        public int CleanupExpiredSessions()
        {
            var now = DateTime.UtcNow;
            var expiredSessions = _registry
                .GetAll()
                .Where(x => now - x.LastAccessUtc > TimeSpan.FromSeconds(_options.SessionTimeoutSeconds))
                .ToList();

            foreach (var session in expiredSessions)
            {
                if (_registry.TryRemove(session.Id, out var removed))
                {
                    CloseInternal(removed);

                    _logger.LogInformation(
                        "Expired SQL connection cleaned up. ConnectionId: {ConnectionId}",
                        removed.Id);
                }
            }

            return expiredSessions.Count;
        }

        public void CloseAll()
        {
            var sessions = _registry.GetAll().ToList();

            foreach (var session in sessions)
            {
                if (_registry.TryRemove(session.Id, out var removed))
                {
                    CloseInternal(removed);

                    _logger.LogInformation(
                        "SQL connection closed during shutdown. ConnectionId: {ConnectionId}",
                        removed.Id);
                }
            }
        }

        private bool IsExpired(ConnectionSession session)
        {
            var now = DateTime.UtcNow;
            return now - session.LastAccessUtc > TimeSpan.FromSeconds(_options.SessionTimeoutSeconds);
        }

        private void CloseInternal(ConnectionSession session)
        {
            try
            {
                session.Connection.Close();
            }
            catch
            {
            }

            try
            {
                session.Connection.Dispose();
            }
            catch
            {
            }
        }
    }
}