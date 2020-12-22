using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class EmeraldEmpowerment : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Emerald Empowerment");
			Description.SetDefault("Increases the effectiveness your shamanic air bonds");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanAirBonus += 1;
		}
    }
}