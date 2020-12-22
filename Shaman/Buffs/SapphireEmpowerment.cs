using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class SapphireEmpowerment : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Sapphire Empowerment");
			Description.SetDefault("Increases the effectiveness your shamanic water bonds");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanWaterBonus += 1;
		}
    }
}