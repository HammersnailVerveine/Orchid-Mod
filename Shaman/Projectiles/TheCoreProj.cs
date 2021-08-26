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
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.extraUpdates = 1;
			projectile.scale = 0f;
			projectile.timeLeft = 140;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Bolt");
		}

		public override void AI()
		{
			float distance = 1000f;
			bool target = false;
			if (projectile.timeLeft == 140)
			{
				saveVelocityX = projectile.velocity.X;
				saveVelocityY = projectile.velocity.Y;
			}
			++projectile.localAI[0];
			if ((double)projectile.localAI[0] <= 7.0)
				return;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 110, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity *= (float)(0.100000001490116 + (double)Main.rand.Next(4) * 0.100000001490116);
				Main.dust[index].scale *= (float)(1.0 + (double)Main.rand.Next(5) * 0.100000001490116);
			}
			projectile.ai[1]++;
			if (projectile.ai[1] == 20)
			{
				projectile.ai[1] = 0;
				if (saveVelocityX != projectile.velocity.X && saveVelocityY != projectile.velocity.Y)
				{
					projectile.velocity.Y = projectile.velocity.Y + (Main.rand.Next(20) - 10);
					projectile.velocity.X = projectile.velocity.X + (Main.rand.Next(20) - 10);
				}
				else projectile.timeLeft -= 40;
			}

			if (target = false && saveVelocityX != projectile.velocity.X && saveVelocityY != projectile.velocity.Y)
				projectile.velocity.Y /= 2;

			if (target = false && saveVelocityX != projectile.velocity.X && saveVelocityY != projectile.velocity.Y)
				projectile.velocity.X /= 2;


			if (projectile.timeLeft <= 120)
			{
				if (projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref projectile.velocity);
					projectile.localAI[0] = 1f;
				}
				Vector2 move = Vector2.Zero;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
					{
						Vector2 newMove = Main.npc[k].Center - projectile.Center;
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
					projectile.velocity = (10 * projectile.velocity + move) / 3f;
					AdjustMagnitude(ref projectile.velocity);
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

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int index2 = Dust.NewDust(projectile.Center, 0, 0, 110, 0.0f, 0.0f, 110, new Color(), 1f);
				Main.dust[index2].scale = 2f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].position = projectile.Center;
				Main.dust[index2].velocity = projectile.velocity * 0.1f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}