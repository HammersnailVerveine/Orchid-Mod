using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
    public class LivingSapVialProj : OrchidModAlchemistProjectile
    {
		public int heal = 0;
		
        public override void SafeSetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.scale = 1f;
			projectile.alpha = 128;
			projectile.penetrate = 10;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sap Bubble");
        } 
		
		public override void AI()
        {
			projectile.velocity *= 0.95f;;
			projectile.rotation += 0.02f;
			
			if (projectile.damage != 0) {
				this.heal += projectile.damage;
				projectile.damage = 0;
			}
			
			if (Main.rand.Next(20) == 0) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 102);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
            }
			
			
			//Player player = Main.player[projectile.owner];
			Player player = Main.player[Main.myPlayer]; // < TEST MP
			Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 50f && projectile.position.X < player.position.X + player.width && projectile.position.X + projectile.width > player.position.X && projectile.position.Y < player.position.Y + player.height && projectile.position.Y + projectile.height > player.position.Y) {
				if (!Main.LocalPlayer.moonLeech) {
					int damage = player.statLifeMax2 - player.statLife;
					if (heal > damage) {
						this.heal = damage;
					}
					if (this.heal > 0) {
						player.HealEffect(this.heal, true);
						player.statLife += this.heal;
						projectile.Kill();
					}
				}
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 102);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
        }
    }
}