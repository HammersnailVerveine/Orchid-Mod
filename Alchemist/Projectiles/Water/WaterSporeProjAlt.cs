using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class WaterSporeProjAlt : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Spore");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			projectile.alpha += 3 + Main.rand.Next(3);
			if (projectile.alpha >= 255)
			{
				projectile.Kill();
			}
			projectile.velocity = (projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5)));
			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			projectile.ai[1] = projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 29);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}