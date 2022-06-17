using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class GlowingMushroomVialProjAlt : OrchidModAlchemistProjectile
	{
		private double dustVal = 0;
		private int sporeType = 172;
		private int sporeDamage = 0;
		private Color glowColor = new Color(95, 110, 255);

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 16;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom");
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/Projectiles/Nature/GlowingMushroomVialProjAlt_Glow").Value;
			OrchidModProjectile.DrawProjectileGlowmask(Projectile, Main.spriteBatch, texture, this.glowColor);
		}

		public override void AI()
		{
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Projectile.velocity.Y += 0.01f;

			int range = 100;
			if (modPlayer.timer120 % 2 == 0)
			{
				this.spawnDust(sporeType, range);
			}
			this.dustVal--;

			if (Projectile.damage > 0)
			{
				Projectile.timeLeft = 60 * Projectile.damage * 3;
				Projectile.damage = 0;
				Projectile.netUpdate = true;
			}

			if (Main.rand.NextBool(30))
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X *= 2;
				Main.dust[dust2].velocity.Y *= 2;
			}

			Vector2 center = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float offsetX = player.Center.X - center.X;
			float offsetY = player.Center.Y - center.Y;
			float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			if (distance < (float)range)
			{
				player.AddBuff(BuffType<Alchemist.Buffs.MushroomHeal>(), 300);
			}

			if (this.sporeType == 172)
			{
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && Main.rand.Next(2) == 0)
					{
						float offsetXProj = proj.Center.X - center.X;
						float offsetYProj = proj.Center.Y - center.Y;
						float distanceProj = (float)Math.Sqrt(offsetXProj * offsetXProj + offsetYProj * offsetYProj);
						if (distanceProj < (float)range)
						{
							if (proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>())
							{
								sporeType = 6;
								this.sporeDamage = proj.damage;
								glowColor = new Color(255, 84, 0);
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>())
							{
								sporeType = 59;
								this.sporeDamage = proj.damage;
								glowColor = new Color(0, 0, 255);
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>())
							{
								sporeType = 61;
								this.sporeDamage = proj.damage;
								glowColor = new Color(0, 255, 0);
								break;
							}

							if (proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>())
							{
								sporeType = 63;
								this.sporeDamage = proj.damage;
								glowColor = new Color(190, 223, 232);
								break;
							}
						}
					}
				}
			}
			else if (Projectile.timeLeft % 60 == 0)
			{
				int projType = 0;
				if (sporeType == 6)
				{
					projType = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>();
				}

				if (sporeType == 59)
				{
					projType = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>();
				}

				if (sporeType == 61)
				{
					projType = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
				}

				if (sporeType == 63)
				{
					projType = ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>();
				}

				int rand = Main.rand.Next(3) + 1;
				for (int i = 0; i < rand; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, projType, this.sporeDamage, 0f, Projectile.owner);
					Main.projectile[spawnProj].localAI[1] = 1f;
				}
			}
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
	}
}