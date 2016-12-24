using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class CacheManagerTests
    {
        [TestMethod]
        public void CacheManager_CreateFixedItemCache()
        {

            int elementNumber = 100000;

            var cache = CacheManager<KeyData, ValueData>.CreateFixedItemCache(
                10000,
                10,
                new DirectMappingReplacementAlgorithmProvider<KeyData, ValueData>(),
                new KeyDataSetCalculationPolicy());

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(3000, totalHintCount);
        }

        [TestMethod]
        public void CacheManager_CreateFixedItemCacheWithLeastRecentlyUsedAlgorithm()
        {

            int elementNumber = 100000;
            var cache = CacheManager<KeyData, ValueData>.CreateFixedItemCacheWithLeastRecentlyUsedAlgorithm(10000, 10);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(30000, totalHintCount);
        }

        [TestMethod]
        public void CacheManager_CreateFixedItemCacheWithMostRecentlyUsedAlgorithm()
        {
            int elementNumber = 100000;
            var cache = CacheManager<KeyData, ValueData>.CreateFixedItemCacheWithMostRecentlyUsedAlgorithm(10000, 10);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(30000, totalHintCount);
        }
    }
}
