using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
	public class QueenBeeFlaskBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Human Beehive");
			// Description.SetDefault("Attacking with 3 or more element will summon bees");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}