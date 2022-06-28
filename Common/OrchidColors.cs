using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidMod.Common
{
	public static class OrchidColors
	{
		public static readonly Color AlchemistTag = new(155, 255, 55);

		public static readonly Color CrossmodContentWarning = new(255, 130, 110);

		public static Color GetClassTagColor(ClassTags tag)
		{
			return tag switch
			{
				ClassTags.Alchemist => AlchemistTag,
				_ => throw new NotImplementedException()
			};
		}	
	}
}