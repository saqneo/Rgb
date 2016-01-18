// <author>
// Shawn Quereshi
// </author>
namespace RgbHidLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class representing payload(s) to be sent to a Corsair RGB keyboard device.
    /// </summary>
    public class CorsairRgbKeyboardPacket
    {
        /// <summary>
        /// The size of the payload in each packet
        /// </summary>
        private const int Size = 64;

        /// <summary>
        /// The reserved suffix
        /// </summary>
        private const byte ReservedSuffix = 0x00;

        /// <summary>
        /// The reserved prefix
        /// </summary>
        private CorsairPayloadPrefixByte reservedPrefix;

        /// <summary>
        /// The sequence number
        /// </summary>
        private CorsairPayloadSequenceByte sequenceNumber;

        /// <summary>
        /// The data in the payload
        /// </summary>
        private byte[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairRgbKeyboardPacket"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        private CorsairRgbKeyboardPacket(byte[] payload, CorsairPayloadSequenceByte sequenceNumber)
        {
            this.reservedPrefix = this.reservedPrefix = CorsairPayloadPrefixByte.Data;
            this.data = new byte[payload.Length];
            Buffer.BlockCopy(payload, 0, this.data, 0, payload.Length);
            this.sequenceNumber = sequenceNumber;

            if (this.sequenceNumber == CorsairPayloadSequenceByte.Flush)
            {
                this.reservedPrefix = CorsairPayloadPrefixByte.Flush;
            }
        }

        /// <summary>
        /// The payload prefix
        /// </summary>
        private enum CorsairPayloadPrefixByte
        {
            Data = 0x7f,
            Flush = 0x07
        }

        /// <summary>
        /// The packet number byte in the payload.
        /// </summary>
        private enum CorsairPayloadSequenceByte : byte
        {
            First = 0x01,
            Second = 0x02,
            Third = 0x03,
            Fourth = 0x04,
            Flush = 0x27
        }

        /// <summary>
        /// Converts the color arrays to packets for sending to a Corsair <see cref="RgbHidBase"/>.
        /// 72 bytes per color, 4 bits for each key (144 keys, some unused depending on device). Top bit reserved; 0x7 is off, 0x0 max, for a total of 9 bits of color per key. />
        /// </summary>
        /// <param name="red">The red values for individual keys.</param>
        /// <param name="green">The green values for individual keys.</param>
        /// <param name="blue">The blue values for individual keys.</param>
        /// <returns>The packets.</returns>
        /// <exception cref="System.ArgumentException">Color arrays must be 72 bytes long</exception>
        public static IEnumerable<CorsairRgbKeyboardPacket> GetPackets(byte[] red, byte[] green, byte[] blue)
        {
            if (red.Length != 72 || green.Length != 72 || blue.Length != 72)
            {
                throw new ArgumentException("Color arrays must be 72 bytes long");
            }

            var totalPayload = red.Concat(green).Concat(blue).ToArray<byte>();

            byte[] payload = new byte[60];
            Buffer.BlockCopy(totalPayload, 0, payload, 0, 60);
            yield return new CorsairRgbKeyboardPacket(payload, CorsairPayloadSequenceByte.First);

            payload = new byte[60];
            Buffer.BlockCopy(totalPayload, 60, payload, 0, 60);
            yield return new CorsairRgbKeyboardPacket(payload, CorsairPayloadSequenceByte.Second);

            payload = new byte[60];
            Buffer.BlockCopy(totalPayload, 120, payload, 0, 60);
            yield return new CorsairRgbKeyboardPacket(payload, CorsairPayloadSequenceByte.Third);

            payload = new byte[totalPayload.Length - 180];
            Buffer.BlockCopy(totalPayload, 180, payload, 0, totalPayload.Length - 180);

            yield return new CorsairRgbKeyboardPacket(payload, CorsairPayloadSequenceByte.Fourth);
            yield return new CorsairRgbKeyboardPacket(new byte[] { 0x00, 0xd8 }, CorsairPayloadSequenceByte.Flush);
        }

        /// <summary>
        /// Gets the packet bytes.
        /// </summary>
        /// <returns>The packet bytes.</returns>
        public byte[] GetBytes()
        {
            var result = new byte[Size];
            result[0] = (byte)this.reservedPrefix;
            result[1] = (byte)this.sequenceNumber;
            result[2] = (byte)this.data.Length;
            result[3] = ReservedSuffix;

            var payloadIndex = 0;
            for (int index = 4; index < Size; index++)
            {
                if (payloadIndex < this.data.Length)
                {
                    result[index] = this.data[payloadIndex];
                }
                else
                {
                    result[index] = 0x00;
                }

                payloadIndex++;
            }

            return result;
        }
    }
}
