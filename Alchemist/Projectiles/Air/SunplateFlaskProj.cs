using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
	public class SunplateFlaskProj : OrchidModAlchemistProjectile
	{
		public bool hasTarget = false;
		public Vector2 orbitPoint = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Air Spore");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 24;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 64;
			Projectile.timeLeft = 900;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void AI()
		{
			if (!this.initialized)
			{
				this.orbitPoint = Projectile.Center;
				this.initialized = true;
			}

			Projectile.ai[1] = Projectile.ai[1] + 1f + Projectile.ai[0] >= 360f ? 0f : Projectile.ai[1] + 1 + Projectile.ai[0];
			Projectile.rotation += 0.1f + (Projectile.ai[0] / 30f);

			if (Main.rand.NextBool(30))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			if (Projectile.timeLeft <= 880)
			{
				if (Projectile.timeLeft == 880)
				{
					Projectile.friendly = true;
					Projectile.netUpdate = true;
				}
				else
				{
					float distance = 2000f;
					Player player = Main.player[Projectile.owner];
					if (player.HasBuff(BuffType<Alchemist.Buffs.StellarTalcBuff>()))
					{
						orbitPoint = player.Center;
					}
					else
					{
						for (int k = 0; k < 200; k++)
						{
							if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5
							&& OrchidModAlchemistNPC.AttractiteCanHome(Main.npc[k]))
							{
								Vector2 newMove = Main.npc[k].Center - Projectile.Center;
								float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
								if (distanceTo < distance)
								{
									distance = distanceTo;
									orbitPoint = Main.npc[k].Center;
								}
							}
						}
					}

					Vector2 move = orbitPoint - Projectile.Center + new Vector2(0f, 100f).RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));
					distance = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
					move.Normalize();
					float vel = (1f + (distance * 0.05f));
					vel = vel > 10f ? 10f : vel;
					move *= vel;
					Projectile.velocity = move;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}
