namespace SqlVersionService.Infrastructure.Models
{
    public sealed class SqlSessionOptions
    {
        public int SessionTimeoutSeconds { get; set; } = 300;
        public int CleanupIntervalSeconds { get; set; } = 60;
    }
}