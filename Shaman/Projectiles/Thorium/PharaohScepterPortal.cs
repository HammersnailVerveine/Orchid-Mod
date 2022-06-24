using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class PharaohScepterPortal : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 9;
			Projectile.alpha = 32;
			Projectile.tileCollide = false;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Portal");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.timeLeft == 500)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 2f;
				}
			}

			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 % 7 == 0)
				Projectile.frame++;
			if (Projectile.frame == 9)
				Projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, 64, 0f, 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}

			if (Projectile.timeLeft % 10 == 0)
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

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + Projectile.velocity.X - 6;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + Projectile.velocity.Y - 6;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 2f;
			}
		}
	}
}