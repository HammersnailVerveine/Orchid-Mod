using Terraria;

namespace OrchidMod.Content.Shaman.Projectiles
{
	public class BlumProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.penetrate = 3;
			Projectile.CloneDefaults(123);
			AIType = 123;
			Projectile.timeLeft = 23;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = 123;
			return true;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}