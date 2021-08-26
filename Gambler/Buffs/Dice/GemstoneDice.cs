using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Buffs.Dice
{
	public class GemstoneDice : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Gemstone Die");
			Description.SetDefault("Increases chances not to consume chips");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}