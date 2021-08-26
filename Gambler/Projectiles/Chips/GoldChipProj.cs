namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class GoldChipProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Chip");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 250;
			projectile.penetrate = 3;
		}
	}
}