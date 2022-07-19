using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkyCardProj : OrchidModGamblerProjectile
	{
		//private Texture2D texture;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 57;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Main.projFrames[Projectile.type] = 2;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if (Projectile.ai[1] < 2f) return base.GetAlpha(lightColor);
			Color yellow = Color.Yellow * 0.5f;
			return new Color(yellow.R + lightColor.R, yellow.G + lightColor.G, yellow.B + lightColor.B);
		}

		public override void OnSpawn(IEntitySource source)
		{
			for (int i = 0; i < 5; i++) {
				int dustType = DustID.YellowStarDust;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override void SafeAI()
		{
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.25f);
			this.checkMouseDrag();

			if (Projectile.ai[1] >= 2f)
			{
				if (Main.rand.NextBool(10))
				{
					int dustType = DustID.YellowStarDust;
					Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
					Dust dust = Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)];
					dust.velocity = Projectile.velocity / 5f;
				}

				Projectile.rotation += Projectile.velocity.Length() / 30.5f * (Projectile.velocity.X > 0 ? 1f : -1f);
				if (Projectile.frame == 1) Projectile.timeLeft--;
			}
		}

		public void checkMouseDrag()
		{
			Projectile proj = Main.projectile[(int)Projectile.ai[0]];

			if (proj.type != ProjectileType<Gambler.Projectiles.SkyCardBase>() || proj.active == false && Projectile.ai[1] < 2f)
			{
				Projectile.Kill();
			}

			if (Projectile.ai[1] == 0f)
			{
				proj.ai[0]++;
				Projectile.timeLeft++;
				if (Projectile.velocity.X > 0f)
				{
					Projectile.localAI[1] = Projectile.velocity.X;
					Projectile.velocity.X = 0f;
				}

				Projectile.position = proj.Center - new Vector2(Projectile.width, Projectile.height - 20) * 0.5f;

				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Vector2 newMove = Main.MouseWorld - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 25f)
					{
						Projectile.ai[1] = 1f;
						Projectile.netUpdate = true;
						Projectile.localAI[0] = Main.myPlayer;
					}
				}
			}

			if (Projectile.ai[1] == 1f)
			{
				proj.ai[0]++;
				Projectile.timeLeft++;
				if ((int)Projectile.localAI[0] == Main.myPlayer)
				{
					if (Main.mouseLeft)
					{
						Vector2 newMove = Main.MouseWorld - proj.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						float range = 40f;
						if (distanceTo > range)
						{
							newMove.Normalize();
							Projectile.position = proj.Center + newMove * range - new Vector2(Projectile.width, Projectile.height) * 0.5f;
						}
						else
						{
							Projectile.position = Main.MouseWorld - new Vector2(Projectile.width, Projectile.height) * 0.5f;
						}
					}
					else
					{
						Vector2 newMove = proj.Center - Projectile.Center;
						newMove.Normalize();
						newMove *= Projectile.localAI[1];
						Projectile.velocity = newMove;
						Projectile.ai[1] = 2f;
						Projectile.tileCollide = true;
						Projectile.friendly = true;
						Projectile.netUpdate = true;
						Projectile.alpha = 0;
						SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			int dustid = DustID.YellowStarDust;

			if (Projectile.frame == 1 && Projectile.penetrate == 1 && Projectile.ai[1] == 2f)
			{
				dustid = DustID.Cloud;
				int projType = ProjectileType<Gambler.Projectiles.SkyCardProjAlt>();
				int newProjectile = (DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), getDummy()));
				Main.projectile[newProjectile].ai[0] = Projectile.ai[0];
				Main.projectile[newProjectile].netUpdate = true;
			}


			OrchidModProjectile.spawnDustCircle(Projectile.position, DustID.YellowStarDust, 5, 4 + Main.rand.Next(3), true, 1f, 4f, 2f);
			for (int i = 0; i < 3; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustid);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}

			//SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
		}
	}
}