using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles.Water
{
    public class WaterSporeProj: OrchidModAlchemistProjectile
	{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Water Spore");
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
			if (!this.initialized) {
				projectile.ai[1] = Main.rand.Next(2) == 0 ? -1 : 1;
				projectile.timeLeft -= Main.rand.Next(15);
				projectile.netUpdate = true;
				this.initialized = true;
			}
			
			if (projectile.timeLeft % 50 == 0) {
				for(int i=0; i<5; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}

			if (projectile.timeLeft <= 550) {
				if (projectile.timeLeft == 550) {
					projectile.velocity *= (float)((4 + Main.rand.Next(3)) / 10f);
					projectile.friendly = true;
					projectile.netUpdate = true;
				} else {		
					Vector2 move = Vector2.Zero;
					float distance = 2000f;
					bool target = false;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 
						&& OrchidModAlchemistNPC.AttractiteCanHome(Main.npc[k])) {
							Vector2 newMove = Main.npc[k].Center - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance) {
								move = newMove;
								distance = distanceTo;
								target = true;
							}
						}
					}
					
					if (target) {
						move.Normalize();
						float vel = (1f + (distance * 0.03f));
						vel = vel > 10f ? 10f : vel;
						move *= vel;
						projectile.velocity = move;
						projectile.timeLeft ++;
						
						if (Main.rand.Next(4) == 0) {
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].noLight = true;
							Main.dust[dust].scale *= 1.5f;
						}
					} else {
						int angle = (int)(5 * projectile.ai[1]);
						move = projectile.velocity.RotatedBy(MathHelper.ToRadians(angle));
						move.Normalize();
						move *= 3f;
						projectile.velocity = move;
					}
				}
			}
        }
		
		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {
			if (modTarget.alchemistFire > 0) {
				damage = (int)(damage * 1.1f);
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.knockBackResist > 0f) {
				target.velocity *= 0f;
			}
		}
    }
}