using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Buffs
{
    public class JungleLilyExtractBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Purifying Lilies");
			Description.SetDefault("Each alchemical attack using 2 or more elements will release a purifying aura");
            Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
        }
    }
}