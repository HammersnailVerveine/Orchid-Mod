using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class WaterSporeProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Spore");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 126;
			Projectile.timeLeft = 600;
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
					bool target = false;
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
								target = true;
							}
						}
					}

					if (target)
					{
						move.Normalize();
						float vel = (1f + (distance * 0.03f));
						vel = vel > 10f ? 10f : vel;
						move *= vel;
						Projectile.velocity = move;
						Projectile.timeLeft++;

						if (Main.rand.Next(4) == 0)
						{
							int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
							Main.dust[dust].noGravity = true;
							Main.dust[dust].noLight = true;
							Main.dust[dust].scale *= 1.5f;
						}
					}
					else
					{
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

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.knockBackResist > 0f)
			{
				target.velocity *= 0f;
			}
		}
	}
}