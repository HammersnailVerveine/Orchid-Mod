using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Alchemist.Projectiles.Water
{
	public class WaterSporeProjAlt : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Water Spore");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			this.projectileTrail = true;
		}

		public override void AI()
		{
			Projectile.alpha += 3 + Main.rand.Next(3);
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
			Projectile.velocity = (Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5)));
			if (Main.rand.NextBool(15))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			Projectile.ai[1] = Projectile.ai[1] == -1 ? 1 : -1;
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 29);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}