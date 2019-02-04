using Microsoft.Extensions.Caching.Memory;
using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Storage.Interfaces;
using System;
using System.Threading;

namespace MiniRedis.Services.Storage.InMemory
{
    public class InMemoryDatabase : IDatabase
    {
        private IMemoryCache data;
        private string identity = Guid.NewGuid().ToString();
        private object saveLockObject = new object();

        public InMemoryDatabase(IMemoryCache memoryCache)
        {
            data = memoryCache;
        }

        private string GetLockKey(string key) => $"{identity}_key";
        
        public GenericResult<DatabaseValue> Get(string key)
        {
            if (data.TryGetValue(key, out object result))
            {
                var value = result as DatabaseValue;
                return new GenericResult<DatabaseValue>(value);
            }

            return new GenericResult<DatabaseValue>().WithError("Invalid key.");
        }

        public GenericResult<DatabaseValue> Remove(string key)
        {
            bool valid = false;
            if (data.TryGetValue(key, out object value))
            {
                data.Remove(key);
                valid = true;
            }

            return new GenericResult<DatabaseValue>(value as DatabaseValue).Valid(valid);
        }

        public GenericResult Save(string key, DatabaseValue value)
        {
            var ttl = value.TTL;
            if (value.TTL.HasValue)
                data.Set(key, value, ttl.Value);
            else
                data.Set(key, value);

            return new GenericResult();
        }

        public GenericResult<long> GetKeysCount()
        {
            var keys = (data as MemoryCache).Count;
            return new GenericResult<long>(keys);
        }
    }
}
