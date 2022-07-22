using System;

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
	}
}