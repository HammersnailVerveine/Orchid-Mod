using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Thorium
{
    public class GraniteAura : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Granite Aura");
			Description.SetDefault("Granite energy orbits around you");
        }
    }
}