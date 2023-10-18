using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class BloodMoonFlaskProj : OrchidModAlchemistProjectile
	{
		float rotationSpeed = 0f;

		public override void SafeSetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blood Mist");
		}

		public override void AI()
		{
			if (!this.initialized)
			{
				Projectile.frame = Main.rand.Next(2);
				this.initialized = true;
				Projectile.rotation += Main.rand.NextFloat();
				this.rotationSpeed = (0.01f + Main.rand.NextFloat() * 0.03f) * (Main.rand.NextBool(2)? 1f : -1f);
			}
			Projectile.rotation += this.rotationSpeed;
			Projectile.velocity *= 0.925f;
			Projectile.alpha += Main.rand.Next(3);
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		// public override void Kill(int timeLeft)
		// {
		// for(int i=0; i<4; i++)
		// {
		// int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
		// Main.dust[dust].velocity *= 1.5f;
		// Main.dust[dust].scale *= 1f;
		// }
		// }
	}
}