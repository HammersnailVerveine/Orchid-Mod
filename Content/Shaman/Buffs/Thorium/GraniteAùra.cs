using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs.Thorium
{
	public class GraniteAura : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Granite Aura");
			// Description.SetDefault("Granite energy orbits around you");
		}
	}
}