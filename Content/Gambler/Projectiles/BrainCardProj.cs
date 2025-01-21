using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.ModObjects;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class BrainCardProj : OrchidModGamblerProjectile
	{
		public static Texture2D outlineTexture;
		private int bounceDelay = 0;
		private double dustVal = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Brain");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;

			if (Main.rand.NextBool(60 - (Projectile.ai[0] > 0f ? 50 : 0)))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5);
				Main.dust[dust].velocity *= 0f;
			}

			if (Projectile.ai[0] > 0f)
			{
				Projectile.alpha -= Projectile.alpha > 0 ? 4 : 0;
				if (Main.rand.NextBool(60))
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60);
					Main.dust[dust].velocity *= 0f;
					Main.dust[dust].scale *= 1.5f;
					Main.dust[dust].noLight = true;
					Main.dust[dust].noGravity = true;
				}
			}
			else
			{
				Projectile.alpha += Projectile.alpha < 196 ? 4 : 0;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.BrainCard>())
				{
					Vector2 dustDist = Projectile.Center - player.Center;
					float minDistance = (float)Math.Sqrt(dustDist.X * dustDist.X + dustDist.Y * dustDist.Y);
					if (minDistance < 100f)
					{
						Projectile.friendly = false;
						Projectile.alpha += Projectile.alpha < 196 ? 8 : 0;
					}
					else
					{
						Projectile.friendly = Projectile.ai[0] > 0f;
					}

					if (Projectile.ai[1] == 0f)
					{
						if (this.bounceDelay <= 0)
						{
							if (modPlayer.modPlayer.Timer120 % 2 == 0)
							{
								this.spawnDust(60, 100);
							}
							this.dustVal--;

							Vector2 newMove = Main.MouseWorld - Projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);

							if (distanceTo > 1f)
							{
								newMove.Normalize();
								newMove *= distanceTo > 10f ? 10f : distanceTo;
								Projectile.velocity = newMove;
							}
							else
							{
								if (Projectile.velocity.Length() > 0f)
								{
									Projectile.velocity *= 0f;
								}
							}
							int velocityXBy1000 = (int)(newMove.X * 1000f);
							int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
							int velocityYBy1000 = (int)(newMove.Y * 1000f);
							int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

							if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
							{
								Projectile.netUpdate = true;
							}
						}
					}
					else
					{
						Vector2 newMove = new Vector2(0f, 0f);
						bool found = false;
						int projType = ProjectileType<Content.Gambler.Projectiles.BrainCardProj>();
						for (int l = 0; l < Main.projectile.Length; l++)
						{
							Projectile proj = Main.projectile[l];
							if (proj.active && proj.type == projType && proj.owner == player.whoAmI && proj.ai[1] == 0f)
							{
								found = true;
								newMove = proj.position - new Vector2(player.Center.X - (int)(Projectile.width / 2), player.Center.Y);
								break;
							}
						}
						if (found)
						{
							int rotationVal = Projectile.ai[1] == 1f ? 90 : Projectile.ai[1] == 2f ? 180 : 270;
							Projectile.position = new Vector2(player.Center.X - (int)(Projectile.width / 2), player.Center.Y) + newMove.RotatedBy(rotationVal);
						}
						else
						{
							Projectile.Kill();
						}
					}
				}
				else
				{
					Projectile.Kill();
				}
			}
		}

		public void spawnDust(int dustType, int distToCenter)
		{
			Player player = Main.player[Projectile.owner];
			for (int i = 0; i < 3; i++)
			{
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = player.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = player.Center.Y - (int)(Math.Sin(rad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 0, 0, dustType);

				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			outlineTexture ??= ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/BrainCardProj_Outline", AssetRequestMode.ImmediateLoad).Value;
			if (Projectile.friendly) DrawOutline(outlineTexture, spriteBatch, lightColor);
			return true;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			Projectile.friendly = false;
			int projType = ProjectileType<Content.Gambler.Projectiles.BrainCardProj2>();
			bool dummy = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj;
			DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, projType, Projectile.damage, 0, Projectile.owner), dummy);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 5, 10, 5 + Main.rand.Next(5), false, 1f, 1f, 5f, true, true, false, 0, 0, true);
			SoundEngine.PlaySound(SoundID.Item83, Projectile.Center);
			Projectile.ai[0] = 0f;
			target.AddBuff(BuffID.Confused, 60 * 3);
			bool skipped = false;
			bool switched = false;
			projType = ProjectileType<Content.Gambler.Projectiles.BrainCardProj>();
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					proj.damage += 30;
					if (proj.ai[1] != Projectile.ai[1])
					{
						if ((Main.rand.NextBool(2) || skipped) && !switched)
						{
							proj.ai[0] = 300f;
							switched = true;
							proj.friendly = true;
							proj.netUpdate = true;
							OrchidModProjectile.spawnDustCircle(proj.Center, 60, 10, 5 + Main.rand.Next(5), true, 1.5f, 1f, 5f, true, true, false, 0, 0, true, true);
						}
						else
						{
							skipped = true;
						}
					}
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			this.bounceDelay = 15;
			return false;
		}
	}
}