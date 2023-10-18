using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class KingSlimeCardProj2 : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 75;
			Projectile.height = 75;
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
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, 175, new Color(0, 80, 255, 0));
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slimexplosion");
		}
	}
}