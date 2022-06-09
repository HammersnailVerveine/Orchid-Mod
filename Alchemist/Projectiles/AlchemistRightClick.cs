using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles
{
	public class AlchemistRightClick : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}
	}
}