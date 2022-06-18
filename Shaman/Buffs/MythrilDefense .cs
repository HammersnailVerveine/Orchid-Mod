using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class MythrilDefense : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Mythril Defense");
			Description.SetDefault("Increases defense by 8");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 8;
		}
	}
}