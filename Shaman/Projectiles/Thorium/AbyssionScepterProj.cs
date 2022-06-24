using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class AbyssionScepterProj : OrchidModShamanProjectile
	{
		public Vector2 storedVelocity;
		public float dustVelocity = 0f;
		public int storedDamage = 0;
		public Vector2 storedPosition = Vector2.Zero;
		public List<int> hitEnemies = new List<int>();

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.alpha = 255;
			Projectile.penetrate = 3;
			Projectile.tileCollide = false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Gods Energy");
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 120)
			{
				Projectile.position.X += Projectile.velocity.X * 50;
				Projectile.position.Y += Projectile.velocity.Y * 50;
				this.storedVelocity = Projectile.velocity * -1f;
				Projectile.velocity *= 0f;
				this.storedDamage = Projectile.damage;
				Projectile.damage = 0;
				storedPosition = Projectile.position;
			}

			if (Projectile.timeLeft > 50)
			{
				this.dustVelocity += 0.07f;
				int dust = Dust.NewDust(Projectile.Center, 1, 1, 27);
				Main.dust[dust].velocity *= dustVelocity;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				if (Projectile.timeLeft % 10 == 0)
				{
					spawnDustCircle(27, (int)(120 - Projectile.timeLeft) / 3);
				}
			}
			else
			{
				Projectile.damage = this.storedDamage;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust].velocity = Projectile.velocity / 2f;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust2].velocity = Projectile.velocity / 3f;
				Main.dust[dust2].scale = 2.5f;
				Main.dust[dust2].noGravity = true;

				Vector2 move = Vector2.Zero;
				float distance = 140f;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !(hitEnemies.Contains(k)))
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
					Projectile.velocity = (8 * Projectile.velocity + move) / 1f;
					AdjustMagnitude(ref Projectile.velocity);
					Projectile.timeLeft ++;
				} else {
					Projectile.position = storedPosition;
					Projectile.velocity = storedVelocity * 1.75f;
					storedPosition = Projectile.position + Projectile.velocity;
				}
			}
		}
		
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 8f)
			{
				vector *= 8f / magnitude;
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public override bool? CanHitNPC(NPC target) {
			return !(target.friendly || this.hitEnemies.Contains(target.whoAmI));
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
			}
			spawnDustCircle(27, 20);
			spawnDustCircle(27, 30);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			target.AddBuff(153, 60 * 5); // Shadowflame
			hitEnemies.Add(target.whoAmI);
			Projectile.position = storedPosition;
		}
	}
}