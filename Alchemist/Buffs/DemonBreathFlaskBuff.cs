using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Buffs
{
    public class DemonBreathFlaskBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Demon Reek");
			Description.SetDefault("Demon breath will create more projectiles");
            Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
        }
    }
}