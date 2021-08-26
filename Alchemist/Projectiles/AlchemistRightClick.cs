using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles
{
	public class AlchemistRightClick : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 1;
			projectile.height = 1;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
		}
	}
}