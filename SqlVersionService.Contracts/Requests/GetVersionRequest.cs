using System;

namespace SqlVersionService.Contracts.Requests
{
    public sealed class GetVersionRequest
    {
        public Guid ConnectionId { get; set; }
    }
}