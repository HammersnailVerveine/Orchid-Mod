namespace OrchidMod.Gambler.Projectiles
{
	public class MushroomCardProjExplosion : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			this.gamblingChipChance = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom Explosion");
		}
	}
}