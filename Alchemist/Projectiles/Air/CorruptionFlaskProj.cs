using Microsoft.Xna.Framework;
using System;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Air
{
	public class CorruptionFlaskProj : OrchidModAlchemistProjectile
	{
		private double dustVal = 0;
		private int sporeType = 21;
		private int sporeDamage = 0;
		private int range = 50;

		public override void SafeSetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 24;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Main.projFrames[Projectile.type] = 7;
			this.catalytic = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom");
		}

		public override void AI()
		{
			Player player = Main.player[Main.myPlayer];
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			this.initialized = true;

			if (Projectile.velocity.Y > 0) Projectile.timeLeft++;
			Projectile.velocity.Y += 0.05f;

			this.dustVal++;
			if (modPlayer.timer120 % 3 == 0)
			{
				this.spawnDust(sporeType, range);
			}

			if (Projectile.timeLeft == 1)
			{
				Projectile.timeLeft = 10 + (int)(Projectile.damage / 5);
				Projectile.frame++;
				if (Projectile.frame >= 7) Projectile.Kill();
			}

			if (this.sporeType == 21)
			{
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && Main.rand.Next(2) == 0)
					{
						float offsetXProj = proj.Center.X - Projectile.Center.X;
						float offsetYProj = proj.Center.Y - Projectile.Center.Y;
						float distanceProj = (float)Math.Sqrt(offsetXProj * offsetXProj + offsetYProj * offsetYProj);
						if (distanceProj < (float)range)
						{
							if (proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>())
							{
								sporeType = 6;
								this.sporeDamage = proj.damage;
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>())
							{
								sporeType = 59;
								this.sporeDamage = proj.damage;
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>())
							{
								sporeType = 61;
								this.sporeDamage = proj.damage;
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>())
							{
								sporeType = 63;
								this.sporeDamage = proj.damage;
								break;
							}
						}
					}
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false; //so it sticks to platforms
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0f;
			return false;
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

		public override void Catalyze(Player player, Projectile projectile, OrchidModGlobalProjectile modProjectile)
		{
			if (this.initialized) projectile.Kill();
		}

		public override void OnKill(int timeLeft)
		{
			int damage = (int)((Projectile.damage / 8) * (Projectile.frame + 1));
			int range = (int)(25 * (Projectile.frame + 1));
			int nb = (int)(10 * (Projectile.frame + 1));
			OrchidModProjectile.spawnDustCircle(Projectile.Center, sporeType, range, nb, true, 1.5f, 1f, 8f);
			spawnGenericExplosion(Projectile, damage, Projectile.knockBack, range * 2, 2, true, true);
			int spawnProj = 0;
			int spawnProj2 = 0;

			switch (sporeType)
			{
				case 6:
					spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>();
					spawnProj2 = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
					break;
				case 59:
					spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>();
					spawnProj2 = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
					break;
				case 61:
					spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
					spawnProj2 = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
					break;
				case 63:
					spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>();
					spawnProj2 = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
					break;
				default:
					break;
			}

			if (spawnProj != 0)
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int newSpore = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, spawnProj, this.sporeDamage, 0f, Projectile.owner);
					Main.projectile[newSpore].localAI[1] = 1f;
				}
				for (int i = 0; i < 5; i++)
				{
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, spawnProj2, 0, 0f, Projectile.owner);
				}
			}
		}
	}
}