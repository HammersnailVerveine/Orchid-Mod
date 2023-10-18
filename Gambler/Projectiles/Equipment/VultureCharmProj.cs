using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles.Equipment
{
	public class VultureCharmProj : OrchidModGamblerProjectile
	{
		private bool rapidFade = false;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vulture Feather");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			this.projectileTrail = true;
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.direction = Projectile.spriteDirection;
			Projectile.velocity *= 0.98f;
			Projectile.alpha += Projectile.timeLeft < 120 ? Projectile.timeLeft < 60 ? 3 : 2 : 0;
			Projectile.alpha += this.rapidFade ? 3 : 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X *= 0.01f;
			Projectile.velocity.Y *= 0.01f;
			Projectile.timeLeft /= 2;
			rapidFade = true;
			Projectile.tileCollide = false;
			return false;
		}
	}
}