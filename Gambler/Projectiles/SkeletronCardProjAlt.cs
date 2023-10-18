using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkeletronCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Water Bolt");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.CloneDefaults(123);
			AIType = 123;
			Projectile.timeLeft = 180;
		}

		public override void SafeAI()
		{
			if (Main.rand.Next(5) == 0)
			{
				int dustType = 59;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
		}
	}
}