using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class KingSlimeCardProj2 : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 75;
			projectile.height = 75;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, 0.0f, 0.0f, 175, new Color(0, 80, 255, 0));
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimexplosion");
		}
	}
}