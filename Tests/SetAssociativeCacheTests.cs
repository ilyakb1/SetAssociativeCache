using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NWaySetAssociativeCache.Core;
using NWaySetAssociativeCache.Core.ReplacementAlgorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SetAssociativeCacheTests
    {
        [TestMethod]
        public void SetAssociativeCache_Get()
        {
            var item1 = new CacheItem<KeyData, ValueData>(new KeyData(12, "Key 12"), new ValueData(12, "Value 12"));
            var item2 = new CacheItem<KeyData, ValueData>(new KeyData(24, "Key 24"), new ValueData(24, "Value 24"));

            int totalCacheItemNumber = 20;
            int itemNumberPerSet = 5;

            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);

            var setDictionary = new Dictionary<int, CacheItem<KeyData, ValueData>>();

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.CreateLazyLoading(totalCacheItemNumber / itemNumberPerSet, replacementAlgorithmProvider.Object);
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            ValueData value;
            Assert.IsFalse(cache.TryGet(item1.Key, out value));
            Assert.IsFalse(cache.TryGet(item2.Key, out value));

            replacementAlgorithm.VerifyAll();
            replacementAlgorithmProvider.VerifyAll();
        }

        [TestMethod]
        public void SetAssociativeCache_Add()
        {
            int totalCacheItemNumber = 20;
            int itemNumberPerSet = 5;

            var item1 = new CacheItem<KeyData, ValueData>(new KeyData(12, "Key 12"), new ValueData(12, "Value 12"));
            var item2 = new CacheItem<KeyData, ValueData>(new KeyData(24, "Key 24"), new ValueData(24, "Value 24"));
            var item3 = new CacheItem<KeyData, ValueData>(new KeyData(51, "Key 51"), new ValueData(51, "Value 51"));

            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider =
                new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);
            replacementAlgorithmProvider.Setup(p => p.Create(0)).Returns(replacementAlgorithm.Object);
            replacementAlgorithmProvider.Setup(p => p.Create(3)).Returns(replacementAlgorithm.Object);

            replacementAlgorithm.Setup(a => a.Add(item1.Key, item1.Value));
            replacementAlgorithm.Setup(a => a.Add(item2.Key, item2.Value));
            replacementAlgorithm.Setup(a => a.Add(item3.Key, item3.Value));


            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.CreateLazyLoading(totalCacheItemNumber / itemNumberPerSet, replacementAlgorithmProvider.Object);
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);
            cache.Add(item1.Key, item1.Value);
            cache.Add(item2.Key, item2.Value);
            cache.Add(item3.Key, item3.Value);

            replacementAlgorithm.VerifyAll();
            replacementAlgorithmProvider.VerifyAll();
        }


        [TestMethod]
        public void SetAssociativeCache_Add_Get()
        {
            int totalCacheItemNumber = 20;
            int itemNumberPerSet = 5;

            var item1 = new CacheItem<KeyData, ValueData>(new KeyData(12, "Key 12"), new ValueData(12, "Value 12"));
            var item2 = new CacheItem<KeyData, ValueData>(new KeyData(24, "Key 24"), new ValueData(24, "Value 24"));
            var item3 = new CacheItem<KeyData, ValueData>(new KeyData(51, "Key 51"), new ValueData(51, "Value 51"));

            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);

            replacementAlgorithmProvider.Setup(p => p.Create(0)).Returns(replacementAlgorithm.Object);
            replacementAlgorithmProvider.Setup(p => p.Create(3)).Returns(replacementAlgorithm.Object);

            replacementAlgorithm.Setup(a => a.Add(item1.Key, item1.Value));
            replacementAlgorithm.Setup(a => a.Add(item3.Key, item3.Value));

            ValueData value1 = item1.Value;
            replacementAlgorithm.Setup(a => a.TryGet(item1.Key, out value1)).Returns(true);

            ValueData value2 = null;
            replacementAlgorithm.Setup(a => a.TryGet(item2.Key, out value2)).Returns(false);

            ValueData value3 = item3.Value;
            replacementAlgorithm.Setup(a => a.TryGet(item3.Key, out value3)).Returns(true);

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.CreateLazyLoading(totalCacheItemNumber / itemNumberPerSet, replacementAlgorithmProvider.Object);
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            cache.Add(item1.Key, item1.Value);
            cache.Add(item3.Key, item3.Value);

            ValueData value11 = null;
            Assert.IsTrue(cache.TryGet(item1.Key, out value11));
            Assert.AreEqual(item1.Value, value11);

            ValueData value22 = null;
            Assert.IsFalse(cache.TryGet(item2.Key, out value22));

            ValueData value33 = null;
            Assert.IsTrue(cache.TryGet(item3.Key, out value33));
            Assert.AreEqual(item3.Value, value33);

            Assert.AreEqual(item1.Value, value1);
            Assert.AreEqual(null, value2);
            Assert.AreEqual(item3.Value, value3);

            replacementAlgorithm.VerifyAll();
            replacementAlgorithmProvider.VerifyAll();
        }

        [TestMethod]
        public void SetAssociativeCache_Add_Clear_Add()
        {
            int totalCacheItemNumber = 20;
            int itemNumberPerSet = 5;

            var item1 = new CacheItem<KeyData, ValueData>(new KeyData(12, "Key 12"), new ValueData(12, "Value 12"));

            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);
            replacementAlgorithmProvider.Setup(p => p.Create(0)).Returns(replacementAlgorithm.Object);

            replacementAlgorithm.Setup(a => a.Add(item1.Key, item1.Value));
            replacementAlgorithm.Setup(a => a.Clear());

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.CreateLazyLoading(totalCacheItemNumber / itemNumberPerSet, replacementAlgorithmProvider.Object);
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            cache.Add(item1.Key, item1.Value);
            cache.Clear();
            cache.Add(item1.Key, item1.Value);

            replacementAlgorithmProvider.Verify(p => p.Create(0), Times.Exactly(2));
            replacementAlgorithm.VerifyAll();
            replacementAlgorithmProvider.VerifyAll();
        }


        [TestMethod]
        public void SetAssociativeCache_MultiThreading()
        {
            int elementNumber = 1000;

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.Create(50, new LeastRecentlyUsedReplacementAlgorithmProvider<KeyData, ValueData>(5));
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            var dataList = TestDataHelper.Generate(elementNumber);

            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);

            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine("Cache hit {0} out of {1}", totalHintCount, taskList.Count * elementNumber);
            Assert.IsTrue(totalHintCount > 0, "At least one cache hit");
        }

        [TestMethod]
        public void SetAssociativeCache_MultiThreading_AllFromCache()
        {
            int elementNumber = 1000;

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.Create(100, new MostRecentlyUsedReplacementAlgorithmProvider<KeyData, ValueData>(5));
            var cache = new SetAssociativeCache<KeyData, ValueData>(new KeyDataSetCalculationPolicy(), cacheSetDictionary);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, cache);
            var task4 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, cache);

            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine("Cache hit {0} out of {1}", totalHintCount, taskList.Count * elementNumber);
            Assert.AreEqual(1500, totalHintCount);
        }
    }
}



