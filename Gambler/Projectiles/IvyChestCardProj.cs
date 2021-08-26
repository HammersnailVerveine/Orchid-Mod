using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Projectiles
{
	public class IvyChestCardProj : OrchidModGamblerProjectile
	{
		private float rotation = 0f;
		private int proximityDuration = 0;
		private bool pushedSync = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leaf");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			if (!this.initialized)
			{
				this.initialized = true;
				this.rotation = 0.03f * (Main.rand.Next(2) == 0 ? -1 : 1);
				projectile.frame = Main.rand.Next(4);
			}

			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 2);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.5f;
			}

			projectile.rotation += this.rotation;


			if (Main.myPlayer == projectile.owner)
			{
				Vector2 move = Vector2.Zero;
				float distance = 100f;

				Vector2 newMove = Main.MouseWorld - projectile.Center;
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);

				if (distanceTo < distance)
				{
					if (projectile.timeLeft % 15 == 0)
					{
						projectile.damage++;
					}

					proximityDuration += proximityDuration < 90 ? 1 : 0;
					int index = 0 + proximityDuration;
					int mult = 0;
					while (index > 30)
					{
						mult++;
						index -= 30;
					}

					projectile.velocity.Normalize();
					projectile.velocity *= 5f + mult * 1.5f;
					projectile.rotation += this.rotation * 2;
					if (pushedSync == false)
					{
						projectile.netUpdate = true;
						pushedSync = true;
					}
				}
				else
				{
					projectile.velocity *= 0.975f;
					proximityDuration = 0;
					if (pushedSync == true)
					{
						projectile.netUpdate = true;
						pushedSync = false;
					}
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 5f)
			{
				vector *= 5f / magnitude;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 2);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].velocity *= 3f;
			}
			Main.PlaySound(6, (int)projectile.Center.X, (int)projectile.Center.Y, 0);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(20, 60 * 3);
			}
		}
	}
}