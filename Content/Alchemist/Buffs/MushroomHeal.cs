using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Buffs
{
	public class MushroomHeal : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom Spores");
			// Description.SetDefault("Alchemist attacks will create more spores");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}