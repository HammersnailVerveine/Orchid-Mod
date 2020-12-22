using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class EarthRegeneration : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Earth Regeneration");
			Description.SetDefault("Increased damage resistance and life regeneration");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.endurance  += 0.1f;
			modPlayer.lifeRegen += 2;
		}
    }
}