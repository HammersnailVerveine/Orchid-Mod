using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Projectiles.Nature
{
	public class SunflowerFlaskProj2 : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sunflower");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 3);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}