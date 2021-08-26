using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class SunflowerFlaskProj3 : OrchidModAlchemistProjectile
	{
		private int rotationDirection = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunflower");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void AI()
		{
			if (!this.initialized)
			{
				this.rotationDirection = Main.rand.Next(2) == 0 ? 1 : -1;
				this.initialized = true;
			}

			projectile.rotation += 0.04f * this.rotationDirection;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 250);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}