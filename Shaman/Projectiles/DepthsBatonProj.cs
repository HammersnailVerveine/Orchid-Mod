using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Projectiles
{
    public class DepthsBatonProj : OrchidModShamanProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Depths Blast");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
			projectile.scale = 0f;
            projectile.aiStyle = 0;
			projectile.timeLeft = 45;
			projectile.tileCollide = true;
            this.empowermentType = 5;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {    
            if (projectile.timeLeft == 45)
            {	
				projectile.ai[0] = (Main.rand.Next(50) - 25);
				projectile.ai[1] = (Main.rand.Next(50) - 25);
				projectile.netUpdate = true;
			}	
		    for (int index1 = 0; index1 < 9; ++index1)
                {	
					if (index1 % 3 == 0) {
						float x = projectile.position.X - projectile.velocity.X / 10f * (float) index1;
						float y = projectile.position.Y - projectile.velocity.Y / 10f * (float) index1;
						int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 70, 0.0f, 0.0f, 0, new Color(), 1f);
						Main.dust[index2].alpha = projectile.alpha;
						Main.dust[index2].position.X = x;
						Main.dust[index2].position.Y = y;
						Main.dust[index2].scale = (float) 100 * 0.015f;
						if (projectile.timeLeft <= 14) Main.dust[index2].scale = (float) (200 * 0.015f / projectile.timeLeft);
						Main.dust[index2].velocity *= 0.0f;
						Main.dust[index2].noGravity = true;
					}
                }
            if (projectile.ai[1] == 0)
            {	
				projectile.ai[1] = 25;
			}	
            if (projectile.ai[0] == 0)
            {	
				projectile.ai[0] = 25;
			}		
            if (projectile.timeLeft == 40)
            {	
				projectile.tileCollide = false;
				projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
            if (projectile.timeLeft == 37)
            {	
				projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
			}
            if (projectile.timeLeft == 30)
            {	
				projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
			}
            if (projectile.timeLeft == 24)
            {	
				projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
            if (projectile.timeLeft == 17)
            {	
				projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
            if (projectile.timeLeft == 14)
            {
				projectile.tileCollide = true;
				projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
				for(int i=0; i<10; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
					Main.dust[dust].scale = (float) 100 * 0.015f;
				}
				//projectile.penetrate = 1;
			}
			if (projectile.timeLeft <= 10)
            {
				projectile.damage += 2;
			}
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].scale = (float) 200 * 0.015f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}