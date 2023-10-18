using System;

namespace OrchidMod.Content.Gambler
{
	[Flags]
	public enum GamblerCardSets : byte
	{
		Without = 0,

		Biome = 1 << 0,
		Boss = 1 << 1,
		Elemental = 1 << 2,
		Slime = 1 << 3
	}
}