using System;
using Microsoft.Data.SqlClient;

namespace SqlVersionService.Infrastructure.Models
{
    public sealed class ConnectionSession
    {
        public ConnectionSession(Guid id, SqlConnection connection, DateTime createdAtUtc)
        {
            Id = id;
            Connection = connection;
            CreatedAtUtc = createdAtUtc;
            LastAccessUtc = createdAtUtc;
        }

        public Guid Id { get; }
        public SqlConnection Connection { get; }
        public DateTime CreatedAtUtc { get; }
        public DateTime LastAccessUtc { get; private set; }

        public void Touch(DateTime utcNow)
        {
            LastAccessUtc = utcNow;
        }
    }
}