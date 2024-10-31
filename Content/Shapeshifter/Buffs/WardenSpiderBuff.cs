using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Buffs
{
	public class WardenSpiderBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 15;
		}
	}
}