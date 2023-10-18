using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class ShamanicBaubles : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Shamanic Baubles");
			// Description.SetDefault("Your next hit will give you a shamanic orb for free");
		}
	}
}