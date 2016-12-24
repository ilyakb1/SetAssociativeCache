using NWaySetAssociativeCache.Core;

namespace Tests
{
    public class DirectMappingReplacementAlgorithm<TKey, TValue> : ICache<TKey, TValue>
    {
        CacheItem<TKey, TValue> cachedItem;

        public void Add(TKey key, TValue value)
        {
            if (key == null) return;
            cachedItem = new CacheItem<TKey, TValue>(key, value);
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (key != null && cachedItem != default(CacheItem<TKey, TValue>) && key.Equals(cachedItem.Key))
            {
                value = cachedItem.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void Clear()
        {
            cachedItem = default(CacheItem<TKey, TValue>);
        }
    }
}
