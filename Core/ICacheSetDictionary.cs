using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    public interface ICacheSetDictionary<TKey, TValue> : ICacheSetDictionaryBase<TKey, TValue>, IDictionary<int, ICache<TKey, TValue>>
    {
        new void Clear();
    }

    [ContractClass(typeof(CacheSetDictionaryContract<,>))]
    public interface ICacheSetDictionaryBase<in TKey, TValue>
    {
        int NumberOfSet { get; }

        ICache<TKey, TValue> TryGetOrCreateSet(int setId);
    }

    [ContractClassFor(typeof(ICacheSetDictionaryBase<,>))]
    public abstract class CacheSetDictionaryContract<TKey, TValue> : ICacheSetDictionaryBase<TKey, TValue>
    {
        public int NumberOfSet => default(int);

        public ICache<TKey, TValue> TryGetOrCreateSet(int setId)
        {
            Contract.Requires(setId >= 0);
            Contract.Ensures(Contract.Result<ICache<TKey, TValue>>() != null);
            return default(ICache<TKey, TValue>);
        }
    }
}
