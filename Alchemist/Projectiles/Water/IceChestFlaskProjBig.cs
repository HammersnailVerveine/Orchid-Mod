using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class IceChestFlaskProjBig : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.penetrate = 1;
			projectile.friendly = true;
			Main.projFrames[projectile.type] = 3;
			this.projectileTrail = true;
			projectile.alpha = 128;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flash Freeze Flake");
		}

		public override void AI()
		{
			projectile.velocity.Y += 0.05f;
			projectile.rotation += (projectile.velocity.Y * 1.5f) / 20f;
		}

		public override void Kill(int timeLeft)
		{
			int range = 125;
			// OrchidModProjectile.spawnDustCircle(projectile.Center, 67, (int)(range / 2), 10, true, 1.5f, 1f, 6f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 67, 15, 8, true, 1.5f, 1f, 8f);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 67, 10, 6, true, 1.5f, 1f, 6f);
			spawnGenericExplosion(projectile, projectile.damage, 1f, range * 2, 2, false, 27);
		}
	}
}