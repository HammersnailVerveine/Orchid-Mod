using Terraria;

namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class HellChipProjAlt : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			for (int i = 0; i < 20; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			OrchidModProjectile.spawnExplosionGore(Projectile);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Explosion");
		}
	}
}