using NWaySetAssociativeCache.Core.ReplacementAlgorithm;
using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    public class CacheManager<TKey, TValue>
    {
        public static ICache<TKey, TValue> CreateFixedItemCacheWithLeastRecentlyUsedAlgorithm(int totalCacheItemNumber, int itemNumberPerSet)
        {
            Contract.Requires(totalCacheItemNumber != default(int));
            Contract.Requires(itemNumberPerSet > 1);

            Contract.Requires(totalCacheItemNumber > itemNumberPerSet);
            Contract.Requires(totalCacheItemNumber % itemNumberPerSet == 0);
            Contract.Requires(totalCacheItemNumber / itemNumberPerSet != 0);

            return CreateFixedItemCache(totalCacheItemNumber, itemNumberPerSet, new LeastRecentlyUsedReplacementAlgorithmProvider<TKey, TValue>(itemNumberPerSet), new DefaultGetHashCodeSetCalculationPolicy<TKey>());
        }

        public static ICache<TKey, TValue> CreateFixedItemCacheWithMostRecentlyUsedAlgorithm(int totalCacheItemNumber, int itemNumberPerSet)
        {
            Contract.Requires(totalCacheItemNumber != default(int));
            Contract.Requires(itemNumberPerSet > 1);

            Contract.Requires(totalCacheItemNumber > itemNumberPerSet);
            Contract.Requires(totalCacheItemNumber % itemNumberPerSet == 0);
            Contract.Requires(totalCacheItemNumber / itemNumberPerSet != 0);

            return CreateFixedItemCache(totalCacheItemNumber, itemNumberPerSet, new MostRecentlyUsedReplacementAlgorithmProvider<TKey, TValue>(itemNumberPerSet), new DefaultGetHashCodeSetCalculationPolicy<TKey>());
        }

        public static ICache<TKey, TValue> CreateFixedItemCache(int totalCacheItemNumber, int itemNumberPerSet, IReplacementAlgorithmProvider<TKey, TValue> replacementAlgorithmProvider, ISetCalculationPolicy<TKey> setCalculationPolicy)
        {
            Contract.Requires(totalCacheItemNumber != default(int));
            Contract.Requires(itemNumberPerSet > 1);

            Contract.Requires(totalCacheItemNumber > itemNumberPerSet);
            Contract.Requires(totalCacheItemNumber % itemNumberPerSet == 0);
            Contract.Requires(totalCacheItemNumber / itemNumberPerSet != 0);

            var cacheSetDictionary = CacheSetDictionaryProvider<TKey, TValue>.Create(totalCacheItemNumber / itemNumberPerSet, replacementAlgorithmProvider);

            return new SetAssociativeCache<TKey, TValue>(setCalculationPolicy, cacheSetDictionary);
        }
    }
}
