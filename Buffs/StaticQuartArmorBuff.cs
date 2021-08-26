using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Buffs
{
	public class StaticQuartArmorBuff : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Static Power");
			Description.SetDefault("10% increased damage and movement speed");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.allDamage += 0.1f;
			player.moveSpeed += 0.1f;
		}
	}
}