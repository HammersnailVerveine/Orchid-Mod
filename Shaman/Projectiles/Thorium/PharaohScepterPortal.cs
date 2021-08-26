using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class PharaohScepterPortal : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 38;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			Main.projFrames[projectile.type] = 9;
			projectile.alpha = 32;
			projectile.tileCollide = false;
			projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Portal");
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (projectile.timeLeft == 500)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 2f;
				}
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 7 == 0)
				projectile.frame++;
			if (projectile.frame == 9)
				projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				int dust = Dust.NewDust(pos, projectile.width, projectile.height, 64, 0f, 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}

			if (projectile.timeLeft % 10 == 0)
			{
				spawnDustCircle(64, Main.rand.Next(6) + 15);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + projectile.velocity.X - 6;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + projectile.velocity.Y - 6;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
			}
		}
	}
}