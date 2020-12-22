using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class AbyssShard : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyss Lightning");
        } 
		public override void SafeSetDefaults()
		{
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
			projectile.tileCollide = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 70;
            projectile.penetrate = 15;
			projectile.extraUpdates = 5;
			projectile.alpha = 255;
			this.empowermentType = 1;
			this.empowermentLevel = 5;
			this.spiritPollLoad = 0;
		}
		
		
        public override void AI()
        {	
			for (int index1 = 0; index1 < 10; ++index1)
            {
				if (index1 % 2 == 0) {
					float x = projectile.Center.X - projectile.velocity.X / 10f * (float) index1;
					float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float) index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 172, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float) Main.rand.Next(70, 110) * 0.013f;
					Main.dust[index2].velocity *= 0.0f;
					Main.dust[index2].noGravity = true;
				}
            }
            if (projectile.timeLeft == 69)
            {	
				projectile.ai[0] = (Main.rand.Next(50) - 25);
				projectile.ai[1] = (Main.rand.Next(50) - 25);
			}					
            if (projectile.timeLeft == 50)
            {	
					projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
					projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
            if (projectile.timeLeft == 47)
            {	
					projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
					projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
			}
            if (projectile.timeLeft == 30)
            {	
					projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
					projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
            if (projectile.timeLeft == 27)
            {	
					projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
					projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
			}
		}
	}
}