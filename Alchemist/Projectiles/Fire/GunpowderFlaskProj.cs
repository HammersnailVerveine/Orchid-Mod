using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Fire
{
	public class GunpowderFlaskProj : AlchemistProjCatalyst
	{
		public override void SetDefaults()
		{
			Projectile.width = 250;
			Projectile.height = 250;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void CatalystInteractionEffect(Player player) { }

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
			OrchidModProjectile.resetIFrames(Projectile);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}
	}
}