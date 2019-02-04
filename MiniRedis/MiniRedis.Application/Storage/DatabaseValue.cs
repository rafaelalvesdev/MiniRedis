using MiniRedis.Services.Storage.Enums;
using System;

namespace MiniRedis.Services.Storage
{
    public class DatabaseValue
    {
        public DatabaseValueType Type { get; }
        public object Value { get; }
        public DateTimeOffset? TTL { get; }

        public DatabaseValue(string value)
        {
            Type = DatabaseValueType.Plain;
            Value = value;
        }

        public DatabaseValue(string value, DateTimeOffset? ttl)
            :this(value)
        {
            TTL = ttl;
        }

        public DatabaseValue(ScoredCollection value)
        {
            Type = DatabaseValueType.ScoredCollection;
            Value = value;
        }

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
    }
}