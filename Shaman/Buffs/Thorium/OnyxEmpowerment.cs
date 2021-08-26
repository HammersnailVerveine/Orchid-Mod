using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Thorium
{
	public class OnyxEmpowerment : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Onyx Empowerment");
			Description.SetDefault("Increases armor penetration by 3");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.armorPenetration += 3;
		}
	}
}