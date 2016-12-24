using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Core;
using NWaySetAssociativeCache.Core.ReplacementAlgorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SetAssociativeCacheIntegrationTests
    {
        [TestMethod]
        public void Integration_SetAssociativeCache_MultiThreading_MostRecentlyUsedReplacementAlgorithmProvider()
        {
            int elementNumber = 1000000;

            var cacheSetDictionary = CacheSetDictionaryProvider< KeyData, ValueData>.Create(1000, new MostRecentlyUsedReplacementAlgorithmProvider<KeyData, ValueData>(10));
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(30000, totalHintCount);
        }

        [TestMethod]
        public void Integration_SetAssociativeCache_MultiThreading_LeastRecentlyUsedReplacementAlgorithmProvider()
        {
            int elementNumber = 1000000;

            var cacheSetDictionary = CacheSetDictionaryProvider< KeyData, ValueData>.Create(1000, new LeastRecentlyUsedReplacementAlgorithmProvider<KeyData, ValueData>(10));
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(10000, totalHintCount);
        }

        [TestMethod]
        public void Integration_SetAssociativeCache_MultiThreading_DirectMappingReplacementAlgorithmProvider()
        {
            int elementNumber = 1000000;

            var cacheSetDictionary = CacheSetDictionaryProvider< KeyData, ValueData>.Create(100000, new DirectMappingReplacementAlgorithmProvider<KeyData, ValueData>());
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(100000, totalHintCount);
        }

    }
}
