using Terraria;

namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class HellChipProjAlt : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 100;
			projectile.height = 100;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			for (int i = 0; i < 20; i++)
			{
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			OrchidModProjectile.spawnExplosionGore(projectile);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}
	}
}