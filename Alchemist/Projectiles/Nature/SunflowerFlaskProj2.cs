using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class SunflowerFlaskProj2 : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunflower");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 20;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 3);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}