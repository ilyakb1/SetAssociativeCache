using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class TestHelper
    {
        public static Task<int> AddAndGetDataFromCacheInReverseOrder(
            Dictionary<KeyData, ValueData> dataList,
            ICache<KeyData, ValueData> algorithm)
        {
            int hitCount = 0;
            var keys = dataList.Keys.ToArray();
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                var key = keys[i];
                ValueData value;
                if (!algorithm.TryGet(key, out value))
                {
                    algorithm.Add(key, dataList[key]);
                    continue;
                }

                Assert.AreEqual(dataList[key], value, "Cache is broken. Cache value not mathed original.");
                hitCount++;
            }

            return Task.FromResult(hitCount);
        }

        public static Task<int> AddAndGetDataFromCacheInOrder(
            Dictionary<KeyData, ValueData> dataList,
            ICache<KeyData, ValueData> algorithm)
        {
            int hitCount = 0;
            var keys = dataList.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                ValueData value;
                if (!algorithm.TryGet(key, out value))
                {
                    algorithm.Add(key, dataList[key]);
                    continue;
                }

                Assert.AreEqual(dataList[key], value, "Cache is broken. Cache value not mathed original.");
                hitCount++;
            }

            return Task.FromResult(hitCount);
        }
    }
}
