using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class IceChestFlaskProjSmall : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Main.projFrames[Projectile.type] = 3;
			this.projectileTrail = true;
			Projectile.alpha = 128;
		}

		// public override Color? GetAlpha(Color lightColor) {
		// return Color.White;
		// }

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flash Freeze Flake");
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.05f;
			Projectile.rotation += (Projectile.velocity.Y * 1.5f) / 20f;
		}

		public override void OnKill(int timeLeft)
		{
			int range = 50;
			// OrchidModProjectile.spawnDustCircle(projectile.Center, 67, (int)(range / 2), 5, true, 1.5f, 1f, 6f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 67, 5, 6, true, 1f, 1f, 4f);
			spawnGenericExplosion(Projectile, Projectile.damage, 1f, range * 2, 2, false, false);
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
		}
	}
}