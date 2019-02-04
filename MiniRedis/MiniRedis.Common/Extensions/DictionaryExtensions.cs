using System.Collections.Generic;

namespace MiniRedis.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) 
            where TValue : new()
        {
            TValue result;

            if (dictionary.TryGetValue(key, out result)) return result;

            result = new TValue();
            dictionary[key] = result;

            return result;
        }
    }
}
