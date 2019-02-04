using MiniRedis.Services.Storage.Enums;
using System;

namespace MiniRedis.Services.Storage
{
    public class DatabaseValue
    {
        public DatabaseValue(DatabaseValueType type, object value)
        {
            Type = type;
            Value = value;
        }

        public DatabaseValue(DatabaseValueType type, object value, DateTimeOffset? ttl)
            : this(type, value)
        {
            TTL = ttl;
        }

        public DatabaseValueType Type { get; }

        public object Value { get; }

        public DateTimeOffset? TTL { get; }
    }
}