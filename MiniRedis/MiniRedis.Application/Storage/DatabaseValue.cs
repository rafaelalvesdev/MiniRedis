using MiniRedis.Services.Storage.Enums;
using System;

namespace MiniRedis.Services.Storage
{
    public class DatabaseValue
    {
        public DatabaseValue(DatabaseValueType type, object value, DateTimeOffset? ttl)
        {
            Type = type;
            Value = value;
            TTL = ttl;
        }

        public DatabaseValueType Type { get; }

        public object Value { get; }

        public DateTimeOffset? TTL { get; }
    }
}