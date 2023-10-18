using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Large
{
	public class SanctifyProjAlt : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 3;
			Projectile.height = 3;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 90;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Holy Magic");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int dust2 = Dust.NewDust(pos, Projectile.width, Projectile.height / 2, 169);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].scale = 1f;
				Main.dust[dust2].velocity = Projectile.velocity / 2;
				Main.dust[dust2].noLight = true;
			}

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}

			Vector2 move = Vector2.Zero;
			float distance = 400f;
			bool target = false;

			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Projectile.timeLeft < 75)
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
				Projectile.velocity = (10 * Projectile.velocity + move) / 1f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
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
			spawnDustCircle(169, 10);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}