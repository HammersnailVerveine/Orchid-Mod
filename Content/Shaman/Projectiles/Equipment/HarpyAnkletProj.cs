using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Shaman.Projectiles.Equipment
{
	public class HarpyAnkletProj : OrchidModShamanProjectile
	{
		private bool rapidFade = false;
		
		public override void SafeSetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 100;
			Projectile.penetrate = 15;
			this.projectileTrail = true;
		}

		public override void SafeAI()
		{
			if (Projectile.timeLeft == 200)
				Projectile.rotation += (float)Main.rand.Next(20);

			Projectile.rotation += 0.5f;
			Projectile.velocity = Projectile.velocity * 0.95f;
			if (Projectile.timeLeft < 85) Projectile.alpha += 3;
			if (rapidFade) Projectile.alpha += 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X = 0f;
			Projectile.velocity.Y = 0f;
			Projectile.timeLeft /= 2;
			rapidFade = true;
			return false;
		}
	}
}