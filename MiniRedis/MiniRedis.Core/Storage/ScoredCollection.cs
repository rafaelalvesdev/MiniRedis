using MiniRedis.Common.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniRedis.Core.Storage
{
    public class ScoredCollection
    {
        public ConcurrentDictionary<string, float> Collection { get; private set; } = new ConcurrentDictionary<string, float>();

        public SortedDictionary<float, List<string>> SortedCollection { get; private set; } = new SortedDictionary<float, List<string>>();

        public List<string> SortedList { get; private set; } = new List<string>();

        public async Task UpdateSortedCollection()
        {
            SortedCollection.Clear();

            foreach (var item in Collection)
                SortedCollection.GetOrCreate(item.Value).Add(item.Key);

            foreach (var item in SortedCollection)
                item.Value.Sort((x, y) => StringComparer.Ordinal.Compare(x, y));

            SortedList = SortedCollection.SelectMany(x => x.Value).ToList();
        }
    }
}
