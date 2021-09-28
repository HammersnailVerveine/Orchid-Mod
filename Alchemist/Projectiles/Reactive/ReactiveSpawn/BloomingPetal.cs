using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Projectiles.Reactive.ReactiveSpawn
{
	public class BloomingPetal : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Petal");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 12;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 29;
			projectile.timeLeft = 900;
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 2;
		}

		public override void AI()
		{
			projectile.rotation += 0.3f;

			if (projectile.timeLeft == 900)
			{
				projectile.frame = projectile.knockBack > 0 ? 1 : 0;
				projectile.knockBack = 0f;
			}

			if (projectile.timeLeft <= 850)
			{

				if (projectile.timeLeft == 850)
				{
					projectile.velocity *= 0.25f;
				}

				projectile.friendly = true;

				if (projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref projectile.velocity);
					projectile.localAI[0] = 1f;
				}

				Vector2 move = Vector2.Zero;
				float distance = 2000f;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].HasBuff(mod.BuffType("Attraction")))
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
					projectile.velocity = (5 * projectile.velocity + move) / 1f;
					AdjustMagnitude(ref projectile.velocity);
				}
			}
			else
			{
				projectile.friendly = false;
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
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int type = projectile.frame == 1 ? DustType<Content.Dusts.BloomingAltDust>() : DustType<Content.Dusts.BloomingDust>();
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, type);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
	}
}