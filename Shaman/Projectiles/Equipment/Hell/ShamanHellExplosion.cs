using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Equipment.Hell
{
    public class ShamanHellExplosion : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 150;
            projectile.height = 150;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
        }
		
        public override void AI()
        {
			for (int i = 0 ; i < 20 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X *= 2;
				Main.dust[dust2].velocity.Y *= 2;
			}
			
			for (int i = 0 ; i < 15 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 37);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X *= 1;
				Main.dust[dust2].velocity.Y *= 1;
			}
			spawnDustCircle(6, 100);
			spawnDustCircle(6, 120);
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 52 ; i ++ )
			{
				double dustDeg = (i * (7));//    + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter);
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter);
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity = projectile.velocity / 2;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 1.2f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
        }
    }
}