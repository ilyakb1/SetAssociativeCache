using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    public class SetAssociativeCache<TKey, TValue> : ICache<TKey, TValue>
    {
        readonly ISetCalculationPolicy<TKey> setCalculationPolicy;
        readonly ICacheSetDictionary<TKey, TValue> cacheSetDictionary;

        public SetAssociativeCache(
            ISetCalculationPolicy<TKey> setCalculationPolicy,
            ICacheSetDictionary<TKey, TValue> cacheSetDictionary)
        {
            Contract.Requires(setCalculationPolicy != null);
            Contract.Requires(cacheSetDictionary != null);

            this.setCalculationPolicy = setCalculationPolicy;
            this.cacheSetDictionary = cacheSetDictionary;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) return;

            int setId = setCalculationPolicy.GetSetId(key, cacheSetDictionary.NumberOfSet);
            var replacementAlgorithm = cacheSetDictionary.TryGetOrCreateSet(setId);
            replacementAlgorithm.Add(key, value);
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (key == null)
            {
                value = default(TValue);
                return false;
            }

            int setId = setCalculationPolicy.GetSetId(key, cacheSetDictionary.NumberOfSet);

            ICache<TKey, TValue> replacementAlgorithm;
            if (!cacheSetDictionary.TryGetValue(setId, out replacementAlgorithm))
            {
                value = default(TValue);
                return false;
            }

            return replacementAlgorithm.TryGet(key, out value) ? true : false;
        }

        public void Clear()
        {
            cacheSetDictionary.Clear();
        }
    }
}
