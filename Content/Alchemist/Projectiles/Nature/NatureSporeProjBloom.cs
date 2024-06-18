using OrchidMod.Common.ModObjects;
using Terraria;

namespace OrchidMod.Content.Alchemist.Projectiles.Nature
{
	public class NatureSporeProjBloom : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nature Spore");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 1)
			{
				Projectile.friendly = true;
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 44, 5, 5 + Main.rand.Next(3), true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
			}
		}
	}
}