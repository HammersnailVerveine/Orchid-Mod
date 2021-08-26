using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Dusts.Thorium;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class PharaohScepterProjAlt : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 45;
			projectile.scale = 1f;
			projectile.alpha = 128;
			aiType = ProjectileID.Bullet; 
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sand Bolt");
        } 
		
        public override void AI()
        {		
			int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 64, -projectile.velocity.X / 3, -projectile.velocity.Y / 3, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			
			if (projectile.timeLeft % 5 == 0) {
				spawnDustCircle(64, 15, false);
				spawnDustCircle(64, 10, true);
			}
		}
		
		public void spawnDustCircle(int dustType, int distToCenter, bool backwards) {
			for (int i = 0 ; i < 20 ; i ++ ) {
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity = backwards ? projectile.velocity / 2 : projectile.velocity * 1.5f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.3f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}