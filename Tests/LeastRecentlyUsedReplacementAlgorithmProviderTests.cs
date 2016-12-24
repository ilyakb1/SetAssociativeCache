using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Core.ReplacementAlgorithm;

namespace Tests
{
    [TestClass]
    public class LeastRecentlyUsedReplacementAlgorithmProviderTests
    {
        [TestMethod]
        public void LeastRecentlyUsedReplacementAlgorithmProvider_Create()
        {
            var provider = new LeastRecentlyUsedReplacementAlgorithmProvider<KeyData, ValueData>(10);

            Assert.IsInstanceOfType(provider.Create(5),
                typeof(LeastRecentlyUsedReplacementAlgorithm<KeyData, ValueData>));
        }
    }
}
