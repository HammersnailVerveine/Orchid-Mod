using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class SlimeRainCardProj2 : OrchidModGamblerProjectile
	{
		private bool greenSlime = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 10;
			Projectile.aiStyle = 63;
			Projectile.friendly = true;
			Projectile.timeLeft = 180;
			Projectile.penetrate = 10;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			Main.projFrames[Projectile.type] = 4;
			this.gamblingChipChance = 10;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 180)
			{
				this.greenSlime = Main.rand.Next(2) == 0;
			}

			Projectile.aiStyle = this.initialized ? 63 : 0;

			if (Projectile.timeLeft % 10 == 0 && !this.initialized)
			{
				Projectile.damage++;
			}

			if (Projectile.velocity.Y > 8) Projectile.velocity.Y = 8;
			if (Projectile.velocity.X > 4) Projectile.velocity.X = 4;
			if (Projectile.velocity.X < -4) Projectile.velocity.X = -4;
			Projectile.frame = Projectile.velocity.Y < 0f ? 1 + (this.greenSlime ? 2 : 0) : 0 + (this.greenSlime ? 2 : 0);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.timeLeft = this.initialized ? Projectile.timeLeft : 90;
			this.initialized = true;
			Projectile.velocity.Y = -3;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerGambler modPlayer)
		{
			this.gamblingChipChance = 3;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Color dustColor = this.greenSlime ? new Color(0, 255, 70, 0) : new Color(0, 80, 255, 0);
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, 175, dustColor);
			}
		}
	}
}
