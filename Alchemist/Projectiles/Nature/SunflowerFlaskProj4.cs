using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class SunflowerFlaskProj4 : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Petal");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 6;
			projectile.height = 12;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			//this.projectileTrail = true;
		}

		public override void AI()
		{
			projectile.rotation += 0.2f;
			projectile.alpha += 3 + Main.rand.Next(3);
			if (projectile.alpha >= 255)
			{
				projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}
	}
}