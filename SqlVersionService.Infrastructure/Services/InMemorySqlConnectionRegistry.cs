using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SqlVersionService.Infrastructure.Models;
using SqlVersionService.Infrastructure.Abstractions;

namespace SqlVersionService.Infrastructure.Services
{
    public sealed class InMemorySqlConnectionRegistry : ISqlConnectionRegistry
    {
        private readonly ConcurrentDictionary<Guid, ConnectionSession> _sessions = new ConcurrentDictionary<Guid, ConnectionSession>();

        public void Add(ConnectionSession session)
        {
            _sessions[session.Id] = session;
        }

        public bool TryGet(Guid id, out ConnectionSession session)
        {
            return _sessions.TryGetValue(id, out session);
        }

        public bool TryRemove(Guid id, out ConnectionSession session)
        {
            return _sessions.TryRemove(id, out session);
        }

        public IReadOnlyCollection<ConnectionSession> GetAll()
        {
            return _sessions.Values.ToList();
        }
    }
}