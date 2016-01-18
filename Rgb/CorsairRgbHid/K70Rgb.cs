// <author>
// Shawn Quereshi
// </author>
namespace RgbHidLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using HidLibrary;
    using static K70RgbMappings;

    /// <summary>
    /// A class for the Corsair K70 RGB keyboard.
    /// </summary>
    /// <seealso cref="RgbHidLibrary.RgbHidBase" />
    public sealed class K70Rgb : RgbHidBase
    {
        /// <summary>
        /// The width of an image supported by the keyboard
        /// </summary>
        private const int Width = 68;

        /// <summary>
        /// The height of an image supported by the keyboard
        /// </summary>
        private const int Height = 19;

        /// <summary>
        /// The ratio of width to height
        /// </summary>
        private const float Ratio = Width / (float)Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="K70Rgb"/> class.
        /// </summary>
        /// <param name="hidDevice">The hid device.</param>
        public K70Rgb(IHidDevice hidDevice) : base(hidDevice)
        {
        }

        /// <summary>
        /// Lights the keyboard with the specified keys and colors.
        /// </summary>
        /// <param name="keyColors">The key colors.</param>
        public override void DrawKeys(IDictionary<uint, Color> keyColors)
        {
            this.DrawKeys(keyColors, Color.White);
        }

        /// <summary>
        /// Lights the keyboard with the specified keys and colors, using the specified default color for unlisted keys.
        /// </summary>
        /// <param name="keyColors">The key colors.</param>
        /// <param name="defaultColor">The default color for unspecified keys.</param>
        public override void DrawKeys(IDictionary<uint, Color> keyColors, Color defaultColor)
        {
            var redPayload = new byte[72];
            var greenPayload = new byte[72];
            var bluePayload = new byte[72];
            var redDefault = (byte)(defaultColor.R ^ byte.MaxValue);
            var greenDefault = (byte)(defaultColor.G ^ byte.MaxValue);
            var blueDefault = (byte)(defaultColor.B ^ byte.MaxValue);

            for (uint i = 0; i < 72; i++)
            {
                var keyIndex = i * 2;

                byte red;
                byte green;
                byte blue;

                Color color;
                if (keyColors.TryGetValue(keyIndex, out color))
                {
                    red = (byte)(((color.R ^ byte.MaxValue) >> 1) & 0x70);
                    green = (byte)(((color.G ^ byte.MaxValue) >> 1) & 0x70);
                    blue = (byte)(((color.B ^ byte.MaxValue) >> 1) & 0x70);
                }
                else
                {
                    red = (byte)(redDefault >> 1);
                    green = (byte)(greenDefault >> 1);
                    blue = (byte)(blueDefault >> 1);
                }

                keyIndex = (i * 2) + 1;
                if (keyColors.TryGetValue(keyIndex, out color))
                {
                    red |= (byte)((color.R ^ byte.MaxValue) >> 5);
                    green |= (byte)((color.G ^ byte.MaxValue) >> 5);
                    blue |= (byte)((color.B ^ byte.MaxValue) >> 5);
                }
                else
                {
                    red |= (byte)(redDefault >> 5);
                    green |= (byte)(greenDefault >> 5);
                    blue |= (byte)(blueDefault >> 5);
                }

                redPayload[i] = red;
                greenPayload[i] = green;
                bluePayload[i] = blue;
            }

            var payloads = CorsairRgbKeyboardPacket.GetPackets(redPayload, greenPayload, bluePayload);
            Hid.OpenDevice();
            foreach (var payload in payloads)
            {
                Hid.WriteReport(new HidReport(Hid.Capabilities.OutputReportByteLength, new HidDeviceData(new byte[] { 0x00 }.Concat(payload.GetBytes()).ToArray<byte>(), HidDeviceData.ReadStatus.Success)));
            }

            Hid.CloseDevice();
        }

        /// <summary>
        /// Draws an image to the keyboard.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="fitMode">The mode describing how to fit the image to the keyboard.</param>
        public override void DrawImage(Image sourceImage, ImageFitMode fitMode)
        {
            var newImage = this.GetRescaledImage(sourceImage, fitMode);

            var data = newImage.LockBits(new Rectangle(new Point(0, 0), newImage.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            byte[] content = new byte[Height * Width * 3];
            try
            {
                IntPtr ptr = data.Scan0;

                Marshal.Copy(ptr, content, 0, content.Length);
            }
            finally
            {
                newImage.UnlockBits(data);
            }
            
            var dict = new Dictionary<uint, Color>();
            for (int i = 0; i < K70RgbMappings.PixelLocationX.Length; i++)
            {
                for (int j = 0; j < K70RgbMappings.PixelLocationX[i].Length; j++)
                {
                    int totalRed = 0;
                    int totalGreen = 0;
                    int totalBlue = 0;
                    for (int x = K70RgbMappings.PixelLocationX[i][j]; x < K70RgbMappings.PixelLocationX[i][j] + 3; x++)
                    {
                        for (int y = K70RgbMappings.PixelRowLocationY[i]; y < K70RgbMappings.PixelRowLocationY[i] + 3; y++)
                        {
                            totalBlue += content[(y * Width * 3) + (x * 3)];
                            totalGreen += content[(y * Width * 3) + (x * 3) + 1];
                            totalRed += content[(y * Width * 3) + (x * 3) + 2];
                        }
                    }

                    dict[(uint)K70RgbMappings.KeyPayloadBitsLocation[i][j]] = Color.FromArgb(totalRed / 9, totalGreen / 9, totalBlue / 9);
                }
            }

            dict[(uint)K70Keys.Brightness] = Color.White;
            dict[(uint)K70Keys.Gaming] = Color.White;
            dict[(uint)K70Keys.VolumeMute] = Color.White;
            dict[(uint)K70Keys.MediaNextTrack] = Color.White;
            dict[(uint)K70Keys.MediaPlayPause] = Color.White;
            dict[(uint)K70Keys.MediaPreviousTrack] = Color.White;
            dict[(uint)K70Keys.MediaNextTrack] = Color.White;

            this.DrawKeys(dict);
        }

        /// <summary>
        /// Rescales the source image to an image that fits the keyboard.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="fitMode">The fit mode.</param>
        /// <returns>An image fitting the keyboard.</returns>
        private Bitmap GetRescaledImage(Image sourceImage, ImageFitMode fitMode)
        {
            float sourceImageRatio = sourceImage.Size.Width / (float)sourceImage.Size.Height;

            float rescale;
            var sourceRectangle = new Rectangle(0, 0, sourceImage.Size.Width, sourceImage.Size.Height);
            var destinationRectangle = new Rectangle(0, 0, Width, Height);
            switch (fitMode)
            {
                case ImageFitMode.Fill:
                    // Partial source image, change source rectangle
                    if (sourceImageRatio > Ratio)
                    {
                        // Wider, Y is too small
                        rescale = Height / (float)sourceImage.Size.Height;
                        sourceRectangle.X = (int)(sourceImage.Size.Width * rescale / 2);
                    }
                    else
                    {
                        // Taller, X is too small
                        rescale = Width / (float)sourceImage.Size.Width;
                        sourceRectangle.Height = (int)(sourceImage.Size.Height * (sourceImageRatio / Ratio));
                        sourceRectangle.Y = (int)(sourceImage.Size.Height - sourceRectangle.Height) / 2;
                      }
                    
                    break;
                case ImageFitMode.Fit:
                    // Partial drawn image, change drawn rectangle
                    if (sourceImageRatio > Ratio)
                    {
                        // Wider, X is too large
                        rescale = Width / (float)sourceImage.Size.Width;
                        destinationRectangle.Height = (int)(sourceImage.Size.Height * rescale);
                        destinationRectangle.Y = (int)(Height - (sourceImage.Size.Height * rescale)) / 2;
                    }
                    else
                    {
                        // Taller, Y is too large
                        rescale = Height / (float)sourceImage.Size.Height;
                        destinationRectangle.Width = (int)(sourceImage.Size.Width * rescale);
                        destinationRectangle.X = (int)(Width - (sourceImage.Size.Width * rescale)) / 2;
                    }
                    
                    break;
                case ImageFitMode.Stretch:
                default:
                    // All of source and drawn, use defaults
                    break;
            }
            
            var newImage = new Bitmap(Width, Height);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(sourceImage, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }

            return newImage;
        }

        /// <summary>
        /// Gets the template image.
        /// </summary>
        /// <returns>A blank image matching the dimensions of the keyboard.</returns>
        private Bitmap GetTemplateImage()
        {
            return new Bitmap(Width, Height);
        }
    }
}
