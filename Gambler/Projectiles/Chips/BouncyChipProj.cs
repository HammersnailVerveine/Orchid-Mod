using Microsoft.Xna.Framework;

namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class BouncyChipProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambling Chip");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 600;
			projectile.penetrate = 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}
	}
}