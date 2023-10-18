using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class AbyssPrecinctProjExplosion : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 200;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Abyssal Explosion");
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}