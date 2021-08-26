using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class PiratesGloryProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 42;
			projectile.extraUpdates = 5;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate's Magic");
		}

		public override void AI()
		{
			for (int index1 = 0; index1 < 3; ++index1)
			{
				projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 127, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity = projectile.velocity / 4;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].noGravity = true;
			}

			if (projectile.timeLeft % 7 == 0)
				OrchidModProjectile.spawnDustCircle(projectile.Center, 127, 20, 20, true, 1f);
			if ((projectile.timeLeft - 5) % 7 == 0)
				OrchidModProjectile.spawnDustCircle(projectile.Center, 127, 10, 20, true, 1f);
		}

		public override void Kill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(projectile.Center, 127, 10, 20, true, 1f);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			OrchidModProjectile.spawnDustCircle(projectile.Center, 127, 20, 20, true, 1f);
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 2)
				target.AddBuff((72), 5 * 60); // Midas
		}
	}
}