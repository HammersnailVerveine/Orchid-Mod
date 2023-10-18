using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Projectiles.Air
{
	public class AirSporeProj : OrchidModAlchemistProjectile
	{
		public bool hasTarget = false;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Air Spore");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 126;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
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
				Projectile.ai[1] = Main.rand.Next(2) == 0 ? -1 : 1;
				Projectile.ai[0] = 150;
				Projectile.timeLeft -= Main.rand.Next(15);
				Projectile.netUpdate = true;
				this.initialized = true;
			}

			if (Projectile.timeLeft <= 550)
			{
				if (Projectile.timeLeft == 550)
				{
					Projectile.velocity *= (float)((4 + Main.rand.Next(3)) / 10f);
					Projectile.friendly = true;
					Projectile.netUpdate = true;
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
							Vector2 newMove = Main.npc[k].Center - Projectile.Center;
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
						float projPosX = Projectile.Center.X;
						float projPosY = Projectile.Center.Y;
						if (Math.Abs(npcPosY - projPosY) > 5f)
						{
							if (npcPosY > projPosY)
							{
								Projectile.velocity.Y += Projectile.velocity.X < 5f ? 0.75f : 0f;
							}
							else
							{
								Projectile.velocity.Y -= Projectile.velocity.Y > -5f ? 0.75f : 0f;
							}
							Projectile.velocity.X *= 0.9f;
						}
						else
						{
							if (npcPosX > projPosX)
							{
								Projectile.velocity.X += Projectile.velocity.X < 8f ? 0.75f : 0f;
							}
							else
							{
								Projectile.velocity.X -= Projectile.velocity.X > -8f ? 0.75f : 0f;
							}
							Projectile.velocity.Y *= 0.9f;
						}

						// Vector2 newVel = Main.npc[flag].Center - projectile.Center;
						// newVel.Normalize();
						// projectile.ai[0] -= projectile.ai[0] > 0f ? 1f : 0f;
						// projectile.velocity = newVel.RotatedBy(0.01f * (int)projectile.ai[0] * (int)projectile.ai[1]) * (6f + 0.05f * (int)projectile.ai[0]);

						if (Main.rand.Next(4) == 0)
						{
							int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].noLight = true;
							Main.dust[dust].scale *= 1.5f;
						}

						Projectile.timeLeft++;
					}
					else
					{
						this.hasTarget = false;
						int angle = (int)(5 * Projectile.ai[1]);
						move = Projectile.velocity.RotatedBy(MathHelper.ToRadians(angle));
						move.Normalize();
						move *= 3f;
						Projectile.velocity = move;
					}
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer)
		{
			if (modTarget.alchemistFire > 0)
			{
				damage = (int)(damage * 1.1f);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			Projectile.ai[1] = Projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (OrchidModAlchemistNPC.AttractiteCanHome(target) || !this.hasTarget)
			{
				Projectile.Kill();
			}
		}
	}
}