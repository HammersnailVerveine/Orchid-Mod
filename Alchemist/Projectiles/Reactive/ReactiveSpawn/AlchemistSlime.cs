using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Alchemist.Projectiles.Reactive.ReactiveSpawn
{
	public class AlchemistSlime : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemic Slime");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 10;
			projectile.aiStyle = 63;
			projectile.friendly = true;
			projectile.timeLeft = 600;
			projectile.penetrate = 10;
			projectile.scale = 1f;
			projectile.alpha = 64;
			Main.projFrames[projectile.type] = 2;
		}

		public override void AI()
		{
			if (projectile.velocity.Y > 8) projectile.velocity.Y = 8;
			if (projectile.velocity.X > 4) projectile.velocity.X = 4;
			if (projectile.velocity.X < -4) projectile.velocity.X = -4;
			projectile.frame = projectile.velocity.Y < 0f ? 1 : 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity.Y = -3;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int Alpha = 175;
				Color newColor = new Color(0, 80, 255, 0);
				int dust = Dust.NewDust(projectile.position + Vector2.One * 6f, projectile.width, projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.7f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noLight = true;
			}
		}
	}
}
