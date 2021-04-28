using SevenZip.Compression.LZMA;
using System;
using System.IO;

namespace Circles.Utils
{
    public static class LzmaUtils
    {
		public static byte[] Compress(byte[] decodedBytes)
		{
			var encoder = new Encoder();

			using var encodedStream = new MemoryStream();
			using var decodedStream = new MemoryStream(decodedBytes);

			encoder.WriteCoderProperties(encodedStream);

			for (int i = 0; i < 8; i++)
				encodedStream.WriteByte((byte)(decodedStream.Length >> (8 * i)));

			encoder.Code(decodedStream, encodedStream, -1, -1, null);

			return encodedStream.ToArray();
		}

		public static byte[] Decompress(byte[] encodedBytes)
		{
			var decoder = new Decoder();

			using var encodedStream = new MemoryStream(encodedBytes);
			using var decodedStream = new MemoryStream();

			var properties = new byte[5];
			var length = new byte[8];

			if (encodedStream.Read(properties, 0, 5) != 5)
				throw new InvalidDataException("LZMA encoded stream was too short");

			encodedStream.Read(length, 0, 8);

			decoder.SetDecoderProperties(properties);
			decoder.Code(encodedStream, decodedStream, encodedStream.Length, BitConverter.ToInt64(length, 0), null);

			return decodedStream.ToArray();
		}
	}
}
