using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Core.ReplacementAlgorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class MostRecentlyUsedReplacementAlgorithmTests
    {
        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithmTests_Get()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData = new KeyData(1, "Key1");
            ValueData value;
            Assert.IsFalse(algorithm.TryGet(keyData, out value));
            Assert.AreEqual(null, value);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_Add_Get()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData = new KeyData(1, "Key1");
            var valueData = new ValueData(1, "Value1");

            algorithm.Add(keyData, valueData);
            ValueData value;
            Assert.IsTrue(algorithm.TryGet(keyData, out value));
            Assert.AreEqual(valueData, value);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_Add_Clear_Get()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData = new KeyData(1, "Key1");
            var valueData = new ValueData(1, "Value1");

            algorithm.Add(keyData, valueData);
            algorithm.Clear();

            ValueData value;
            Assert.IsFalse(algorithm.TryGet(keyData, out value));
            Assert.AreEqual(null, value);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_Add2Items_GetSecond()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData1 = new KeyData(1, "Key1");
            var valueData1 = new ValueData(1, "Value1");

            var keyData2 = new KeyData(2, "Key2");
            var valueData2 = new ValueData(2, "Value2");

            algorithm.Add(keyData1, valueData1);
            algorithm.Add(keyData2, valueData2);

            ValueData value;
            Assert.IsTrue(algorithm.TryGet(keyData2, out value));
            Assert.AreEqual(valueData2, value);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_Add1_GetNone()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData1 = new KeyData(1, "Key1");
            var valueData1 = new ValueData(1, "Value1");

            var keyData2 = new KeyData(2, "Key2");


            algorithm.Add(keyData1, valueData1);

            ValueData value;
            Assert.IsFalse(algorithm.TryGet(keyData2, out value));
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_Add4_LastShouldBeOverridden()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData1 = new KeyData(1, "Key1");
            var valueData1 = new ValueData(1, "Value1");

            var keyData2 = new KeyData(2, "Key2");
            var valueData2 = new ValueData(2, "Value2");

            var keyData3 = new KeyData(3, "Key3");
            var valueData3 = new ValueData(3, "Value3");

            var keyData4 = new KeyData(4, "Key4");
            var valueData4 = new ValueData(4, "Value4");

            algorithm.Add(keyData1, valueData1);
            algorithm.Add(keyData2, valueData2);
            algorithm.Add(keyData3, valueData3);
            algorithm.Add(keyData4, valueData4);

            ValueData value3;
            Assert.IsFalse(algorithm.TryGet(keyData3, out value3));

            ValueData value1;
            Assert.IsTrue(algorithm.TryGet(keyData1, out value1));
            Assert.AreEqual(valueData1, value1);

            ValueData value2;
            Assert.IsTrue(algorithm.TryGet(keyData2, out value2));
            Assert.AreEqual(valueData2, value2);

            ValueData value4;
            Assert.IsTrue(algorithm.TryGet(keyData4, out value4));
            Assert.AreEqual(valueData4, value4);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_Add4_LeastUsedShouldBeOverridden()
        {
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(3);

            var keyData1 = new KeyData(1, "Key1");
            var valueData1 = new ValueData(1, "Value1");

            var keyData2 = new KeyData(2, "Key2");
            var valueData2 = new ValueData(2, "Value2");

            var keyData3 = new KeyData(3, "Key3");
            var valueData3 = new ValueData(3, "Value3");

            var keyData4 = new KeyData(4, "Key4");
            var valueData4 = new ValueData(4, "Value4");

            algorithm.Add(keyData1, valueData1);
            algorithm.Add(keyData2, valueData2);
            algorithm.Add(keyData3, valueData3);

            ValueData value;
            algorithm.TryGet(keyData1, out value);

            algorithm.Add(keyData4, valueData4);


            ValueData value1;
            Assert.IsFalse(algorithm.TryGet(keyData1, out value1));

            ValueData value2;
            Assert.IsTrue(algorithm.TryGet(keyData2, out value2));
            Assert.AreEqual(valueData2, value2);

            ValueData value3;
            Assert.IsTrue(algorithm.TryGet(keyData3, out value3));
            Assert.AreEqual(valueData3, value3);

            ValueData value4;
            Assert.IsTrue(algorithm.TryGet(keyData4, out value4));
            Assert.AreEqual(valueData4, value4);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_MultiThreading()
        {
            int elementNumber = 100;
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(elementNumber / 4);

            var dataList = TestDataHelper.Generate(elementNumber);

            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, algorithm);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, algorithm);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, algorithm);
            var task4 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, algorithm);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.IsTrue(totalHintCount > 0, "At least one cache hit");
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_MultiThreading_AllFromCache()
        {
            int elementNumber = 100;
            var algorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(100);

            var dataList = TestDataHelper.Generate(elementNumber);
            var task1 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, algorithm);
            var task2 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, algorithm);
            var task3 = TestHelper.AddAndGetDataFromCacheInReverseOrder(dataList, algorithm);
            var task4 = TestHelper.AddAndGetDataFromCacheInOrder(dataList, algorithm);


            var taskList = new List<Task<int>> { task1, task2, task3, task4 };
            Task.WaitAll(taskList.ToArray());

            int totalHintCount = 0;
            taskList.ForEach(t => totalHintCount += t.Result);

            Debug.WriteLine($"Cache hit {totalHintCount} out of {taskList.Count * elementNumber}");
            Assert.AreEqual(300, totalHintCount);
        }

        [TestMethod]
        public void MostRecentlyUsedReplacementAlgorithm_KeyIsNullObject()
        {
            var replacementAlgorithm = new MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>(5);

            ValueData value;
            Assert.IsFalse(replacementAlgorithm.TryGet(null, out value));

            var value1 = new ValueData(1, "Value 1");
            replacementAlgorithm.Add(null, null);
            Assert.IsFalse(replacementAlgorithm.TryGet(null, out value));
            replacementAlgorithm.Add(null, value1);
            Assert.IsFalse(replacementAlgorithm.TryGet(null, out value));
        }
    }
}
