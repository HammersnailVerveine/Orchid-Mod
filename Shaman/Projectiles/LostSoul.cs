namespace OrchidMod.Shaman.Projectiles
{
	public class LostSoul : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Soul");
		}

		public override void SafeSetDefaults()
		{
			Projectile.CloneDefaults(297);
			AIType = 297;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 150;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = 297;
			return true;
		}
	}
}