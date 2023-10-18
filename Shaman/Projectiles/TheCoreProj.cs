using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class TheCoreProj : OrchidModShamanProjectile
	{
		float saveVelocityX = 0f;
		float saveVelocityY = 0f;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.extraUpdates = 1;
			Projectile.scale = 0f;
			Projectile.timeLeft = 140;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Core Bolt");
		}

		public override void AI()
		{
			float distance = 1000f;
			bool target = false;
			if (Projectile.timeLeft == 140)
			{
				saveVelocityX = Projectile.velocity.X;
				saveVelocityY = Projectile.velocity.Y;
			}
			++Projectile.localAI[0];
			if ((double)Projectile.localAI[0] <= 7.0)
				return;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110, Projectile.velocity.X, Projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity *= (float)(0.100000001490116 + (double)Main.rand.Next(4) * 0.100000001490116);
				Main.dust[index].scale *= (float)(1.0 + (double)Main.rand.Next(5) * 0.100000001490116);
			}
			Projectile.ai[1]++;
			if (Projectile.ai[1] == 20)
			{
				Projectile.ai[1] = 0;
				if (saveVelocityX != Projectile.velocity.X && saveVelocityY != Projectile.velocity.Y)
				{
					Projectile.velocity.Y = Projectile.velocity.Y + (Main.rand.Next(20) - 10);
					Projectile.velocity.X = Projectile.velocity.X + (Main.rand.Next(20) - 10);
				}
				else Projectile.timeLeft -= 40;
			}

			if (target = false && saveVelocityX != Projectile.velocity.X && saveVelocityY != Projectile.velocity.Y)
				Projectile.velocity.Y /= 2;

			if (target = false && saveVelocityX != Projectile.velocity.X && saveVelocityY != Projectile.velocity.Y)
				Projectile.velocity.X /= 2;


			if (Projectile.timeLeft <= 120)
			{
				if (Projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref Projectile.velocity);
					Projectile.localAI[0] = 1f;
				}
				Vector2 move = Vector2.Zero;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
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
					Projectile.velocity = (10 * Projectile.velocity + move) / 3f;
					AdjustMagnitude(ref Projectile.velocity);
				}
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

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int index2 = Dust.NewDust(Projectile.Center, 0, 0, 110, 0.0f, 0.0f, 110, new Color(), 1f);
				Main.dust[index2].scale = 2f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].position = Projectile.Center;
				Main.dust[index2].velocity = Projectile.velocity * 0.1f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}