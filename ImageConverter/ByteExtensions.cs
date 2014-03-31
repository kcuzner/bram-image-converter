using System;

namespace ImageConverter
{
	public static class ByteExtensions
    {
		/// <summary>
		/// Converts byte to a 2 bit string
		/// </summary>
		/// <returns>The two bit string.</returns>
		/// <param name="b">The byte</param>
		public static string ToTwoBitString(this byte b)
		{
			int data = Math.Min(4, b >> 6);

			return Convert.ToString(data, 2).PadLeft(2, '0');
		}

		/// <summary>
		/// Converts a byte to a 3 bit string
		/// </summary>
		/// <returns>The three bit string.</returns>
		/// <param name="b">The byte</param>
		public static string ToThreeBitString(this byte b)
		{
			int data = Math.Min(7, b >> 5);

			return Convert.ToString(data, 2).PadLeft(3, '0');
		}
    }
}

