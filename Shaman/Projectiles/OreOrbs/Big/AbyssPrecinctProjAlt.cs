using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
    public class AbyssPrecinctProjAlt : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 89;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.alpha = 192;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyss Bolt");
        } 
		
        public override void AI()
        {  
			projectile.rotation += 0.1f;
		
			if (projectile.timeLeft % 30 == 0) {
				spawnDustCircle(172, 50);
				spawnDustCircle(172, 100);
				spawnDustCircle(29, 75);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, mod.ProjectileType("AbyssPrecinctProjExplosion"), projectile.damage * 2, 0.0f, projectile.owner, 0.0f, 0.0f);
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
				if (projectile.timeLeft == 30) projectile.Kill();
			}
			
			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.7f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ ) {
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + projectile.velocity.Y + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity = distToCenter == 50 ? projectile.velocity : projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
    }
}