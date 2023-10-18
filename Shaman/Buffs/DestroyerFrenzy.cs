using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class DestroyerFrenzy : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Destroyer Frenzy");
			// Description.SetDefault("15% increased shamanic damage and critical strike damage");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.15f;
			modPlayer.shamanDestroyerCount = 0;
		}
	}
}