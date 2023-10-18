using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class TopazEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Topaz Empowerment");
			// Description.SetDefault("Increases defense by 5");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 5;
		}
	}
}