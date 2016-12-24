using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWaySetAssociativeCache.Core;

namespace Tests
{
    public class KeyDataSetCalculationPolicy : ISetCalculationPolicy<KeyData>
    {
        public int GetSetId(KeyData key, int numberOfSet)
        {
            return key.Id%numberOfSet;
        }
    }
}
