using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class ShadowEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shadow Empowerment");
			Description.SetDefault("Increases movement speed and shamanic damage by 10%");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.moveSpeed += 0.1f;
			modPlayer.shamanDamage += 0.1f;
			modPlayer.shamanShadowEmpowerment = true;
		}
	}
}