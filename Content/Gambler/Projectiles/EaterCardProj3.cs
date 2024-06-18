using OrchidMod.Common.ModObjects;
using Terraria;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class EaterCardProj3 : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 350;
			Projectile.height = 350;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			OrchidModProjectile.ResetIFrames(Projectile);
			for (int i = 0; i < 10; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Eyexplosion");
		}
	}
}