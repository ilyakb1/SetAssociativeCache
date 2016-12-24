using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NWaySetAssociativeCache.Core;
using NWaySetAssociativeCache.Core.ReplacementAlgorithm;

namespace Tests
{
    [TestClass]
    public class DifferentCacheKeyAndValueTypesTests
    {
        [TestMethod]
        public void DifferentCacheKeyAndValueTypesTests_KeyIntValueInt()
        {
            var cacheSetDictionary = CacheSetDictionaryProvider<int, int>.Create(5, new LeastRecentlyUsedReplacementAlgorithmProvider<int, int>(3));
            var cache = new SetAssociativeCache<int, int>(new DefaultGetHashCodeSetCalculationPolicy<int>(), cacheSetDictionary);

            int value;
            Assert.IsFalse(cache.TryGet(-12, out value));
            Assert.IsFalse(cache.TryGet(0, out value));
            Assert.IsFalse(cache.TryGet(1, out value));

            cache.Add(-12, -20);
            cache.Add(-2, -12);
            cache.Add(-1, -11);
            cache.Add(0, 0);
            cache.Add(1, 0);
            cache.Add(2, 10);
            cache.Add(13, 22);

            Assert.IsFalse(cache.TryGet(-16, out value));

            int value1;
            Assert.IsTrue(cache.TryGet(-12, out value1));
            Assert.AreEqual(-20, value1);

            int value2;
            Assert.IsTrue(cache.TryGet(-2, out value2));
            Assert.AreEqual(-12, value2);

            int value3;
            Assert.IsTrue(cache.TryGet(-1, out value3));
            Assert.AreEqual(-11, value3);

            int value4;
            Assert.IsTrue(cache.TryGet(0, out value4));
            Assert.AreEqual(0, value4);

            int value5;
            Assert.IsTrue(cache.TryGet(1, out value5));
            Assert.AreEqual(0, value5);

            int value6;
            Assert.IsTrue(cache.TryGet(2, out value6));
            Assert.AreEqual(10, value6);

            int value7;
            Assert.IsTrue(cache.TryGet(13, out value7));
            Assert.AreEqual(22, value7);
        }

        [TestMethod]
        public void DifferentCacheKeyAndValueTypesTests_KeyStringValueString()
        {
            var cacheSetDictionary = CacheSetDictionaryProvider<string, string>.Create(5, new LeastRecentlyUsedReplacementAlgorithmProvider<string, string>(3));
            var cache = new SetAssociativeCache<string, string>(new DefaultGetHashCodeSetCalculationPolicy<string>(), cacheSetDictionary);

            string value;
            Assert.IsFalse(cache.TryGet("Bla", out value));
            Assert.IsFalse(cache.TryGet("", out value));
            Assert.IsFalse(cache.TryGet(null, out value));

            cache.Add("K1", "V1");
            cache.Add("K5", "V5");
            cache.Add("K2", "");
            cache.Add("", "");
            cache.Add(null, null);

            Assert.IsFalse(cache.TryGet("Bla", out value));

            string value1;
            Assert.IsTrue(cache.TryGet("K1", out value1));
            Assert.AreEqual("V1", value1);

            string value2;
            Assert.IsTrue(cache.TryGet("K5", out value2));
            Assert.AreEqual("V5", value2);

            string value3;
            Assert.IsTrue(cache.TryGet("K2", out value3));
            Assert.AreEqual("", value3);

            string value4;
            Assert.IsTrue(cache.TryGet("", out value4));
            Assert.AreEqual("", value4);

            Assert.IsFalse(cache.TryGet(null, out value));

            cache.Add("", null);
            Assert.IsFalse(cache.TryGet(null, out value));
        }

        [TestMethod]
        public void DifferentCacheKeyAndValueTypesTests_KeyIsNullObject()
        {
            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.CreateLazyLoading(5, replacementAlgorithmProvider.Object);
            var cache = new SetAssociativeCache<KeyData, ValueData>(new DefaultGetHashCodeSetCalculationPolicy<KeyData>(), cacheSetDictionary);

            ValueData value;
            Assert.IsFalse(cache.TryGet(null, out value));

            var value1 = new ValueData(1, "Value 1");
            cache.Add(null, null);
            Assert.IsFalse(cache.TryGet(null, out value));
            cache.Add(null, value1);
            Assert.IsFalse(cache.TryGet(null, out value));

            replacementAlgorithm.VerifyAll();
        }
    }
}



