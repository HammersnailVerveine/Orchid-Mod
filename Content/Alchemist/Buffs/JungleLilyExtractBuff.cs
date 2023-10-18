using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Buffs
{
	public class JungleLilyExtractBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Purifying Lilies");
			// Description.SetDefault("Each alchemical attack using two or more elements releases a purifying aura");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}