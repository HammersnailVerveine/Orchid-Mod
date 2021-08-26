using Microsoft.Xna.Framework;
using System;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
	public class CrimsonFlaskProjAlt : OrchidModAlchemistProjectile
	{
		private double dustVal = 0;
		private int sporeType = 127;
		private int sporeDamage = 0;
		private int range = 100;

		public override void SafeSetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = 0;
			projectile.timeLeft = 180;
			projectile.scale = 1f;
			projectile.penetrate = -1;
			projectile.friendly = false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom Aura");
		}

		public override void AI()
		{
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			this.dustVal++;
			if (modPlayer.timer120 % 3 == 0)
			{
				this.spawnDust(sporeType, range);
			}

			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active)
				{
					float offsetXProj = proj.Center.X - projectile.Center.X;
					float offsetYProj = proj.Center.Y - projectile.Center.Y;
					float distanceProj = (float)Math.Sqrt(offsetXProj * offsetXProj + offsetYProj * offsetYProj);
					if (distanceProj < (float)range)
					{
						if (this.sporeType == 127)
						{
							if (proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>())
							{
								sporeType = 6;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>())
							{
								sporeType = 59;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>())
							{
								sporeType = 61;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>())
							{
								sporeType = 63;
								this.sporeDamage = proj.damage;
								found = true;
								break;
							}
						}
						if (proj.type == ProjectileType<Alchemist.Projectiles.Air.CrimsonFlaskProj>())
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
			if (!found) projectile.Kill();
		}

		public void spawnDust(int dustType, int distToCenter)
		{
			for (int i = 0; i < 3; i++)
			{
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + projectile.velocity.X - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + projectile.velocity.Y - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = projectile.velocity.X == 0 ? 1.5f : (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[index2].noGravity = true;
			}
		}
	}
}