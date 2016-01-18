// <author>
// Shawn Quereshi
// </author>
namespace RgbHidLibrary
{
    /// <summary>
    /// Key mappings to pixels (if using image, scale to 67 x 19 and use 3x3 aggregation of surrounding pixels per LED)
    /// </summary>
    internal static class K70RgbMappings
    {
        /// <summary>
        /// The corsair vendor identifier
        /// </summary>
        public const int CorsairVendorId = 6940;

        /// <summary>
        /// The K70 RGB product identifier
        /// </summary>
        public const int K70RgbProductId = 6931;

        /// <summary>
        /// The K70 RGB payload size
        /// </summary>
        public const short K70RgbPayloadSize = 65;

        /// <summary>
        /// The mapping of K70 keys to their location in the payload
        /// </summary>
        public enum K70Keys : uint
        {
            Brightness = 136,
            Gaming = 9,
            VolumeMute = 21,

            // First row
            Escape = 1,
            F1 = 13,
            F2 = 25,
            F3 = 37,
            F4 = 49,
            F5 = 61,
            F6 = 73,
            F7 = 85,
            F8 = 97,
            F9 = 109,
            F10 = 121,
            F11 = 133,
            F12 = 7,
            PrintScreen = 19,
            Scroll = 31,
            Pause = 43,
            MediaStop = 33,
            MediaPreviousTrack = 45,
            MediaPlayPause = 57,
            MediaNextTrack = 69,

            // Second Row
            OemTilde = 0,
            D1 = 12,
            D2 = 24,
            D3 = 36,
            D4 = 48,
            D5 = 60,
            D6 = 72,
            D7 = 84,
            D8 = 96,
            D9 = 108,
            D0 = 120,
            OemMinus = 132,
            OemPlus = 6,
            Back = 30,
            Insert = 55,
            Home = 67,
            PageUp = 79,
            NumLock = 81,
            Divide = 93,
            Multiply = 105,
            Subtract = 117,

            // Third row
            Tab = 3,
            Q = 15,
            W = 27,
            E = 39,
            R = 51,
            T = 63,
            Y = 75,
            U = 87,
            I = 99,
            O = 111,
            P = 123,
            OemOpenBrackets = 135,
            OemCloseBrackets = 91,
            OemBackslash = 103,
            Delete = 42,
            End = 54,
            PageDown = 66,
            NumPad7 = 8,
            NumPad8 = 20,
            NumPad9 = 32,
            Plus = 129,

            // Fourth row
            CapsLock = 2,
            A = 14,
            S = 26,
            D = 38,
            F = 50,
            G = 62,
            H = 74,
            J = 86,
            K = 98,
            L = 110,
            OemSemicolon = 122,
            OemQuotes = 134,
            Return = 127,
            NumPad4 = 56,
            NumPad5 = 68,
            NumPad6 = 80,

            // Fifth row
            LeftShift = 5,
            Z = 29,
            X = 41,
            C = 53,
            V = 65,
            B = 77,
            N = 89,
            M = 101,
            OemComma = 113,
            OemPeriod = 125,
            OemQuestion = 137,
            RightShift = 78,
            Up = 102,
            NumPad1 = 92,
            NumPad2 = 104,
            NumPad3 = 116,
            Enter = 141,

            // Bottom row
            LeftCtrl = 4,
            LWin = 16,
            LeftAlt = 28,
            Space = 52,
            RightAlt = 88,
            RWin = 100,
            Context = 112,
            RightCtrl = 90,
            Left = 114,
            Down = 126,
            Right = 138,
            NumPad0 = 128,
            Decimal = 140
        }

        /// <summary>
        /// Gets an array containing the x-coordinate of the pixel location for each LED.
        /// </summary>
        public static int[][] PixelLocationX { get; } = new int[][]
        {
            // new int[] { 46, 49, 59 } // above top row, e.g. mute
            new int[] { 0, 5, 8, 11, 14, 19, 22, 25, 28, 33, 36, 39, 42, 46, 49, 52, 56, 59, 62, 65 }, // function row
            new int[] { 0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 40 /* 40.5 */, 46, 49, 52, 56, 59, 62, 65 }, // number row
            new int[] { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 41, 46, 49, 52, 56, 59, 62, 65 }, // qwerty row
            new int[] { 1, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35, 40 /* 40.5 */, 56, 59, 62 }, // return row
            new int[] { 2, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 39, 49, 56, 59, 62, 65 }, // shift row
            new int[] { 1, 4, 8, 19, 30, 34, 37, 41, 46, 49, 52, 57 /* 57.5 */, 62 }
        };

        /// <summary>
        /// Gets an array containing the y-coordinate of the pixel location for each LED row.
        /// </summary>
        public static int[] PixelRowLocationY { get; } = new int[]
        {
            0,
            4,
            7,
            10,
            13,
            16
        };

        /// <summary>
        /// Gets an array containing the mapping of each key (based on 2D keyboard location) to its location in the payload
        /// </summary>
        public static K70Keys[][] KeyPayloadBitsLocation { get; } = new K70Keys[][]
        {
            new K70Keys[] { K70Keys.Escape, K70Keys.F1, K70Keys.F2, K70Keys.F3, K70Keys.F4, K70Keys.F5, K70Keys.F6, K70Keys.F7, K70Keys.F8, K70Keys.F9, K70Keys.F10, K70Keys.F11, K70Keys.F12, K70Keys.PrintScreen, K70Keys.Scroll, K70Keys.Pause, K70Keys.MediaStop, K70Keys.MediaPreviousTrack, K70Keys.MediaPlayPause, K70Keys.MediaNextTrack },
            new K70Keys[] { K70Keys.OemTilde, K70Keys.D1, K70Keys.D2, K70Keys.D3, K70Keys.D4, K70Keys.D5, K70Keys.D6, K70Keys.D7, K70Keys.D8, K70Keys.D9, K70Keys.D0, K70Keys.OemMinus, K70Keys.OemPlus, K70Keys.Back, K70Keys.Insert, K70Keys.Home, K70Keys.PageUp, K70Keys.NumLock, K70Keys.Divide, K70Keys.Multiply, K70Keys.Subtract },
            new K70Keys[] { K70Keys.Tab, K70Keys.Q, K70Keys.W, K70Keys.E, K70Keys.R, K70Keys.T, K70Keys.Y, K70Keys.U, K70Keys.I, K70Keys.O, K70Keys.P, K70Keys.OemOpenBrackets, K70Keys.OemCloseBrackets, K70Keys.OemBackslash, K70Keys.Delete, K70Keys.End, K70Keys.PageDown, K70Keys.NumPad7, K70Keys.NumPad8, K70Keys.NumPad9, K70Keys.Plus },
            new K70Keys[] { K70Keys.CapsLock, K70Keys.A, K70Keys.S, K70Keys.D, K70Keys.F, K70Keys.G, K70Keys.H, K70Keys.J, K70Keys.K, K70Keys.L, K70Keys.OemSemicolon, K70Keys.OemQuotes, K70Keys.Return, K70Keys.NumPad4, K70Keys.NumPad5, K70Keys.NumPad6 },
            new K70Keys[] { K70Keys.LeftShift, K70Keys.Z, K70Keys.X, K70Keys.C, K70Keys.V, K70Keys.B, K70Keys.N, K70Keys.M, K70Keys.OemComma, K70Keys.OemPeriod, K70Keys.OemQuestion, K70Keys.RightShift, K70Keys.Up, K70Keys.NumPad1, K70Keys.NumPad2, K70Keys.NumPad3, K70Keys.Enter },
            new K70Keys[] { K70Keys.LeftCtrl, K70Keys.LWin, K70Keys.LeftAlt, K70Keys.Space, K70Keys.RightAlt, K70Keys.RWin, K70Keys.Context, K70Keys.RightCtrl, K70Keys.Left, K70Keys.Down, K70Keys.Right, K70Keys.NumPad0, K70Keys.Decimal }
        };
    }
}
