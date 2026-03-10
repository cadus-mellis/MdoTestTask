using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using SqlVersionService.Application.Abstractions;
using SqlVersionService.Infrastructure.Models;

namespace SqlVersionService.Web.Background
{
    public sealed class SessionCleanupJob : IDisposable
    {
        private readonly ISqlConnectionSessionService _sessionService;
        private readonly SqlSessionOptions _options;
        private readonly ILogger<SessionCleanupJob> _logger;
        private Timer _timer;
        private int _isRunning;

        public SessionCleanupJob(
            ISqlConnectionSessionService sessionService,
            SqlSessionOptions options,
            ILogger<SessionCleanupJob> logger)
        {
            _sessionService = sessionService;
            _options = options;
            _logger = logger;
        }

        public void Start()
        {
            _timer = new Timer(
                _ => Execute(),
                null,
                TimeSpan.FromSeconds(_options.CleanupIntervalSeconds),
                TimeSpan.FromSeconds(_options.CleanupIntervalSeconds));

            _logger.LogInformation(
                "Session cleanup job started. IntervalSeconds: {IntervalSeconds}",
                _options.CleanupIntervalSeconds);
        }

        private void Execute()
        {
            if (Interlocked.Exchange(ref _isRunning, 1) == 1)
                return;

            try
            {
                var removedCount = _sessionService.CleanupExpiredSessions();

                if (removedCount > 0)
                {
                    _logger.LogInformation(
                        "Session cleanup completed. RemovedCount: {RemovedCount}",
                        removedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Session cleanup failed");
            }
            finally
            {
                Interlocked.Exchange(ref _isRunning, 0);
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}