using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class DiabolistCauterize : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Diabolist Cauterization");
			// Description.SetDefault("Quickly gaining health");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 50;
		}
	}
}