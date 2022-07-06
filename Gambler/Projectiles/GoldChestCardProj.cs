using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class GoldChestCardProj : OrchidModGamblerProjectile
	{
		bool redDust = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sparkle");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 3;
			Main.projFrames[Projectile.type] = 4;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeAI()
		{
			if (!this.initialized)
			{
				this.initialized = true;
				this.redDust = Projectile.ai[1] > 1f;
				Projectile.frame = (int)Projectile.ai[1];
			}

			Projectile.velocity *= 0.985f;

			Projectile.rotation += this.redDust ? 0.05f : -0.05f;

			// if (Main.rand.Next(8) == 0) {
			// int dustType = this.redDust ? 60 : 59;
			// Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			// int index = Dust.NewDust(pos, projectile.width, projectile.height, dustType);
			// Main.dust[index].velocity *= 0.25f;
			// Main.dust[index].scale *= 1.5f;
			// Main.dust[index].noGravity = false;
			// }

			if (Main.rand.Next(3) == 0)
			{
				int Type = Main.rand.Next(3);
				if (Type == 0) Type = 15;
				if (Type == 1) Type = 57;
				if (Type == 2) Type = 58;
				int index = Dust.NewDust(Projectile.position - Projectile.velocity * 0.25f, Projectile.width, Projectile.height, Type, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
				Main.dust[index].velocity *= 0.2f;
				Main.dust[index].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 0.5f;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			int dustType = this.redDust ? 60 : 59;
			OrchidModProjectile.spawnDustCircle(Projectile.Center, dustType, 10, 5, true, 1.5f, 1f, 5f);
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType)].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(31, 60 * 2); // Confused
			}
		}
	}
}