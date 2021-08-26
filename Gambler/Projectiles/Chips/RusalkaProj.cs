namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class RusalkaProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusalka");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.penetrate = 3;
		}
	}
}