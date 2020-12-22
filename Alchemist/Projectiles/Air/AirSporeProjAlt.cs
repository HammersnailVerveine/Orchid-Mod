using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class AirSporeProjAlt: OrchidModAlchemistProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Air Spore");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			this.projectileTrail = true;
        }
		
		public override void AI()
        {
			projectile.alpha += 3 + Main.rand.Next(3);
			if (projectile.alpha >= 255) {
				projectile.Kill();
			}
			projectile.velocity = (projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5)));
			if (Main.rand.Next(10) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15);
				Main.dust[dust].noGravity = true;
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15);
				Main.dust[dust].noGravity = true;
            }
		}
    }
}