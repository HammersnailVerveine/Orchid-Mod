using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class MeteorPhasestaffProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = 100;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 5;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phasebeam");
		}

		public override void AI()
		{
			Projectile.damage -= Projectile.timeLeft % 5 == 0 && Projectile.damage > 1 ? 1 : 0;

			int index1 = Dust.NewDust(Projectile.Center, 0, 0, 270, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
			Main.dust[index1].velocity *= 0.2f;
			Main.dust[index1].fadeIn = 1f;
			Main.dust[index1].scale = 0.8f + ((Projectile.timeLeft) / 90f) * 1.8f;
			Main.dust[index1].noGravity = true;

			if (Projectile.timeLeft == 60 || Projectile.timeLeft == 55)
			{
				int dist = Projectile.timeLeft == 55 ? 15 : 20;
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 170, dist, 15, true, 1f, 0f);
			}
		}

		public override void Kill(int timeLeft)
		{
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 170, 20, 15, true, 1f, 0f);
			int dust = Dust.NewDust(Projectile.Center, 0, 0, 270, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.2f;
			Main.dust[dust].scale = 2.5f;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			target.AddBuff((24), 1 * 60);
		}
	}
}