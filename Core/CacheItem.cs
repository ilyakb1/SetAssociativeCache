using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    public class CacheItem<TKey, TValue>
    {
        public CacheItem(TKey key, TValue value)
        {
            Contract.Requires(key != null);
            Key = key;
            Value = value;
        }

        public TKey Key { get; }
        public TValue Value { get; }
    }
}
