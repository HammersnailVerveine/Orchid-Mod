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
			projectile.CloneDefaults(297);
			aiType = 297;
			projectile.tileCollide = false;
			projectile.timeLeft = 150;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = 297;
			return true;
		}
	}
}