using Microsoft.VisualStudio.TestTools.UnitTesting;
// <author>
// Shawn Quereshi
// </author>
namespace RgbUnitTest
{
    using RgbHidLibrary;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Tests which need to be observed on the device to verify fully.
    /// </summary>
    [TestClass]
    public class Observable
    {
        /// <summary>
        /// Tests the keys row by row.
        /// </summary>
        [TestMethod]
        public void TestKeys()
        {
            RgbHidBase rgbDevice;
            Assert.IsTrue(RgbHid.TryCreate(RgbDeviceType.CorsairK70Rgb, out rgbDevice));

            var keyboard = rgbDevice as K70Rgb;
            Assert.IsNotNull(keyboard);

            foreach (var row in K70RgbMappings.KeyPayloadBitsLocation)
            {
                foreach (var key in row)
                {
                    var dict = new Dictionary<uint, Color>();
                    dict[(uint)key] = Color.Red;
                    keyboard.DrawKeys(dict, Color.White);
                }
            }
        }

        /// <summary>
        /// Tests rawing to fill the keyboard.
        /// </summary>
        [TestMethod]
        public void TestDrawFill()
        {
            var image = Image.FromFile(@"Resources\testImage.png");

            RgbHidBase rgbDevice;
            Assert.IsTrue(RgbHid.TryCreate(RgbDeviceType.CorsairK70Rgb, out rgbDevice));
            rgbDevice.DrawImage(image, ImageFitMode.Fill);
        }

        /// <summary>
        /// Tests drawing to fit the keyboard.
        /// </summary>
        [TestMethod]
        public void TestDrawFit()
        {
            var image = Image.FromFile(@"Resources\testImage.png");

            RgbHidBase rgbDevice;
            Assert.IsTrue(RgbHid.TryCreate(RgbDeviceType.CorsairK70Rgb, out rgbDevice));
            rgbDevice.DrawImage(image, ImageFitMode.Fit);
        }

        /// <summary>
        /// Tests stretching the image to fit on the kbyard.
        /// </summary>
        [TestMethod]
        public void TestDrawStretch()
        {
            var image = Image.FromFile(@"Resources\testImage.png");

            RgbHidBase rgbDevice;
            Assert.IsTrue(RgbHid.TryCreate(RgbDeviceType.CorsairK70Rgb, out rgbDevice));
            rgbDevice.DrawImage(image, ImageFitMode.Stretch);
        }
    }
}
