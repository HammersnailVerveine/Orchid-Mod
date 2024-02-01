namespace OrchidMod.Content.Alchemist.Projectiles.Water
{
	public class SlimeFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Explosion");
		}

		public override void AI()
		{
			OrchidModProjectile.ResetIFrames(Projectile);
		}
	}
}