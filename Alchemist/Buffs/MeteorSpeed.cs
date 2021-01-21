using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
    public class MeteorSpeed : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Meteor Speed");
			Description.SetDefault("Increased movement speed");
            Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
        }
		
        public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.5f;
		}
    }
}