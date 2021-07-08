using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class WyvernMorayProjAlt : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyvern Spit");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 150;
			projectile.alpha = 255;
            this.empowermentType = 2;
		}
		
        public override void AI()
        {	
		    if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
			
		    if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 41);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
			
			if (projectile.timeLeft % 5 == 0) spawnDustCircle(41, 10 + Main.rand.Next(10));
		}
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ ) {
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);
					 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width/2 + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height/2 + 4;
					
				Vector2 dustPosition = new Vector2(posX, posY);
					
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
					
				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			
			spawnDustCircle(41, 40);
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 34);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("WyvernMorayProjLingeringAlt"), (int)(projectile.damage * 0.8f), 0.0f, projectile.owner, 0.0f, 0.0f);
			
			for(int i=0; i<10; i++) {
				int dustType = Main.rand.Next(2) == 0 ? 33 : 41;
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
				Main.dust[dust].scale *= 1.5f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	}
}