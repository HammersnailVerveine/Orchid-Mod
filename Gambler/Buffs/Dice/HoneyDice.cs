using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Buffs.Dice
{
	public class HoneyDice : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Honey Die");
			Description.SetDefault("Heals on gambling critical strikes");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}