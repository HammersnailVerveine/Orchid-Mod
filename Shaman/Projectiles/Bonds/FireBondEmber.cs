using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Bonds
{
    public class FireBondEmber : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			Main.projFrames[projectile.type] = 4;
        }
		
		public override void AI()
        {
			if (projectile.timeLeft == 300) {
				projectile.ai[1] = Main.rand.Next(2) == 0 ? -1 : 1;
				projectile.velocity.X = 0.5f;
				projectile.velocity.Y = 0.5f;
				Vector2 initialVelocity = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));
				projectile.velocity = initialVelocity;
				projectile.frame = Main.rand.Next(4);
				projectile.netUpdate = true;
			}
			
			if (Main.rand.Next(3) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			Vector2 projectileVelocity = (new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(projectile.ai[1])));
			projectile.velocity = projectileVelocity;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ember");
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(BuffID.OnFire, 60 * 5);
		}
    }
}