using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class SeafoamVialProjAlt : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 16;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 180;
		}

		public override void AI()
		{
			projectile.rotation += 0.05f;
			projectile.velocity.Y -= 0.03f;
			projectile.velocity.X *= 0.95f;
			projectile.alpha += 3 + Main.rand.Next(3);
			if (projectile.alpha >= 255)
			{
				projectile.Kill();
			}
			projectile.velocity = (projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5)));
			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
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
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}