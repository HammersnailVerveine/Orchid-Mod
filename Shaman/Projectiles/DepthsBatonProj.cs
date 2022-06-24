using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class DepthsBatonProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depths Blast");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.scale = 0f;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 45;
			Projectile.tileCollide = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 45)
			{
				Projectile.ai[0] = (Main.rand.Next(50) - 25);
				Projectile.ai[1] = (Main.rand.Next(50) - 25);
				Projectile.netUpdate = true;
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, Projectile.position + Projectile.velocity * 30f, Projectile.width, Projectile.height);
				if (lineOfSight)
				{
					Projectile.tileCollide = false;
				}
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
			}
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 25;
			}
			if (Projectile.timeLeft == 40)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[1];
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
				Projectile.tileCollide = true;
				Projectile.velocity.Y = Projectile.velocity.Y - Projectile.ai[0];
				Projectile.velocity.X = Projectile.velocity.X - Projectile.ai[1];
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 70);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
					Main.dust[dust].scale = (float)100 * 0.015f;
				}
				//projectile.penetrate = 1;
			}
			if (Projectile.timeLeft <= 10)
			{
				Projectile.damage += 2;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 70);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].scale = (float)200 * 0.015f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer) { }
	}
}