using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Equipment
{
	public class Smite : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 100;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 5;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Smite");
		}

		public override void AI()
		{
			switch (Projectile.timeLeft)
			{
				case 100:
					spawnDustCircle(162, 15);
					break;
				case 95:
					spawnDustCircle(162, 12);
					break;
				case 90:
					spawnDustCircle(162, 9);
					break;
				case 85:
					spawnDustCircle(162, 6);
					break;
				default:
					break;
			}

			for (int index1 = 0; index1 < 9; ++index1)
			{
				if (index1 % 3 == 0)
				{
					float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)index1;
					float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 162, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = Projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float)Main.rand.Next(10, 110) * 0.013f;
					Main.dust[index2].velocity = Projectile.velocity / 2;
					Main.dust[index2].noGravity = true;
				}
			}

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 450f;
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

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 15; i++)
			{
				double dustDeg = (i * (27));//    + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = Projectile.position.X - (int)(Math.Cos(dustRad) * distToCenter) - Projectile.width / 4;
				float posY = Projectile.position.Y - (int)(Math.Sin(dustRad) * distToCenter) - Projectile.height / 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index1].velocity = Projectile.velocity / 2;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 1.2f;
				Main.dust[index1].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (Projectile.timeLeft < 80)
			{
				spawnDustCircle(162, 15);
				Projectile.position -= Projectile.velocity * 3;
				spawnDustCircle(162, 12);
				Projectile.position -= Projectile.velocity * 3;
				spawnDustCircle(162, 9);
				Projectile.position -= Projectile.velocity * 3;
				spawnDustCircle(162, 6);
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
	}
}