using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class PoisonVialProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.alpha = 128;
			Projectile.penetrate = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Bubble");
		}

		public override void AI()
		{
			Projectile.velocity *= 0.9f;
			Projectile.rotation += 0.02f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 44);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}
	}
}