using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class ShadowEmpowerment : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shadow Empowerment");
			Description.SetDefault("Increases movement speed and shamanic damage by 10%");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			Main.player[Main.myPlayer].moveSpeed += 0.1f;
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanDamage += 0.1f;
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanShadowEmpowerment = true;
		}
	}
}