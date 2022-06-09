using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class EaterCardProj2 : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eater Flesh");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 255;
		}

		public override void SafeAI()
		{
			if (Projectile.owner != Main.myPlayer)
			{
				Projectile.active = false;
			}

			if (!this.initialized)
			{
				if (Projectile.timeLeft < 1440)
				{
					this.initialized = true;
					Projectile.velocity *= 0f;
					Projectile.alpha = 0;
					Projectile.damage = 1;
					for (int i = 0; i < 4; i++)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
				}
				else
				{
					Projectile.velocity.Y += 0.1f;
					if (Main.rand.Next(3) == 0)
					{
						int dust = Dust.NewDust(Projectile.Center, 1, 1, 18);
						Main.dust[dust].velocity *= 0f;
						Main.dust[dust].noGravity = true;
					}
				}
			}

			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
				Main.dust[dust].velocity *= 0f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			if (timeLeft < 1499)
			{
				for (int i = 0; i < 4; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
			}
		}
	}
}