using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Water
{
    public class BloodMoonFlaskProj : OrchidModAlchemistProjectile
    {
		float rotationSpeed = 0f;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 40;
            projectile.height = 38;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.alpha = 64;
			projectile.penetrate = -1;
			Main.projFrames[projectile.type] = 2;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Mist");
        } 
		
		public override void AI()
        {
			if (!this.initialized) {
				projectile.frame = Main.rand.Next(2);
				this.initialized = true;
				projectile.rotation += Main.rand.NextFloat();
				this.rotationSpeed =  (0.01f + Main.rand.NextFloat() * 0.03f) * (Main.rand.Next(2) == 0 ? 1f : -1f);
			}
			projectile.rotation += this.rotationSpeed;
			projectile.velocity *= 0.925f;
			projectile.alpha += Main.rand.Next(3);
			if (projectile.alpha >= 255) {
				projectile.Kill();
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
		
		// public override void Kill(int timeLeft)
        // {
            // for(int i=0; i<4; i++)
            // {
                // int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
				// Main.dust[dust].velocity *= 1.5f;
				// Main.dust[dust].scale *= 1f;
            // }
        // }
    }
}