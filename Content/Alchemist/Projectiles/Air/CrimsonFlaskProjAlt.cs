using Microsoft.Xna.Framework;
using System;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Projectiles.Air
{
	public class CrimsonFlaskProjAlt : OrchidModAlchemistProjectile
	{
		private double dustVal = 0;
		private int sporeType = 127;
		private int sporeDamage = 0;
		private int range = 100;

		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom Aura");
		}

		public override void AI()
		{
			Player player = Main.player[Main.myPlayer];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();

			this.dustVal++;
			if (modPlayer.modPlayer.timer120 % 3 == 0)
			{
				this.spawnDust(sporeType, range);
			}

			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active)
				{
					float offsetXProj = proj.Center.X - Projectile.Center.X;
					float offsetYProj = proj.Center.Y - Projectile.Center.Y;
					float distanceProj = (float)Math.Sqrt(offsetXProj * offsetXProj + offsetYProj * offsetYProj);
					if (distanceProj < (float)range)
					{
						if (this.sporeType == 127)
						{
							if (proj.type == ProjectileType<Content.Alchemist.Projectiles.Fire.FireSporeProj>())
							{
								sporeType = 6;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}

							if (proj.type == ProjectileType<Content.Alchemist.Projectiles.Water.WaterSporeProj>())
							{
								sporeType = 59;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}

							if (proj.type == ProjectileType<Content.Alchemist.Projectiles.Nature.NatureSporeProj>())
							{
								sporeType = 61;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}

							if (proj.type == ProjectileType<Content.Alchemist.Projectiles.Air.AirSporeProj>())
							{
								sporeType = 63;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}
						}
						if (proj.type == ProjectileType<Content.Alchemist.Projectiles.Air.CrimsonFlaskProj>())
						{
							if (this.sporeType != 127)
							{
								proj.ai[1] = this.sporeType;
								proj.ai[0] = this.sporeDamage;
								proj.netUpdate = true;
							}
							found = true;
						}
					}
				}
			}
			if (!found) Projectile.Kill();
		}

		public void spawnDust(int dustType, int distToCenter)
		{
			for (int i = 0; i < 3; i++)
			{
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + Projectile.velocity.X - 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + Projectile.velocity.Y - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = Projectile.velocity.X == 0 ? 1.5f : (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}
	}
}