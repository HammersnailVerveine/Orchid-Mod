using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Alchemist.Projectiles.Air
{
	public class QueenBeeFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bee");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
		}

		public override void AI()
		{
			Projectile.alpha += 3 + Main.rand.Next(3);
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			Projectile.ai[1] = Projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}
	}
}