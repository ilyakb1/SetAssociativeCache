namespace NWaySetAssociativeCache.Core
{
    public interface ICache<in TKey, TValue>
    {
        void Add(TKey key, TValue value);
        bool TryGet(TKey key, out TValue value);
        void Clear();
    }
}