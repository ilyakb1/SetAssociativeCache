using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    public class CacheSetDictionaryProvider<TKey, TValue>
    {
        public static ICacheSetDictionary<TKey, TValue> Create(int numberOfSet, IReplacementAlgorithmProvider<TKey, TValue> replacementAlgorithmProvider)
        {
            Contract.Requires(numberOfSet > 0);
            Contract.Requires(replacementAlgorithmProvider != null);

            var cacheSetDictionary = new CacheSetDictionary<TKey, TValue>(numberOfSet, replacementAlgorithmProvider);
            for (int i = 0; i < numberOfSet; i++) cacheSetDictionary.TryGetOrCreateSet(i);
            return cacheSetDictionary;
        }

        public static ICacheSetDictionary<TKey, TValue> CreateLazyLoading(int numberOfSet, IReplacementAlgorithmProvider<TKey, TValue> replacementAlgorithmProvider)
        {
            Contract.Requires(numberOfSet > 0);
            Contract.Requires(replacementAlgorithmProvider != null);

            return new CacheSetDictionary<TKey, TValue>(numberOfSet, replacementAlgorithmProvider);
        }
    }
}
