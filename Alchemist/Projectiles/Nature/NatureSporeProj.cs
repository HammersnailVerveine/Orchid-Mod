using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles.Nature
{
    public class NatureSporeProj : OrchidModAlchemistProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Nature Spore");
        } 
		
        public override void SafeSetDefaults() {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.alpha = 126;
			projectile.timeLeft = 600;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.projectileTrail = true;
        }
		
		public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
		
		public override void AI() {
			projectile.velocity.Y += projectile.velocity.Y < 3f ? 0.1f : 0f;
			
			if (!this.initialized) {
				projectile.ai[1] = Main.rand.Next(2) == 0 ? -1 : 1;
				projectile.timeLeft -= Main.rand.Next(15);
				projectile.netUpdate = true;
				this.initialized = true;
			}

			if (projectile.timeLeft <= 550) {
				if (projectile.timeLeft == 550) {
					projectile.velocity *= (float)((4 + Main.rand.Next(3)) / 10f);
					projectile.friendly = true;
					projectile.netUpdate = true;
				} else {		
					Vector2 move = Vector2.Zero;
					float distance = 2000f;
					int flag = -1;
					for (int k = 0; k < 200; k++) {
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 
						&& OrchidModAlchemistNPC.AttractiteCanHome(Main.npc[k])) {
							Vector2 newMove = Main.npc[k].Center - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance) {
								move = newMove;
								distance = distanceTo;
								flag = k;
							}
						}
					}
					
					if (flag > -1) {
						if (Main.npc[flag].Center.X > projectile.Center.X) {
							projectile.velocity.X += projectile.velocity.X < 5f ? 0.75f : 0f;
						} else {
							projectile.velocity.X -= projectile.velocity.X > -5f ? 0.75f : 0f;	
						}
						
						projectile.timeLeft ++;
						
						if (Main.rand.Next(4) == 0) {
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].noLight = true;
							Main.dust[dust].scale *= 1.5f;
						}
					} else {
						float absY = Math.Abs(projectile.velocity.Y);
						projectile.velocity.X = (3f - absY) * projectile.ai[1];
					}
				}
			}
        }
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) {
				projectile.velocity.Y = -oldVelocity.Y;
				if (projectile.velocity.Y < 0f) {
					projectile.velocity.Y = -2f;
				}
			}
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int projType = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjBloom>();
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, projectile.damage, 3f, projectile.owner, 0.0f, 0.0f);
        }
    }
}