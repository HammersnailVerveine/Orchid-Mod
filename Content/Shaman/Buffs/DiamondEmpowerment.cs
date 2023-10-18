using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class DiamondEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Diamond Empowerment");
			// Description.SetDefault("Increases the duration of your shamanic bonds by 3 seconds");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanBuffTimer += 3;
		}
	}
}