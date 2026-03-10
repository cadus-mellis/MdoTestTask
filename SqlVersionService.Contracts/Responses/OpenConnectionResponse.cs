using System;

namespace SqlVersionService.Contracts.Responses
{
    public sealed class OpenConnectionResponse
    {
        public Guid ConnectionId { get; set; }
        public int SessionTimeoutSeconds { get; set; }
    }
}