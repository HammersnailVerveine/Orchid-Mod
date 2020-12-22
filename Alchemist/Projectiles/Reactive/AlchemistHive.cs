using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
    public class AlchemistHive : AlchemistProjReactive
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 24;
            projectile.height = 30;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.scale = 1f;
			projectile.alpha = 32;
			Main.projFrames[projectile.type] = 5;
			this.spawnTimeLeft = 600;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hive");
        } 
		
		public override void SafeAI()
        {
			projectile.velocity.Y *= 0.95f;
			projectile.velocity.X *= 0.99f;
			
			if (Main.rand.Next(20) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 153);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
            }
			
			if (projectile.timeLeft % 10 == 0) {
				projectile.frame ++;
				if (projectile.frame > 4) {
					projectile.frame = 0;
				}
			}
		}
		
		public override void Despawn() {
            for(int i=0; i<5; i++)
            {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 153);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
            }
		}
		
		public override void SafeKill(int timeLeft, Player player, OrchidModPlayer modPlayer)
        {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 85);
			int dmg = projectile.damage;
			for (int i = 0 ; i < 10 ; i ++) {
				Vector2 vel = ( new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				if (player.strongBees && Main.rand.Next(2) == 0) 
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, 566, (int) (dmg * 1.15f), 0f, projectile.owner, 0f, 0f);
				else {
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, 181, dmg, 0f, projectile.owner, 0f, 0f);
				}
			}
        }
    }
}