using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    [ContractClass(typeof(ReplacementAlgorithmProviderContract<,>))]
    public interface IReplacementAlgorithmProvider<in TKey, TValue>
    {
        ICache<TKey, TValue> Create(int setId);
    }

    [ContractClassFor(typeof(IReplacementAlgorithmProvider<,>))]
    public abstract class ReplacementAlgorithmProviderContract<TKey, TValue> : IReplacementAlgorithmProvider<TKey, TValue>
    {
        public ICache<TKey, TValue> Create(int setId)
        {
            Contract.Ensures(Contract.Result<ICache<TKey, TValue>>() != null);
            return default(ICache<TKey, TValue>);
        }
    }
}
