using SqlVersionService.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace SqlVersionService.Infrastructure.Abstractions
{
    public interface ISqlConnectionRegistry
    {
        void Add(ConnectionSession session);
        bool TryGet(Guid id, out ConnectionSession session);
        bool TryRemove(Guid id, out ConnectionSession session);
        IReadOnlyCollection<ConnectionSession> GetAll();
    }
}