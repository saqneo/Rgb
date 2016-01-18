// <author>
// Shawn Quereshi
// </author>
namespace RgbUnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RgbHidLibrary;

    /// <summary>
    /// Verifies the implementation has not broken.
    /// </summary>
    [TestClass]
    public class Implementation
    {
        /// <summary>
        /// Makes sure the mappings arrays are the same dimensions, as expected by the implementation.
        /// </summary>
        [TestMethod]
        public void VerifyK70RgbMappings()
        {
            Assert.AreEqual(K70RgbMappings.KeyPayloadBitsLocation.Length, K70RgbMappings.PixelLocationX.Length);

            for (int i = 0; i < K70RgbMappings.KeyPayloadBitsLocation.Length; i++)
            {
                Assert.AreEqual(K70RgbMappings.KeyPayloadBitsLocation[i].Length, K70RgbMappings.PixelLocationX[i].Length);
            }
        }
    }
}
