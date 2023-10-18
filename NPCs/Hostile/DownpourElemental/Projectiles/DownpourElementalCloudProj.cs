using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.NPCs.Hostile.DownpourElemental.Projectiles
{
	public class DownpourElementalCloudProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 14;
			Projectile.hostile = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 350;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Storm Bolt");
		}

		public override void AI()
		{
			int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, 0f, 0f, 125, default(Color), 1f);
			Main.dust[DustID2].noGravity = true;
			Main.dust[DustID2].velocity = Projectile.velocity / 2;

			Projectile.tileCollide = Projectile.timeLeft < 320;

			if (Projectile.timeLeft % 15 == 0)
			{
				Projectile.ai[0] = (Main.rand.Next(40) - 20);
				Vector2 projectileVelocity = (new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] / 2)));
				Projectile.velocity = projectileVelocity;
			}
		}
	}
}