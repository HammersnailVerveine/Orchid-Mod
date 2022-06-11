using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
	public class SlimeFlaskBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Samples");
			Description.SetDefault("Using slimy samples with a fire element will release damaging embers");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}