using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Debuffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class DungeonFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Water Bolt");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.alpha = 126;
			Projectile.timeLeft = 600;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 600)
			{
				Projectile.ai[1] = Main.rand.Next(2) == 0 ? -1 : 1;
				Projectile.netUpdate = true;
			}

			Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));

			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
			}

			if (Projectile.timeLeft <= 550)
			{

				if (Projectile.timeLeft == 550)
				{
					Projectile.velocity *= 0.25f;
				}

				Projectile.friendly = true;

				if (Projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref Projectile.velocity);
					Projectile.localAI[0] = 1f;
				}

				Vector2 move = Vector2.Zero;
				float distance = 2000f;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].HasBuff(ModContent.BuffType<Attraction>()))
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
					AdjustMagnitude(ref move);
					Projectile.velocity = (5 * Projectile.velocity + move) / 1f;
					AdjustMagnitude(ref Projectile.velocity);
				}

				Projectile.ai[0] = target ? Main.rand.Next(10) - 5 : 5 * Projectile.ai[1];
				Projectile.netUpdate = true;
				Vector2 projectileVelocity = (Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0])));
				Projectile.velocity = projectileVelocity;
			}
			else
			{
				Projectile.friendly = false;
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
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
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		}
	}
}