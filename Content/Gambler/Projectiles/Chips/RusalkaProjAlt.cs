using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Gambler.Projectiles.Chips
{
	public class RusalkaProjAlt : OrchidModGamblerProjectile
	{
		private Vector2 initialVelocity = new Vector2(0f, 0f);

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rusalka's Waters");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 360;
			Projectile.alpha = 255;
		}

		// public override Color? GetAlpha(Color lightColor)
		// {
		// return Color.White;
		// }

		public override void SafeAI()
		{
			bool moreDust = Projectile.timeLeft > 300;
			if (Main.rand.NextBool(moreDust ? 5 : 10))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(2) == 0 ? 29 : 59);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= moreDust ? 2.5f : 1f;
			}

			if (!this.Initialized)
			{
				this.Initialized = true;
				this.initialVelocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
				Projectile.velocity *= 0f;
			}

			if (Projectile.timeLeft == 300)
			{
				Projectile.velocity = this.initialVelocity;
				this.projectileTrail = true;
				Projectile.alpha = 0;
				Projectile.friendly = true;
				Projectile.aiStyle = 2;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(2) == 0 ? 29 : 59);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}