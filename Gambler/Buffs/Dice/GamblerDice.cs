using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Buffs.Dice
{
    public class GamblerDice : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Gambler Dice");
			Description.SetDefault("Increased gambling damage");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
        }
    }
}