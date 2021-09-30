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
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			projectile.alpha = 255;
			projectile.penetrate = 3;
			projectile.tileCollide = false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Gods Energy");
		}

		public override void AI()
		{
			if (projectile.timeLeft == 120)
			{
				projectile.position.X += projectile.velocity.X * 50;
				projectile.position.Y += projectile.velocity.Y * 50;
				this.storedVelocity = projectile.velocity * -1f;
				projectile.velocity *= 0f;
				this.storedDamage = projectile.damage;
				projectile.damage = 0;
				storedPosition = projectile.position;
			}

			if (projectile.timeLeft > 50)
			{
				this.dustVelocity += 0.07f;
				int dust = Dust.NewDust(projectile.Center, 1, 1, 27);
				Main.dust[dust].velocity *= dustVelocity;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				if (projectile.timeLeft % 10 == 0)
				{
					spawnDustCircle(27, (int)(120 - projectile.timeLeft) / 3);
				}
			}
			else
			{
				projectile.damage = this.storedDamage;
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].velocity = projectile.velocity / 2f;
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust2].velocity = projectile.velocity / 3f;
				Main.dust[dust2].scale = 2.5f;
				Main.dust[dust2].noGravity = true;

				Vector2 move = Vector2.Zero;
				float distance = 140f;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !(hitEnemies.Contains(k)))
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
					projectile.velocity = (8 * projectile.velocity + move) / 1f;
					AdjustMagnitude(ref projectile.velocity);
					projectile.timeLeft ++;
				} else {
					projectile.position = storedPosition;
					projectile.velocity = storedVelocity * 1.75f;
					storedPosition = projectile.position + projectile.velocity;
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

				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width / 2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height / 2 + projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}
		
		public virtual bool? CanHitNPC(NPC target) {
			return !(target.friendly || this.hitEnemies.Contains(target.whoAmI));
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
			}
			spawnDustCircle(27, 20);
			spawnDustCircle(27, 30);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(153, 60 * 5); // Shadowflame
			hitEnemies.Add(target.whoAmI);
			projectile.position = storedPosition;
		}
	}
}