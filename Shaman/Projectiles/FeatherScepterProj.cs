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
			DisplayName.SetDefault("Feather");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 34;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 200;
			projectile.penetrate = -1;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 200)
				projectile.rotation += (float)Main.rand.Next(20);

			projectile.rotation += 0.6f;
			projectile.velocity = projectile.velocity * 0.95f;

			if (projectile.timeLeft < 85)
				projectile.alpha += 3;

			if (projectile.timeLeft < 150 && projectile.timeLeft > 100)
			{
				projectile.rotation += 0.05f;
			}

			if (projectile.timeLeft == 140)
			{
				projectile.damage += 5;
				projectile.velocity *= 0f;
				spawnDustCircle(16, 20);
			}

			if (rapidFade)
				projectile.alpha += 3;
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double dustDeg = (i * (18));//    + 5 - Main.rand.Next(10)));
				double dustRad = dustDeg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - 4;

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
			projectile.velocity.X = 0f;
			projectile.velocity.Y = 0f;
			projectile.timeLeft /= 2;
			rapidFade = true;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}