// <author>
// Shawn Quereshi
// </author>
namespace RgbUnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RgbHidLibrary;

    /// <summary>
    /// Tests related to the environment.
    /// </summary>
    [TestClass]
    public class Environment
    {
        /// <summary>
        /// Makes sure the device is discoverable.
        /// </summary>
        [TestMethod]
        public void CreateDevice()
        {
            RgbHidBase rgbDevice;
            Assert.IsTrue(RgbHid.TryCreate(RgbDeviceType.CorsairK70Rgb, out rgbDevice));
        }
    }
}
