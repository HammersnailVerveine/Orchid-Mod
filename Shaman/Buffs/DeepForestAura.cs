using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class DeepForestAura : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			DisplayName.SetDefault("Deep Forest Aura");
			Description.SetDefault("Sharp Leaves orbits around you");
		}
	}
}