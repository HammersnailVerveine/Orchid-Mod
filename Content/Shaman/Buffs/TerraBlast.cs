using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class TerraBlast : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Terra Blast");
			// Description.SetDefault("Shamanic damage increased by 15%");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.15f;
		}
	}
}