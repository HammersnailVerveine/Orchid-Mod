using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
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
			Projectile.width = 18;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SafeAI()
		{
			if (!this.initialized)
			{
				this.initialized = true;
				this.rotation = 0.03f * (Main.rand.NextBool(2) ? -1 : 1);
				Projectile.frame = Main.rand.Next(4);
			}

			if (Main.rand.NextBool(15))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 2);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.5f;
			}

			Projectile.rotation += this.rotation;


			if (Main.myPlayer == Projectile.owner)
			{
				Vector2 move = Vector2.Zero;
				float distance = 100f;

				Vector2 newMove = Main.MouseWorld - Projectile.Center;
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);

				if (distanceTo < distance)
				{
					if (Projectile.timeLeft % 15 == 0)
					{
						Projectile.damage++;
					}

					proximityDuration += proximityDuration < 90 ? 1 : 0;
					int index = 0 + proximityDuration;
					int mult = 0;
					while (index > 30)
					{
						mult++;
						index -= 30;
					}

					Projectile.velocity.Normalize();
					Projectile.velocity *= 5f + mult * 1.5f;
					Projectile.rotation += this.rotation * 2;
					if (pushedSync == false)
					{
						Projectile.netUpdate = true;
						pushedSync = true;
					}
				}
				else
				{
					Projectile.velocity *= 0.975f;
					proximityDuration = 0;
					if (pushedSync == true)
					{
						Projectile.netUpdate = true;
						pushedSync = false;
					}
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
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
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 2);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].velocity *= 3f;
			}
			SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(20, 60 * 3);
			}
		}
	}
}