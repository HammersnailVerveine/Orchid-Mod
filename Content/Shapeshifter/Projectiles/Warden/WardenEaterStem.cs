using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenEaterStem: OrchidModShapeshifterProjectile
	{
		private int Timespent = 0;
		private static Texture2D TextureMain;
		private static Texture2D TextureLeaves;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 10;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.netImportant = true;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureLeaves ??= ModContent.Request<Texture2D>(Texture + "_Leaves", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Timespent = 0;
		}

		public override void AI()
		{
			Timespent++;
			Player owner = Owner;

			if (!owner.active)
			{
				Projectile.Kill();
			}

			if (IsLocalOwner)
			{
				if (owner.dead)
				{
					Projectile.Kill();
				}

				OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
				if (shapeshifter.IsShapeshifted)
				{
					if (shapeshifter.Shapeshift is not WardenEater)
					{
						Projectile.Kill();
					}
				}
				else
				{
					Projectile.Kill();
				}
			}


			if (Timespent < 60)
			{ // spawn animation sounds
				Rectangle drawRectangle = TextureMain.Bounds;
				drawRectangle.Height /= 11; // there are 11 textures for the stem, including the basis in first position

				Vector2 segment = Projectile.Center - owner.Center;
				segment = Vector2.Normalize(segment) * drawRectangle.Height;

				int amountSegments = 0;
				while ((Projectile.Center - segment * amountSegments).Distance(owner.Center) > drawRectangle.Height && amountSegments < 100)
				{ // counts the number of total segments for the stem, used for the spawn animation
					amountSegments++;
				}

				if (Timespent == amountSegments)
				{
					SoundEngine.PlaySound(SoundID.Dig);
				}
				else if (Timespent % 2 == 0 && Timespent <= amountSegments)
				{
					SoundEngine.PlaySound(SoundID.Grass);
				}
			}

			Projectile.timeLeft ++;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{ // Stem drawing activity
			Player owner = Owner;
			if (owner.active && !owner.dead)
			{
				Rectangle drawRectangle = TextureMain.Bounds;
				drawRectangle.Height /= 11; // there are 11 textures for the stem, including the basis in first position

				Rectangle drawRectangleLeaves = TextureLeaves.Bounds;
				drawRectangleLeaves.Height /= 8; // there are 8 textures for the leaves

				Vector2 segment = Projectile.Center - owner.Center;
				segment = Vector2.Normalize(segment) * drawRectangle.Height;
				float rotation = segment.ToRotation() - MathHelper.PiOver2;

				Vector2 drawPosition = Projectile.Center - Main.screenPosition;

				float distToMaxRange = Projectile.ai[1] - Projectile.Center.Distance(owner.Center);

				int amountSegments = 0;
				while ((Projectile.Center - segment * amountSegments).Distance(owner.Center) > drawRectangle.Height && amountSegments < 100)
				{ // counts the number of total segments for the stem, used for the spawn animation
					amountSegments++;
				}

				if (Timespent >= amountSegments)
				{
					spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, lightColor, rotation, drawRectangle.Size() * 0.5f, Projectile.scale * 1.2f, SpriteEffects.None, 0f);
				}

				Random random = new Random((int)Projectile.ai[0]); // generates the random from the "seed" contained in ai[0], in order to pcik the random stem textures

				float sineMult = 4f * ((distToMaxRange > 0 ? distToMaxRange : 0)  / Projectile.ai[1]); // less wavy stem if the player is further away

				Vector2 scaleSquish = new Vector2(Projectile.scale, Projectile.scale);
				if (distToMaxRange < 16f)
				{
					scaleSquish.X += 0.015f * (distToMaxRange - 16f);
					if (scaleSquish.X < 0.5f)
					{
						scaleSquish.X = 0.5f;
					}
				}

				int count = 0;
				while (count < amountSegments)
				{
					count++;
					if (Timespent >= (amountSegments - count))
					{
						if (Timespent == amountSegments - count)
						{ // Spawn animation : the end of the stem is always the first frame
							drawRectangle.Y = 0;
						}
						else
						{
							drawRectangle.Y = (random.Next(10) + 1) * drawRectangle.Height;
						}

						drawPosition = Projectile.Center - segment * count;
						Color color = Lighting.GetColor((int)(drawPosition.X / 16f), (int)(drawPosition.Y / 16f));
						drawPosition -= Main.screenPosition;
						if (count < 4) sineMult *= count / 3f; // less wavy towards the base of the stem
						drawPosition += Vector2.UnitY.RotatedBy(rotation - MathHelper.PiOver2) * (float)Math.Sin((Timespent + count * 5) * 0.1f) * sineMult; // wave offset

						spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, color, rotation, drawRectangle.Size() * 0.5f, scaleSquish, SpriteEffects.None, 0f);

						if (random.Next(5) > 2)
						{ // leaves & thorns
							int randHeight = random.Next(9);
							drawRectangleLeaves.Y = randHeight * drawRectangleLeaves.Height;
							float side = random.Next(2) == 0 ? 1f : -1f;
							SpriteEffects effect = side == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

							if (distToMaxRange < 16f)
							{ // moves leaves & thorns closer to the stem when it retracts
								float buffer = side * 0.66f;
								side *= 0.33f * distToMaxRange / 16f;
								side += buffer;

								if ((buffer > 0 && side < 0) || (buffer < 0 && side > 0))
								{
									side = 0;
								}
							}

							drawPosition += Vector2.UnitY.RotatedBy(rotation - MathHelper.PiOver2) * (drawRectangleLeaves.Height * 0.5f + 1) * side;
							float rotationRand = (float)Math.Sin((random.Next(6) + Timespent + count * 5) * 0.133f) * 0.075f;
							if (randHeight < 5) rotationRand *= 2f; // leaves wiggle more
							spriteBatch.Draw(TextureLeaves, drawPosition, drawRectangleLeaves, color, rotation + rotationRand, drawRectangleLeaves.Size() * 0.5f, scaleSquish, effect, 0f);
						}
					}
				}
			}

			return false;
		}
	}
}