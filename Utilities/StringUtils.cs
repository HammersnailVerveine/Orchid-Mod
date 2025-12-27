using System;
using System.Globalization;

namespace OrchidMod.Utilities
{
	public static partial class OrchidUtils
	{
		public static string FirstCharToUpper(this string input, bool toLower = false)
		{
			var str = toLower ? input.ToLower() : input;
			return str switch
			{
				null => throw new ArgumentNullException(nameof(input)),
				"" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
				_ => string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1))
			};
		}

		public static string FramesToSeconds(int frames)
		{
			string seconds = Math.DivRem(frames, 60, out int centiseconds).ToString();
			if (centiseconds == 0) return seconds;
			centiseconds = (int)(centiseconds / 0.6f + 0.5f);
			if (centiseconds % 10 == 0) centiseconds /= 10;
			string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			return seconds + separator + centiseconds;
		}
	}
}