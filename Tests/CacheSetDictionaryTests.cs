using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NWaySetAssociativeCache.Core;

namespace Tests
{
    public class CacheSetDictionaryTests
    {
        [TestMethod]
        public void CacheSetDictionary_EagerLoading()
        {
            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);

            replacementAlgorithmProvider.Setup(p => p.Create(0));
            replacementAlgorithmProvider.Setup(p => p.Create(1));
            replacementAlgorithmProvider.Setup(p => p.Create(2));

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.Create(3, replacementAlgorithmProvider.Object);
            Assert.IsNotNull(cacheSetDictionary);
            Assert.AreEqual(3, cacheSetDictionary.Count);

            replacementAlgorithm.VerifyAll();
        }

        [TestMethod]
        public void CacheSetDictionary_LazyLoading()
        {
            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.CreateLazyLoading(3, replacementAlgorithmProvider.Object);
            Assert.IsNotNull(cacheSetDictionary);
            Assert.AreEqual(0, cacheSetDictionary.Count);

            replacementAlgorithm.VerifyAll();
        }

        [TestMethod]
        public void CacheSetDictionary_Clear()
        {
            var replacementAlgorithm = new Mock<ICache<KeyData, ValueData>>(MockBehavior.Strict);
            var replacementAlgorithmProvider = new Mock<IReplacementAlgorithmProvider<KeyData, ValueData>>(MockBehavior.Strict);

            replacementAlgorithmProvider.Setup(p => p.Create(0));
            replacementAlgorithmProvider.Setup(p => p.Create(1));
            replacementAlgorithmProvider.Setup(p => p.Create(2));

            var cacheSetDictionary = CacheSetDictionaryProvider<KeyData, ValueData>.Create(3, replacementAlgorithmProvider.Object);
            Assert.IsNotNull(cacheSetDictionary);
            Assert.AreEqual(3, cacheSetDictionary.Count);

            cacheSetDictionary.Clear();
            Assert.AreEqual(0, cacheSetDictionary.Count);

            replacementAlgorithm.VerifyAll();
        }
    }
}
