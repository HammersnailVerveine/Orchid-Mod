using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class SpiritDropletScepterProj : OrchidModShamanProjectile
	{

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spirit Bone");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 100;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 100)
			{
				float rand = (float)(Main.rand.Next(4) - 2f);
				Projectile.velocity.X += rand;
				Projectile.velocity.Y += rand;
			}

			Projectile.rotation += 0.25f;

			Projectile.velocity = Projectile.velocity * 0.95f;

			Projectile.alpha += 2;

			for (int i = 0; i < 1; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 3f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}