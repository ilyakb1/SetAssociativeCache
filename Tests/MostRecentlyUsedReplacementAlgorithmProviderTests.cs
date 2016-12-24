using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Core.ReplacementAlgorithm;

namespace Tests
{
    [TestClass]
    public class MostRecentlyUsedReplacementAlgorithmProviderTests
    {
        [TestMethod]
        public void LeastRecentlyUsedReplacementAlgorithmProvider_Create()
        {
            var provider = new MostRecentlyUsedReplacementAlgorithmProvider<KeyData, ValueData>(10);

            Assert.IsInstanceOfType(provider.Create(5),
                typeof(MostRecentlyUsedReplacementAlgorithm<KeyData, ValueData>));
        }
    }
}
