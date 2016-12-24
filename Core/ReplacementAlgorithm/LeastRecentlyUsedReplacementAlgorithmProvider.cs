using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core.ReplacementAlgorithm
{
    public class LeastRecentlyUsedReplacementAlgorithmProvider<TKey, TValue> : IReplacementAlgorithmProvider<TKey, TValue>
    {
        public LeastRecentlyUsedReplacementAlgorithmProvider(int capacity)
        {
            Contract.Requires(capacity > 1);
            Capacity = capacity;
        }

        public int Capacity { get; }

        public ICache<TKey, TValue> Create(int setId)
        {
            return new LeastRecentlyUsedReplacementAlgorithm<TKey, TValue>(Capacity);
        }
    }
}
