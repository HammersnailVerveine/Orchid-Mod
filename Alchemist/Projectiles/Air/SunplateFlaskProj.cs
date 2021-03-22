using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class SunplateFlaskProj: OrchidModAlchemistProjectile
    {	
		public bool hasTarget = false;
		public Vector2 orbitPoint = Vector2.Zero;
	
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Air Spore");
        } 
		
        public override void SafeSetDefaults() {
            projectile.width = 22;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.alpha = 64;
			projectile.timeLeft = 900;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			ProjectileID.Sets.Homing[projectile.type] = true;
        }
		
		public override void AI() {
			if (!this.initialized) {
				this.orbitPoint = projectile.Center;
				this.initialized = true;
			}
			
			projectile.ai[1] = projectile.ai[1] + 1f + projectile.ai[0] >= 360f ? 0f : projectile.ai[1] + 1 + projectile.ai[0];
			projectile.rotation += 0.1f + (projectile.ai[0] / 30f);
			
			if (Main.rand.Next(30) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			if (projectile.timeLeft <= 880) {
				if (projectile.timeLeft == 880) {
					projectile.friendly = true;
					projectile.netUpdate = true;
				} else {	
					Vector2 move = Vector2.Zero;
					float distance = 2000f;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 
						&& OrchidModAlchemistNPC.AttractiteCanHome(Main.npc[k])) {
							Vector2 newMove = Main.npc[k].Center - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance) {
								distance = distanceTo;
								orbitPoint = Main.npc[k].Center;
							}
						}
					}
					
					move = orbitPoint - projectile.Center + new Vector2(0f, 100f).RotatedBy(MathHelper.ToRadians(projectile.ai[1]));
					distance = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
					move.Normalize();
					float vel = (1f + (distance * 0.05f));
					vel = vel > 10f ? 10f : vel;
					move *= vel;
					projectile.velocity = move;
				}
			}
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
		}
    }
}
