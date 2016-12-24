using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core.ReplacementAlgorithm
{
    public class MostRecentlyUsedReplacementAlgorithmProvider<TKey, TValue> : IReplacementAlgorithmProvider<TKey, TValue>
    {
        public MostRecentlyUsedReplacementAlgorithmProvider(int capacity)
        {
            Contract.Requires(capacity > 1);
            Capacity = capacity;
        }

        public int Capacity { get; }

        public ICache<TKey, TValue> Create(int setId)
        {
            return new MostRecentlyUsedReplacementAlgorithm<TKey, TValue>(Capacity);
        }
    }
}
