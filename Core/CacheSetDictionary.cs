using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    public class CacheSetDictionary<TKey, TValue> : Dictionary<int, ICache<TKey, TValue>>, ICacheSetDictionary<TKey, TValue>
    {
        readonly object stateGuard = new object();
        readonly IReplacementAlgorithmProvider<TKey, TValue> replacementAlgorithmProvider;

        public CacheSetDictionary(int numberOfSet, IReplacementAlgorithmProvider<TKey, TValue> replacementAlgorithmProvider) : base()
        {
            Contract.Requires(numberOfSet != default(int));
            Contract.Requires(replacementAlgorithmProvider != null);

            this.replacementAlgorithmProvider = replacementAlgorithmProvider;
            NumberOfSet = numberOfSet;
        }

        public int NumberOfSet { get; }

        public ICache<TKey, TValue> TryGetOrCreateSet(int setId)
        {
            lock (stateGuard)
            {
                ICache<TKey, TValue> replacementAlgorithm;

                if (!this.TryGetValue(setId, out replacementAlgorithm))
                {
                    replacementAlgorithm = replacementAlgorithmProvider.Create(setId);
                    this.Add(setId, replacementAlgorithm);
                }

                Contract.Assume(replacementAlgorithm != null);
                return replacementAlgorithm;
            }
        }

        public new void Clear()
        {
            lock (stateGuard)
            {
                foreach (var value in this.Values) value.Clear();
                base.Clear();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
