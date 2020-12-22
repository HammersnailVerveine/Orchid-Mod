using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class SpiritRegeneration : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Spirit Regeneration");
			Description.SetDefault("10% increased damage and resource regeneration");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.allDamage  += 0.1f;
			modPlayer.manaRegen  += 1;
		}
    }
}