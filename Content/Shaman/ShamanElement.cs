using Microsoft.Xna.Framework;

namespace OrchidMod.Content.Shaman
{
	public enum ShamanElement : int
	{
		NULL = 0,
		FIRE = 1,
		WATER = 2,
		AIR = 3,
		EARTH = 4,
		SPIRIT = 5
	}

	public class ShamanElementUtils
	{
		public static Color GetColor(ShamanElement element)
		{
			switch (element)
			{
				case ShamanElement.FIRE: 
					return new Color(195, 41, 44);
				case ShamanElement.WATER:
					return new Color(13, 107, 216);
				case ShamanElement.AIR:
					return new Color(33, 184, 115);
				case ShamanElement.SPIRIT:
					return new Color(192, 48, 245);
				case ShamanElement.EARTH:
					return new Color(255, 221, 62);
				default:
					return Color.White;
			}
		}
	}
}