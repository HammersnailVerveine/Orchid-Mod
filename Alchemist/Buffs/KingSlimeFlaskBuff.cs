using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
	public class KingSlimeFlaskBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Poisonous Slime");
			Description.SetDefault("Increases the likelyhood of spawning slime bubbles, creating spiked jungle slimes");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}