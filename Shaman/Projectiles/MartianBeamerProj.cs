using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class MartianBeamerProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 100;
			Projectile.scale = 0f;
			Projectile.extraUpdates = 10;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Martian Beam");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			if (Projectile.alpha < 170)
			{
				for (int index1 = 0; index1 < 9; ++index1)
				{
					if (index1 % 3 == 0)
					{
						float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)index1;
						float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)index1;
						int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 226, 0.0f, 0.0f, 0, new Color(), 1f);
						Main.dust[index2].alpha = Projectile.alpha;
						Main.dust[index2].position.X = x;
						Main.dust[index2].position.Y = y;
						Main.dust[index2].scale = (float)Main.rand.Next(1, 11) * 0.13f;
						Main.dust[index2].velocity = Projectile.velocity;
						Main.dust[index2].noGravity = true;
					}
				}
			}

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 150f;
			bool target = false;
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
				Projectile.velocity = (20 * Projectile.velocity + move) / 10f;
				AdjustMagnitude(ref Projectile.velocity);
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
			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}