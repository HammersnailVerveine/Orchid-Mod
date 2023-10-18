using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.Equipment.Viscount
{
	public class ViscountOrbSound : OrchidModShamanProjectile
	{
		public int timer = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1200;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sound Bubble");
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;

			this.timer = this.timer > 90 ? 1 : this.timer + 1;
			if (this.timer % 30 == 0)
			{
				spawnDustCircle(89, (int)(3 * (this.timer / 30)));
			}

			Player player = Main.player[Projectile.owner];
			Vector2 center = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y)
			{
				/*
				if (Projectile.owner == Main.myPlayer && !Main.LocalPlayer.moonLeech)
				{
					OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
					modPlayer.shamanFireTimer += modPlayer.shamanFireTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanWaterTimer += modPlayer.shamanWaterTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanAirTimer += modPlayer.shamanAirTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanEarthTimer += modPlayer.shamanEarthTimer == 0 ? 0 : 3 * 60;
					modPlayer.shamanSpiritTimer += modPlayer.shamanSpiritTimer == 0 ? 0 : 3 * 60;
					Projectile.Kill();
				}
				*/
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 15; i++)
			{
				double deg = (i * (54 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1.2f;
				Main.dust[index2].scale = 1.2f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 89);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}