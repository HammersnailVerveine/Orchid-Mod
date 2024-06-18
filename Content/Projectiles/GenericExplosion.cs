using OrchidMod.Common.ModObjects;
using Terraria.ModLoader;

namespace OrchidMod.Content.Projectiles
{
	public class GenericExplosion : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 250;
			Projectile.height = 250;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			OrchidModProjectile.ResetIFrames(Projectile);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Explosion");
		}
	}
}