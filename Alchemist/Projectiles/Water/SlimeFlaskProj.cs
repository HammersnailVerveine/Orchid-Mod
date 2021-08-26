namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class SlimeFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 200;
			projectile.height = 200;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}

		public override void AI()
		{
			OrchidModProjectile.resetIFrames(projectile);
		}
	}
}