using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class AmberEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Amber Empowerment");
			// Description.SetDefault("Increases maximum life by 20");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.statLifeMax2 += 20;
		}
	}
}