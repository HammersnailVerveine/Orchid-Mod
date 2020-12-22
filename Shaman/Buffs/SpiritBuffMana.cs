using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class SpiritBuffMana : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Improved Ancestral Power");
			Description.SetDefault("15% increased damage, paused empowerment duration");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.allDamage  += 0.15f;
			modPlayer.manaCost *= 0f;
		}
    }
}