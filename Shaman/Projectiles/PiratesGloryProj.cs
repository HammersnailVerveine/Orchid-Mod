using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class PiratesGloryProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 42;
			Projectile.extraUpdates = 5;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pirate's Magic");
		}

		public override void AI()
		{
			for (int index1 = 0; index1 < 3; ++index1)
			{
				Projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity = Projectile.velocity / 4;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].noGravity = true;
			}

			if (Projectile.timeLeft % 7 == 0)
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 127, 20, 20, true, 1f);
			if ((Projectile.timeLeft - 5) % 7 == 0)
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 127, 10, 20, true, 1f);
		}

		public override void OnKill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 127, 10, 20, true, 1f);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 127, 20, 20, true, 1f);
			if (modPlayer.GetNbShamanicBonds() > 2)
				target.AddBuff((72), 5 * 60); // Midas
		}
	}
}