using System;
using System.Threading;
using System.Threading.Tasks;

namespace SqlVersionService.Application.Abstractions
{
    public interface ISqlConnectionSessionService
    {
        Task<Guid> OpenAsync(string connectionString, CancellationToken cancellationToken);
        Task<string> GetVersionAsync(Guid connectionId, CancellationToken cancellationToken);
        Task<bool> CloseAsync(Guid connectionId, CancellationToken cancellationToken);
        int GetSessionTimeoutSeconds();
        int CleanupExpiredSessions();
        void CloseAll();
    }
}