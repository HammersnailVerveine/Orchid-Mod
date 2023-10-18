namespace OrchidMod.Shaman.Projectiles.Equipment.Hell
{
	public class ShamanHellExplosion : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Explosion");
		}
	}
}