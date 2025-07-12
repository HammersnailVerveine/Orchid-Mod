using Terraria;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSalamortarProjAlt : OrchidModShapeshifterProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.friendly = true;
		}
	}
}