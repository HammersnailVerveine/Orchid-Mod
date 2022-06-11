using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
	public class JungleLilyExtractBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purifying Lilies");
			Description.SetDefault("Each alchemical attack using 2 or more elements will release a purifying aura");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}