using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class EaterCardProj3 : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 350;
			projectile.height = 350;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			OrchidModProjectile.resetIFrames(projectile);
			for (int i = 0; i < 10; i++)
			{
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eyexplosion");
		}
	}
}