using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class TrueDepthProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 45;
			projectile.scale = 0f;
			projectile.extraUpdates = 1;
			projectile.tileCollide = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Depths Bolt");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			for (int index1 = 0; index1 < 3; ++index1)
			{
				Vector2 Position = projectile.position - projectile.velocity * ((float)index1 * 0.25f);
				projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(Position, 1, 1, 89, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Position;
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;

			}

			for (int index1 = 0; index1 < 3; ++index1)
			{
				Vector2 Position = projectile.position - projectile.velocity * ((float)index1 * 0.25f);
				projectile.alpha = (int)byte.MaxValue;
				int index2 = Dust.NewDust(Position, 1, 1, 21, 0.0f, 0.0f, 0, new Color(), 1f); //89 21
				Main.dust[index2].position = Position;
				Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].velocity *= 0.2f;
				Main.dust[index2].noGravity = true;

			}
			if (projectile.timeLeft == 45)
			{
				projectile.ai[0] = (Main.rand.Next(50) - 25);
				projectile.ai[1] = (Main.rand.Next(50) - 25);
			}
			for (int index1 = 0; index1 < 9; ++index1)
			{
				if (index1 % 3 == 0)
				{
					float x = projectile.position.X - projectile.velocity.X / 10f * (float)index1;
					float y = projectile.position.Y - projectile.velocity.Y / 10f * (float)index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 70, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float)100 * 0.015f;
					if (projectile.timeLeft <= 14) Main.dust[index2].scale = (float)(200 * 0.015f / projectile.timeLeft);
					Main.dust[index2].velocity *= 0.0f;
					Main.dust[index2].noGravity = true;
				}
			}
			if (projectile.ai[1] == 0)
			{
				projectile.ai[1] = 25;
				projectile.netUpdate = true;
			}
			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 25;
				projectile.netUpdate = true;
			}
			if (projectile.timeLeft == 40)
			{
				projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
				projectile.tileCollide = false;
			}
			if (projectile.timeLeft == 37)
			{
				projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
			}
			if (projectile.timeLeft == 30)
			{
				projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
			}
			if (projectile.timeLeft == 24)
			{
				projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
			if (projectile.timeLeft == 17)
			{
				projectile.velocity.Y = projectile.velocity.Y + projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X + projectile.ai[1];
			}
			if (projectile.timeLeft == 14)
			{
				projectile.velocity.Y = projectile.velocity.Y - projectile.ai[0];
				projectile.velocity.X = projectile.velocity.X - projectile.ai[1];
				projectile.tileCollide = true;
				for (int i = 0; i < 10; i++)
				{
					int dustType = 21;
					if (Main.rand.Next(2) == 0) dustType = 70;
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
					Main.dust[dust].scale = (float)100 * 0.015f;
				}
			}
			if (projectile.timeLeft <= 10)
			{
				projectile.damage++;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}