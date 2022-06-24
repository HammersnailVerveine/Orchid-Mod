using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Reactive.ReactiveSpawn
{
	public class AlchemistSlimeLava : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemic Lava Slime");
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
			if (Projectile.velocity.X > 5) Projectile.velocity.X = 5;
			if (Projectile.velocity.X < -5) Projectile.velocity.X = -5;
			Projectile.frame = Projectile.velocity.Y < 0f ? 1 : 0;

			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.Y = -3;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int Alpha = 175;
				Color newColor = new Color(255, 80, 0, 0);
				int dust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.7f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noLight = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60 * 3);
		}
	}
}
