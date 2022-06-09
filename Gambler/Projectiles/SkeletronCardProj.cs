using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkeletronCardProj : OrchidModGamblerProjectile
	{
		private int bounceDelay = 0;
		private double dustVal = 0;
		private int projectilePoll = 10;
		private int fireProj = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron Might");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.gamblingChipChance = 5;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;

			if (Projectile.ai[1] == 0f)
			{
				Projectile.friendly = true;
				Projectile.rotation += 0.25f;
				Projectile.frame = 1;
			}
			else
			{
				Projectile.friendly = false;
				Projectile.rotation = 0f;
				Projectile.frame = 0;
			}
			Projectile.frame = (int)Projectile.ai[1];

			if (Main.myPlayer == Projectile.owner)
			{
				if (modPlayer.timer120 % 2 == 0)
				{
					this.spawnDust(59, 250);
				}
				this.dustVal--;
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.SkeletronCard>() && modPlayer.GamblerDeckInHand)
				{
					if (this.bounceDelay <= 0)
					{
						Vector2 newMove = Main.MouseWorld - Projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo > 5f)
						{
							newMove *= 2f / distanceTo;
							Projectile.velocity = newMove;
						}
						else
						{
							if (Projectile.velocity.Length() > 0f)
							{
								Projectile.velocity *= 0f;
							}
						}

						if (distanceTo > 250f)
						{
							if (Projectile.ai[1] == 0f)
							{
								Projectile.ai[1] = 1f;
								Projectile.netUpdate = true;
							}
							if (modPlayer.timer120 % (int)(60 / this.projectilePoll) == 0)
							{
								this.fireProj++;
							}

							if (this.fireProj == 5)
							{
								Vector2 projMove = newMove = Main.MouseWorld - Projectile.Center;
								projMove *= 10f / distanceTo;
								int projType = ProjectileType<Gambler.Projectiles.SkeletronCardProjAlt>();
								bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
								OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, projMove.X, projMove.Y, projType, (int)(Projectile.damage * 3), Projectile.knockBack, Projectile.owner), dummy);
								SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 8);
								this.projectilePoll -= this.projectilePoll - 1 <= 0 ? 0 : 1;
								this.fireProj = 0;
							}
						}
						else
						{
							if (Projectile.ai[1] == 1f)
							{
								Projectile.ai[1] = 0f;
								Projectile.netUpdate = true;
								this.fireProj = 0;
							}
							if (modPlayer.timer120 % 30 == 0)
							{
								this.projectilePoll += this.projectilePoll + 1 > 10 ? 0 : 1;
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
					Projectile.Kill();
				}
			}
		}

		public override void SafePostAI()
		{
			for (int num46 = Projectile.oldPos.Length - 5; num46 > 0; num46--)
			{
				Projectile.oldPos[num46] = Projectile.oldPos[num46 - 1];
			}
			Projectile.oldPos[0] = Projectile.position;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Projectile.ai[1] == 1f && Projectile.rotation == 0f)
			{
				Texture2D flameTexture = ModContent.GetTexture("OrchidMod/Gambler/Projectiles/SkeletronCardProj_Glow");
				Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 1f, Projectile.height * 1f);
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					drawPos.X += Main.rand.Next(6) - 3 - Main.player[Projectile.owner].velocity.X;
					drawPos.Y += Main.rand.Next(6) - 3 - Main.player[Projectile.owner].velocity.Y;
					Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k * 5) / (float)Projectile.oldPos.Length);
					spriteBatch.Draw(flameTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.3f);
				}
			}
			return true;
		}

		public void spawnDust(int dustType, int distToCenter)
		{
			Player player = Main.player[Projectile.owner];
			for (int i = 0; i < 3; i++)
			{
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter);
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter);

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 0, 0, dustType);

				Main.dust[index2].scale = 1f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			this.bounceDelay = 15;
			return false;
		}
	}
}
