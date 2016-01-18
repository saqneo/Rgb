// <author>
// Shawn Quereshi
// </author>
namespace RgbHidLibrary
{
    using System.Collections.Generic;
    using System.Drawing;
    using HidLibrary;

    /// <summary>
    /// The fit mode of an image
    /// </summary>
    public enum ImageFitMode
    {
        /// <summary>
        /// Letterbox the image so it will fit on the keyboard without stretching
        /// </summary>
        Fit, // Letterbox

        /// <summary>
        /// Fill the keyboard without stretching the image, some will be cropped out
        /// </summary>
        Fill,

        /// <summary>
        /// Stretch the image to fit exactly onto the device
        /// </summary>
        Stretch
    }

    /// <summary>
    /// The base classes for RGB HIDs.
    /// </summary>
    public abstract class RgbHidBase
    {
        /// <summary>
        /// The underlying HID
        /// </summary>
        protected readonly IHidDevice Hid;

        /// <summary>
        /// Initializes a new instance of the <see cref="RgbHidBase"/> class.
        /// </summary>
        /// <param name="hid">The underlying device handle.</param>
        public RgbHidBase(IHidDevice hid)
        {
            this.Hid = hid;
        }

        /// <summary>
        /// Lights the keyboard with the LED keys and colors.
        /// </summary>
        /// <param name="keyColors">The key colors.</param>
        public abstract void DrawKeys(IDictionary<uint, Color> keyColors);

        /// <summary>
        /// Lights the keyboard with the LED keys and colors, using the specified default color for unlisted keys.
        /// </summary>
        /// <param name="keyColors">The key colors.</param>
        /// <param name="defaultColor">The default color for unspecified keys.</param>
        public abstract void DrawKeys(IDictionary<uint, Color> keyColors, Color defaultColor);

        /// <summary>
        /// Draws the source image the device.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="fitMode">The fit mode.</param>
        public abstract void DrawImage(Image sourceImage, ImageFitMode fitMode);
    }
}
