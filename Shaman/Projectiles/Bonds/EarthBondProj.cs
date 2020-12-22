using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Bonds
{
    public class EarthBondProj : OrchidModShamanProjectile
    {
		private double dustVal = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 24;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 1800;	
			projectile.scale = 1f;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Earth Totem");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];		
			
			if (projectile.velocity.Y < 1f) {
				spawnDust(64, 250);
				if (projectile.timeLeft % 4 == 0) {
					spawnDust(64, 50);
				}
			}

			this.dustVal ++;
			
			Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 300f && projectile.position.X - 280 < player.position.X + player.width && projectile.position.X + projectile.width + 280 > player.position.X 
			&& projectile.position.Y - 280 < player.position.Y + player.height && projectile.position.Y + projectile.height + 280 > player.position.Y) {
				int buff = projectile.ai[1] == 1 ? mod.BuffType("EarthTotemPlus") : mod.BuffType("EarthTotem");
				player.AddBuff(buff, 1);
			}
        }
		
		public void spawnDust(int dustType, int distToCenter) {
			for (int i = 0 ; i < 2 ; i ++ ) {
				double deg = (4 * (42 + this.dustVal));
				deg *= i == 1 ? -1 : 1;
				double rad = deg * (Math.PI / 180);
						 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * (distToCenter + (i * 20))) + projectile.velocity.X - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * (distToCenter + (i * 20))) + projectile.velocity.Y - 4;
						
				Vector2 dustPosition = new Vector2(posX, posY);
						
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
						
				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = projectile.velocity.X == 0 ? 1.5f :(float) Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
            }
        }
    }
}