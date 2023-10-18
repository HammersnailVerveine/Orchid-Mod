using Terraria;

namespace OrchidMod.Content.Gambler.Projectiles.Chips
{
	public class MeteorDetonatorProj : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
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
			Player player = Main.player[Projectile.owner];
			Projectile.width = Projectile.damage * 4;
			Projectile.height = Projectile.width;
			Projectile.position.X = player.Center.X - Projectile.width / 2;
			Projectile.position.Y = player.Center.Y - Projectile.width / 2;

			for (int i = 0; i < 20; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			OrchidModProjectile.spawnExplosionGore(Projectile);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, (int)(Projectile.width / 6), 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, (int)(Projectile.width / 3), 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, (int)(Projectile.width / 2), 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, (int)(Projectile.width / 6), 15, true, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, (int)(Projectile.width / 3), 15, true, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, (int)(Projectile.width / 2), 15, true, 1.5f, 1f, 5f);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom Explosion");
		}
	}
}