using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class AmethystEmpowerment : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Amethyst Empowerment");
			Description.SetDefault("Increases the effectiveness your shamanic spirit bonds");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanSpiritBonus += 1;
		}
    }
}