using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
    public class DiamondEmpowerment : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Diamond Empowerment");
			Description.SetDefault("Increases the duration of your shamanic bonds by 3 seconds");
        }
		
        public override void Update(Player player, ref int buffIndex) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 3;
		}
    }
}