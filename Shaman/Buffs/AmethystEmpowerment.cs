using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class AmethystEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Amethyst Empowerment");
			Description.SetDefault("Increases shamanic damage by 10%");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			modPlayer.shamanDamage += 0.1f;
		}
	}
}