using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class EyeCardProj : OrchidModGamblerProjectile
	{
		public const int Radius = 16 * 16;
		public const int Cooldown = 35;

		private double dustVal = 0;
		private int cooldownCounter = 0;

		//public override string Texture => OrchidAssets.InvisiblePath;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye");

			//Main.projFrames[Type] = Main.npcFrameCount[NPCID.DemonEye];

			ProjectileID.Sets.TrailingMode[Type] = 0;
			ProjectileID.Sets.TrailCacheLength[Type] = 7;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 25;
			Projectile.height = 25;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
		}

		public override void SafeAI()
		{
			if (++Projectile.frameCounter >= 10)
			{
				Projectile.frameCounter = 0;

				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}

			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;

			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.direction = Projectile.spriteDirection;

			cooldownCounter -= cooldownCounter > 0 ? 1 : 0;
			Projectile.friendly = cooldownCounter <= 0 && this.initialized && Projectile.velocity.Length() > 1f;

			if (Projectile.ai[1] == 1)
			{
				var velocityLength = Projectile.velocity.Length();

				if (velocityLength >= 4f)
				{
					Projectile.velocity *= 0.965f;
				}
				else if (velocityLength >= 0.5f)
				{
					Projectile.velocity *= 0.8f;
				}
				else
				{
					Projectile.ai[1] = 0;
					cooldownCounter = Cooldown;
					this.initialized = true;
				}
			}

			if (Main.myPlayer == Projectile.owner)
			{
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.EyeCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = Main.MouseWorld - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (Projectile.ai[1] == 0)
					{
						AdjustMagnitude(ref newMove, distanceTo > Radius && cooldownCounter <= 0);
						Projectile.velocity = (5 * Projectile.velocity + newMove);
						AdjustMagnitude(ref Projectile.velocity, distanceTo > Radius && cooldownCounter <= 0);
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
				else
				{
					if (Projectile.ai[1] == 0)
					{
						Projectile.Kill();
					}
				}
			}

			if (modPlayer.modPlayer.timer120 % 2 == 0 && Projectile.ai[1] == 0)
			{
				this.spawnDust(35);
			}
			this.dustVal--;
		}

		public void spawnDust(int dustType)
		{
			for (int i = 0; i < 3; i++)
			{
				double deg = (4 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * Radius) + Projectile.velocity.X - 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * Radius) + Projectile.velocity.Y - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity / 2;
				Main.dust[index2].scale = 1f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector, bool dash)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			float init = dash ? 12f : 0.01f;
			if (magnitude > init)
			{
				vector *= init / magnitude;
			}
			Projectile.ai[1] = dash ? 1f : 0f;
		}

		/*
		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var texture = TextureAssets.Npc[NPCID.DemonEye];
			var height = texture.Height() / Main.projFrames[Type];
			var rectangle = new Rectangle(0, height * Projectile.frame, texture.Width(), height);

			for (int k = 1; k < Projectile.oldPos.Length; k++)
			{
				var progress = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				var drawPosTrail = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size * 0.5f + Vector2.UnitY * Projectile.gfxOffY;
				var color = Color.Lerp(lightColor, Color.Red, progress) * 0.3f * progress;
				var scale = Projectile.scale * (0.95f - (1 - progress) * 0.2f);
				Main.EntitySpriteDraw(texture.Value, drawPosTrail, rectangle, color, Projectile.rotation, rectangle.Size() * 0.5f, scale, SpriteEffects.FlipHorizontally, 0);
			}

			spriteBatch.Draw(texture.Value, Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY, rectangle, lightColor, Projectile.rotation, rectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

			return false;
		}
		*/
	}
}