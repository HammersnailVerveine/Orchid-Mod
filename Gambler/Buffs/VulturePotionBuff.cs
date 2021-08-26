using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Buffs
{
	public class VulturePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Scavenger");
			Description.SetDefault("20% increased chip generation");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerChipChance += 0.2f;
		}
	}
}