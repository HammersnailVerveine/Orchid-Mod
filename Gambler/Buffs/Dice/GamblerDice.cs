using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Buffs.Dice
{
	public class GamblerDice : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Gambler Die");
			Description.SetDefault("Increased gambling damage");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}