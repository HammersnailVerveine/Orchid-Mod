using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class ShroomHeal : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shroom Regeneration");
			Description.SetDefault("Increased life regeneration");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 3;
		}
	}
}