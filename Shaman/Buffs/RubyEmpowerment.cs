using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class RubyEmpowerment : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Ruby Empowerment");
			Description.SetDefault("Increases the effectiveness your shamanic fire bonds");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanFireBonus += 1;
		}
    }
}