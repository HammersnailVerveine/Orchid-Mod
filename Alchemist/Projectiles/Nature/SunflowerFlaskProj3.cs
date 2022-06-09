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
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void AI()
		{
			if (!this.initialized)
			{
				this.rotationDirection = Main.rand.Next(2) == 0 ? 1 : -1;
				this.initialized = true;
			}

			Projectile.rotation += 0.04f * this.rotationDirection;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 250);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}