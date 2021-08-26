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
			projectile.width = 22;
			projectile.height = 20;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.alpha = 255;
		}

		public override void SafeAI()
		{
			if (projectile.owner != Main.myPlayer)
			{
				projectile.active = false;
			}

			if (!this.initialized)
			{
				if (projectile.timeLeft < 1440)
				{
					this.initialized = true;
					projectile.velocity *= 0f;
					projectile.alpha = 0;
					projectile.damage = 1;
					for (int i = 0; i < 4; i++)
					{
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
						Main.dust[dust].velocity *= 1.5f;
						Main.dust[dust].scale *= 1f;
					}
				}
				else
				{
					projectile.velocity.Y += 0.1f;
					if (Main.rand.Next(3) == 0)
					{
						int dust = Dust.NewDust(projectile.Center, 1, 1, 18);
						Main.dust[dust].velocity *= 0f;
						Main.dust[dust].noGravity = true;
					}
				}
			}

			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
				Main.dust[dust].velocity *= 0f;
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
			if (timeLeft < 1499)
			{
				for (int i = 0; i < 4; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].scale *= 1f;
				}
			}
		}
	}
}