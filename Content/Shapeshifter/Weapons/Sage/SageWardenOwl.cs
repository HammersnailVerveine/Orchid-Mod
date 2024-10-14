using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Weapons.Sage
{
	public class SageWardenOwl : OrchidModProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int Frame = 0;
		public int Timespent = 0;

		public override void AltSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 3000;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture + "_Shapeshift", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Frame = 2;
		}

		public override void AI()
		{
			Timespent++;
			Projectile.velocity.Y += 0.05f;

			if (Timespent % 6 == 0 && Timespent > 0)
			{
				Frame++;
				if (Frame == 3)
				{
					Projectile.velocity.Y = -1;
					SoundEngine.PlaySound(SoundID.Item32, Projectile.Center);
				}
					
				if (Frame == 2)
				{
					Timespent = -5;
				}

				if (Frame == 1)
				{
					Timespent = -3;
				}

				if (Frame == 7)
				{
					Frame = 1;
				}
			}

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			Rectangle drawRectangle = TextureMain.Bounds;
			drawRectangle.Height = drawRectangle.Width;
			drawRectangle.Y += drawRectangle.Height * Frame;

			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition2, drawRectangle, lightColor * 0.075f * (i + 1) * colorMult, OldRotation[i], drawRectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			spriteBatch.Draw(TextureMain, drawPosition, drawRectangle, lightColor, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}