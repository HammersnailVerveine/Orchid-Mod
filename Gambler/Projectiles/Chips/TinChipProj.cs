namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class TinChipProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Chip");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 250;
			projectile.penetrate = 2;
		}
	}
}