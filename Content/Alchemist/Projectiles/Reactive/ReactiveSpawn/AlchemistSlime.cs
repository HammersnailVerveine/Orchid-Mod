using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Alchemist.Projectiles.Reactive.ReactiveSpawn
{
	public class AlchemistSlime : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alchemic Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 10;
			Projectile.aiStyle = 63;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 10;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void AI()
		{
			if (Projectile.velocity.Y > 8) Projectile.velocity.Y = 8;
			if (Projectile.velocity.X > 4) Projectile.velocity.X = 4;
			if (Projectile.velocity.X < -4) Projectile.velocity.X = -4;
			Projectile.frame = Projectile.velocity.Y < 0f ? 1 : 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.Y = -3;
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int Alpha = 175;
				Color newColor = new Color(0, 80, 255, 0);
				int dust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.7f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noLight = true;
			}
		}
	}
}
