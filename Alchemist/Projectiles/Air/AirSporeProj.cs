using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Air
{
	public class AirSporeProj : OrchidModAlchemistProjectile
	{
		public bool hasTarget = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Air Spore");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.alpha = 126;
			projectile.timeLeft = 600;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.projectileTrail = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			if (!this.initialized)
			{
				projectile.ai[1] = Main.rand.Next(2) == 0 ? -1 : 1;
				projectile.ai[0] = 150;
				projectile.timeLeft -= Main.rand.Next(15);
				projectile.netUpdate = true;
				this.initialized = true;
			}

			if (projectile.timeLeft <= 550)
			{
				if (projectile.timeLeft == 550)
				{
					projectile.velocity *= (float)((4 + Main.rand.Next(3)) / 10f);
					projectile.friendly = true;
					projectile.netUpdate = true;
				}
				else
				{
					Vector2 move = Vector2.Zero;
					float distance = 2000f;
					int flag = -1;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5
						&& OrchidModAlchemistNPC.AttractiteCanHome(Main.npc[k]))
						{
							Vector2 newMove = Main.npc[k].Center - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance)
							{
								move = newMove;
								distance = distanceTo;
								flag = k;
							}
						}
					}

					if (flag > -1)
					{
						this.hasTarget = true;

						float npcPosX = Main.npc[flag].Center.X;
						float npcPosY = Main.npc[flag].Center.Y;
						float projPosX = projectile.Center.X;
						float projPosY = projectile.Center.Y;
						if (Math.Abs(npcPosY - projPosY) > 5f)
						{
							if (npcPosY > projPosY)
							{
								projectile.velocity.Y += projectile.velocity.X < 5f ? 0.75f : 0f;
							}
							else
							{
								projectile.velocity.Y -= projectile.velocity.Y > -5f ? 0.75f : 0f;
							}
							projectile.velocity.X *= 0.9f;
						}
						else
						{
							if (npcPosX > projPosX)
							{
								projectile.velocity.X += projectile.velocity.X < 8f ? 0.75f : 0f;
							}
							else
							{
								projectile.velocity.X -= projectile.velocity.X > -8f ? 0.75f : 0f;
							}
							projectile.velocity.Y *= 0.9f;
						}

						// Vector2 newVel = Main.npc[flag].Center - projectile.Center;
						// newVel.Normalize();
						// projectile.ai[0] -= projectile.ai[0] > 0f ? 1f : 0f;
						// projectile.velocity = newVel.RotatedBy(0.01f * (int)projectile.ai[0] * (int)projectile.ai[1]) * (6f + 0.05f * (int)projectile.ai[0]);

						if (Main.rand.Next(4) == 0)
						{
							int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].noLight = true;
							Main.dust[dust].scale *= 1.5f;
						}

						projectile.timeLeft++;
					}
					else
					{
						this.hasTarget = false;
						int angle = (int)(5 * projectile.ai[1]);
						move = projectile.velocity.RotatedBy(MathHelper.ToRadians(angle));
						move.Normalize();
						move *= 3f;
						projectile.velocity = move;
					}
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modTarget.alchemistFire > 0)
			{
				damage = (int)(damage * 1.1f);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (OrchidModAlchemistNPC.AttractiteCanHome(target) || !this.hasTarget)
			{
				projectile.Kill();
			}
		}
	}
}