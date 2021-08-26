using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Equipment
{
	public class HarpyAnkletProj : OrchidModShamanProjectile
	{
		private bool rapidFade = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feather");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 100;
			projectile.penetrate = 15;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 200)
				projectile.rotation += (float)Main.rand.Next(20);

			projectile.rotation += 0.5f;
			projectile.velocity = projectile.velocity * 0.95f;
			if (projectile.timeLeft < 85) projectile.alpha += 3;
			if (rapidFade) projectile.alpha += 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity.X = 0f;
			projectile.velocity.Y = 0f;
			projectile.timeLeft /= 2;
			rapidFade = true;
			return false;
		}
	}
}