using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class MoltenResidueScepterProjExplosion : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 200;
            this.empowermentType = 1;
            this.empowermentLevel = 3;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
        }
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			int size = 30 + 30 * OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			projectile.width = size;
			projectile.height = size;
			
			projectile.position.X -= size / 2;
			projectile.position.Y -= size / 2;
			
			// for (int i = 0 ; i < (int)(projectile.width / 2) ; i ++) {
				// int index1 = Dust.NewDust(projectile.position, size, 1, 6, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				// Main.dust[index1].velocity *= 0.2f;
				// Main.dust[index1].fadeIn = 1f;
				// Main.dust[index1].scale = 1.5f;
				// Main.dust[index1].noGravity = true;
			// }
		
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}