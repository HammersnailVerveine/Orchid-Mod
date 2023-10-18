using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class StarScouterScepterProjAltExplosion : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 200;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Orbital Explosion");
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}