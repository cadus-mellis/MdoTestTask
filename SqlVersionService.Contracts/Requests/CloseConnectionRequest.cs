using System;

namespace SqlVersionService.Contracts.Requests
{
    public sealed class CloseConnectionRequest
    {
        public Guid ConnectionId { get; set; }
    }
}