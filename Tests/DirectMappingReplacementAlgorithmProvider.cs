using NWaySetAssociativeCache.Core;

namespace Tests
{
    public class DirectMappingReplacementAlgorithmProvider<TKey, TValue> : IReplacementAlgorithmProvider<TKey, TValue>
    {
        public ICache<TKey, TValue> Create(int setId)
        {
            return new DirectMappingReplacementAlgorithm<TKey, TValue>();
        }
    }
}
