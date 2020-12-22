using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Fire
{
    public class EmberVialProj : OrchidModAlchemistProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 650;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 3;
			Main.projFrames[projectile.type] = 4;
        }
		
		public override void AI()
        {
			if (projectile.timeLeft == 600) {
				projectile.velocity.X = 0f;
				projectile.velocity.Y = 1f;
				Vector2 initialVelocity = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(90));
				projectile.velocity = initialVelocity;
				projectile.frame = Main.rand.Next(4);
				projectile.friendly = true;
				projectile.netUpdate = true;
			}
			
			if (Main.rand.Next(3) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			Vector2 projectileVelocity = (new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedByRandom(MathHelper.ToRadians(3)));
			projectile.velocity = projectileVelocity;
			projectile.velocity.Y += 0.01f;
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.velocity *= 0f;
            return false;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ember");
        }
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60 * 3);
		}
    }
}