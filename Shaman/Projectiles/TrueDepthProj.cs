using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class TrueDepthProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 45;
			Projectile.scale = 0f;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("True Depths Bolt");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			for (int index1 = 0; index1 < 3; ++index1)
			{
				Vector2 Position = Projectile.position - Projectile.velocity * ((float)index1 * 0.25f);
				Projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(Position, 1, 1, 89, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Position;
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;

			}

			for (int index1 = 0; index1 < 3; ++index1)
			{
				Vector2 Position = Projectile.position - Projectile.velocity * ((float)index1 * 0.25f);
				Projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(Position, 1, 1, 21, 0.0f, 0.0f, 0, new Color(), 1f); //89 21
				Main.dust[index2].position = Position;
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;

			}
			if (Projectile.timeLeft == 45)
			{
				Projectile.ai[0] = (Main.rand.Next(50) - 25);
				Projectile.ai[1] = (Main.rand.Next(50) - 25);
			}
			for (int index1 = 0; index1 < 9; ++index1)
			{
				if (index1 % 3 == 0)
				{
					float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)index1;
					float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 70, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = Projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float)100 * 0.015f;
					if (Projectile.timeLeft <= 14) Main.dust[index2].scale = (float)(200 * 0.015f / Projectile.timeLeft);
					Main.dust[index2].velocity *= 0.0f;
					Main.dust[index2].noGravity = true;
				}
			}
			if (Projectile.ai[1] == 0)
			{
				Projectile.ai[1] = 25;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 25;
				Projectile.netUpdate = true;
			}
			if (Projectile.timeLeft == 40)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[1];
				Projectile.tileCollide = false;
			}
			if (Projectile.timeLeft == 37)
			{
				Projectile.velocity.Y = Projectile.velocity.Y - Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X - Projectile.ai[1];
			}
			if (Projectile.timeLeft == 30)
			{
				Projectile.velocity.Y = Projectile.velocity.Y - Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X - Projectile.ai[1];
			}
			if (Projectile.timeLeft == 24)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[1];
			}
			if (Projectile.timeLeft == 17)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[1];
			}
			if (Projectile.timeLeft == 14)
			{
				Projectile.velocity.Y = Projectile.velocity.Y - Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X - Projectile.ai[1];
				Projectile.tileCollide = true;
				for (int i = 0; i < 10; i++)
				{
					int dustType = 21;
					if (Main.rand.Next(2) == 0) dustType = 70;
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
					Main.dust[dust].scale = (float)100 * 0.015f;
				}
			}
			if (Projectile.timeLeft <= 10)
			{
				Projectile.damage++;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}