using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Projectiles.Pets
{
	public class HauntedCandle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Haunted Candle");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}
			if (player.dead)
			{
				modPlayer.hauntedCandle = false;
			}
			if (modPlayer.hauntedCandle)
			{
				Projectile.timeLeft = 2;
			}


			if (Main.rand.Next(3) <= 1) Projectile.ai[0]++;

			if (Projectile.ai[0] <= 155) Projectile.ai[0] += 3;
			if (Projectile.ai[0] >= 365) Projectile.ai[0] += 3;

			if (Projectile.ai[0] <= 255)
			{
				if (Projectile.ai[0] >= 50)
				{
					if (Main.rand.Next(255 - (int)Projectile.ai[0]) <= 20)
					{
						Vector2 pos = new Vector2(Projectile.position.X + 8, Projectile.position.Y - 5);
						Main.dust[Dust.NewDust(pos, Projectile.width / 10, Projectile.height / 10, 269, 0f, 0f, 6, default(Color), 0.8f)].velocity *= 0.1f;
					}
					if (Main.rand.Next(20) == 0)
					{
						int dustType = 16;
						Vector2 pos = new Vector2(Projectile.position.X + 3, Projectile.position.Y + 5);
						Main.dust[Dust.NewDust(pos, Projectile.width / 2, Projectile.height / 2, dustType)].velocity *= 0.01f;
					}
				}
				Projectile.alpha = (int)(255 - Projectile.ai[0]);
			}

			if (Projectile.ai[0] > 255 && Projectile.ai[0] < 750)
			{
				if (Projectile.ai[0] <= 450)
				{
					if (Main.rand.Next(255 + ((int)Projectile.ai[0]) - 255) <= 20)
					{
						Vector2 pos = new Vector2(Projectile.position.X + 8, Projectile.position.Y - 5);
						Main.dust[Dust.NewDust(pos, Projectile.width / 10, Projectile.height / 10, 269, 0f, 0f, 6, default(Color), 0.8f)].velocity *= 0.1f;
					}
					if (Main.rand.Next(20) == 0)
					{
						int dustType = 16;
						Vector2 pos = new Vector2(Projectile.position.X + 3, Projectile.position.Y + 5);
						Main.dust[Dust.NewDust(pos, Projectile.width / 2, Projectile.height / 4, dustType)].velocity *= 0.01f;
					}
				}
				Projectile.alpha = (int)(Projectile.ai[0] - 255);
			}


			if (Projectile.ai[0] >= 510)
			{
				Projectile.ai[0] = 0;
				if (Main.rand.Next(2) == 0) Projectile.position.X = player.position.X - 20;
				else Projectile.position.X = player.position.X + 20;
				Projectile.position.Y = player.position.Y;
			}

			Projectile.velocity.X = 0;
			Projectile.velocity.Y = 0;
		}
	}
}