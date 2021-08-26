using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class ShamanicBaubles : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shamanic Baubles");
			Description.SetDefault("Your next hit will give you a shamanic orb for free");
		}
	}
}