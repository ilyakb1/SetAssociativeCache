using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core
{
    [ContractClass(typeof(SetCalculationPolicyContract<>))]
    public interface ISetCalculationPolicy<in TKey>
    {
        int GetSetId(TKey key, int numberOfSet);
    }

    [ContractClassFor(typeof(ISetCalculationPolicy<>))]
    public abstract class SetCalculationPolicyContract<TKey> : ISetCalculationPolicy<TKey>
    {
        public int GetSetId(TKey key, int numberOfSet)
        {
            Contract.Requires(numberOfSet != default(int));
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<int>() >= 0);
            return default(int);
        }
    }
}