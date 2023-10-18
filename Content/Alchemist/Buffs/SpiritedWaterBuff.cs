using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Buffs
{
	public class SpiritedWaterBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spirited Droplets");
			// Description.SetDefault("Chemical attacks will release spirited water flames");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}