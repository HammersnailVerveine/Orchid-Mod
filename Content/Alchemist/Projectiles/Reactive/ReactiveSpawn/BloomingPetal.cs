using Microsoft.Xna.Framework;
using OrchidMod.Content.Alchemist.Debuffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Content.Alchemist.Projectiles.Reactive.ReactiveSpawn
{
	public class BloomingPetal : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Petal");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 29;
			Projectile.timeLeft = 900;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void AI()
		{
			Projectile.rotation += 0.3f;

			if (Projectile.timeLeft == 900)
			{
				Projectile.frame = Projectile.knockBack > 0 ? 1 : 0;
				Projectile.knockBack = 0f;
			}

			if (Projectile.timeLeft <= 850)
			{

				if (Projectile.timeLeft == 850)
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
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int type = Projectile.frame == 1 ? DustType<Content.Dusts.BloomingAltDust>() : DustType<Content.Dusts.BloomingDust>();
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		}
	}
}