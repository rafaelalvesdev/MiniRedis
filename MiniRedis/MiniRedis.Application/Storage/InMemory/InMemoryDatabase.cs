using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniRedis.Services.Storage.InMemory
{
    public class InMemoryDatabase : IDatabase
    {
        private Dictionary<string, DatabaseValue> data = new Dictionary<string, DatabaseValue>();

        public GenericResult<DatabaseValue> Get(string key)
        {
            if (data.ContainsKey(key))
            {
                var value = data[key];                               
                if (!value.TTL.HasValue || (value.TTL.HasValue && value.TTL > DateTimeOffset.Now))
                    return new GenericResult<DatabaseValue>(data[key]).Valid();
            }

            return new GenericResult<DatabaseValue>().WithError("Invalid key.");
        }

        public GenericResult<DatabaseValue> Remove(string key)
        {
            var value = Get(key);

            if (value.IsValid)
                data.Remove(key);

            return value;            
        }

        public GenericResult Save(string key, DatabaseValue value)
        {
            data[key] = value;
            return new GenericResult().Valid();
        }

        public GenericResult<string[]> GetAllKeys()
        {
            var keys = data
                        .Where(x => (!x.Value.TTL.HasValue) || (x.Value.TTL.HasValue && x.Value.TTL > DateTimeOffset.Now))
                        .Select(x => x.Key)
                        .ToArray();

            return new GenericResult<string[]>(keys).Valid();
        }
    }
}
