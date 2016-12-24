using System;

namespace NWaySetAssociativeCache.Core
{
    public class DefaultGetHashCodeSetCalculationPolicy<TKey> : ISetCalculationPolicy<TKey>
    {
        public int GetSetId(TKey key, int numberOfSet)
        {
            int hashCode = Math.Abs(key.GetHashCode());
            return hashCode % numberOfSet;
        }
    }
}
