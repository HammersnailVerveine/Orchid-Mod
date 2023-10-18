using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class FeatherScepterProj : OrchidModShamanProjectile
	{
		private bool rapidFade = false;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Feather");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 200;
			Projectile.penetrate = -1;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 200)
				Projectile.rotation += (float)Main.rand.Next(20);

			Projectile.rotation += 0.6f;
			Projectile.velocity = Projectile.velocity * 0.95f;

			if (Projectile.timeLeft < 85)
				Projectile.alpha += 3;

			if (Projectile.timeLeft < 150 && Projectile.timeLeft > 100)
			{
				Projectile.rotation += 0.05f;
			}

			if (Projectile.timeLeft == 140)
			{
				Projectile.damage += 5;
				Projectile.velocity *= 0f;
				spawnDustCircle(16, 20);
			}

			if (rapidFade)
				Projectile.alpha += 3;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double dustDeg = (i * (18));//    + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index1].velocity *= 0.05f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f;
				Main.dust[index1].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X = 0f;
			Projectile.velocity.Y = 0f;
			Projectile.timeLeft /= 2;
			rapidFade = true;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}