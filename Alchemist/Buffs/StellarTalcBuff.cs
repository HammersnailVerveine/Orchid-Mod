using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
    public class StellarTalcBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Stellar Orbit");
			Description.SetDefault("Stellar Talc projectiles will orbit around you");
            Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
        }
    }
}