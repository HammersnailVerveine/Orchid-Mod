using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Buffs
{
    public class BloodMistFlaskBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Misty Steps");
			Description.SetDefault("Periodically releases blood mist while moving");
            Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
        }
		
        public override void Update(Player player, ref int buffIndex)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.timer120 % 60 == 0 && player.velocity.X != 0f) {
				int projType = ProjectileType<Alchemist.Projectiles.Water.BloodMoonFlaskProj>();
				int itemType = ItemType<Alchemist.Weapons.Water.BloodMoonFlask>();
				int damage = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, itemType, 2, true);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projType, damage, 1f, player.whoAmI);
			}
		}
    }
}