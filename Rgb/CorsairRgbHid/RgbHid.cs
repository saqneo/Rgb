// <author>
// Shawn Quereshi
// </author>
namespace RgbHidLibrary
{
    using System.Linq;
    using HidLibrary;

    /// <summary>
    /// The device type
    /// </summary>
    public enum RgbDeviceType
    {
        CorsairK70Rgb
    }

    /// <summary>
    /// Creates RGB devices
    /// </summary>
    public static class RgbHid
    {
        /// <summary>
        /// Tries to create a <see cref="RgbHidBase"/> of the specified type.
        /// </summary>
        /// <param name="rgbDeviceType">Type of the RGB device.</param>
        /// <param name="rgbDevice">The RGB device.</param>
        /// <returns>True if the device was successfully created; otherwise, false.</returns>
        public static bool TryCreate(RgbDeviceType rgbDeviceType, out RgbHidBase rgbDevice)
        {
            rgbDevice = null;
            switch (rgbDeviceType)
            {
                case RgbDeviceType.CorsairK70Rgb:
                    var hidDevice = new HidEnumerator().Enumerate(K70RgbMappings.CorsairVendorId, K70RgbMappings.K70RgbProductId).Where(device => device.Capabilities.OutputReportByteLength == K70RgbMappings.K70RgbPayloadSize).First();

                    if (hidDevice == null)
                    {
                        return false;
                    }

                    rgbDevice = new K70Rgb(hidDevice);
                    break;
                default:
                    break;
            }

            return rgbDevice != null;
        }
    }
}
