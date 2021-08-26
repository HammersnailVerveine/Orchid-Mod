using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkeletronCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Bolt");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.CloneDefaults(123);
			aiType = 123;
			projectile.timeLeft = 180;
			this.gamblingChipChance = 20;
		}

		public override void SafeAI()
		{
			if (Main.rand.Next(5) == 0)
			{
				int dustType = 59;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}
	}
}