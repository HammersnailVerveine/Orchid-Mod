using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenEaterStem: OrchidModShapeshifterProjectile
	{
		private int Timespent = 0;
		private static Texture2D TextureMain;
		private static Texture2D TextureLeaves;
		private static Texture2D TextureRange;

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
			TextureRange ??= ModContent.Request<Texture2D>(Texture + "_Range", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
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

			Projectile.timeLeft ++;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
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
				if (distToMaxRange < 96f)
				{
					Color boderColor = new Color(162, 22, 15);
					spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
					spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

					spriteBatch.Draw(TextureRange, drawPosition, null, boderColor * 0.8f * ((96f - distToMaxRange) / 96f), 0f, TextureRange.Size() * 0.5f, 1.8f * (Projectile.ai[1] / 296f), SpriteEffects.None, 0f);

					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}

				spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, lightColor, rotation, drawRectangle.Size() * 0.5f, Projectile.scale * 1.2f, SpriteEffects.None, 0f);

				Random random = new Random((int)Projectile.ai[0]); // generates the random from the "seed" contained in ai[0], in order to pcik the random stem textures
				int count = 0;

				while ((Projectile.Center - segment * count).Distance(owner.Center) > drawRectangle.Height && count < 100)
				{
					count++;
					drawRectangle.Y = (random.Next(10) + 1) * drawRectangle.Height;
					drawPosition = Projectile.Center - segment * count - Main.screenPosition;
					drawPosition += Vector2.UnitY.RotatedBy(rotation - MathHelper.PiOver2) * (float)Math.Sin((Timespent + count * 5) * 0.1f);

					spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, lightColor, rotation, drawRectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

					if (random.Next(5) > 2)
					{ // leaves & thorns
						int randHeight = random.Next(9);
						drawRectangleLeaves.Y = randHeight * drawRectangleLeaves.Height;
						int side = random.Next(2) == 0 ? 1 : -1;
						drawPosition += Vector2.UnitY.RotatedBy(rotation - MathHelper.PiOver2) * (drawRectangleLeaves.Height * 0.5f + 1) * side;
						float rotationRand = (float)Math.Sin((random.Next(6) + Timespent + count * 5) * 0.133f) * 0.075f;
						if (randHeight < 5) rotationRand *= 2f; // leaves wiggle more
						SpriteEffects effect = side == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
						spriteBatch.Draw(TextureLeaves, drawPosition, drawRectangleLeaves, lightColor, rotation + rotationRand, drawRectangleLeaves.Size() * 0.5f, Projectile.scale, effect, 0f);
					}
				}
			}

			return false;
		}
	}
}