using System;

namespace OrchidMod.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CrossmodContentAttribute : Attribute
	{
		public readonly string[] Mods;

		public CrossmodContentAttribute(params string[] mods)
		{
			Mods = mods.Length > 0 ? mods : new string[] { "Unknown" };
		}
	}
}