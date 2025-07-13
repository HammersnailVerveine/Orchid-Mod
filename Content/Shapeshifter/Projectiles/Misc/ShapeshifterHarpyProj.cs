using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Misc
{
	public class ShapeshifterHarpyProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureGlow;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.scale = 0.85f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 3;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureGlow ??= ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			if (!Initialized)
			{ // random texture
				Initialized = true;
				Projectile.ai[0] = Main.rand.Next(3);

				SoundStyle soundStyle = SoundID.Item65;
				soundStyle.Volume *= 0.33f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				Projectile.scale = Main.rand.NextFloat(0.75f, 0.85f);

				for (int i = 0; i < 5; i++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
					dust2.noGravity = true;
				}
			}

			if (OldPosition.Count > 20 || (Projectile.ai[1] == 1f && OldPosition.Count > 0))
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Projectile.ai[1] == 1f)
			{
				if (Projectile.timeLeft > 60)
				{
					Projectile.timeLeft = 60;
				}

				Projectile.velocity *= 0.75f;
			}
			else
			{
				Projectile.tileCollide = Projectile.position.Y + Projectile.height >= Projectile.ai[2];
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[1] = 1f;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			//SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureGlow, drawPosition2, null, lightColor * 0.015f * (i + 1) * colorMult, OldRotation[i], TextureGlow.Size() * 0.5f, Projectile.scale * (i + 1) * 0.04f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Rectangle rectangle = TextureMain.Bounds;
			rectangle.Height /= 3; // 3 frames;
			rectangle.Y += rectangle.Height * (int)Projectile.ai[0];
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, rectangle, lightColor * colorMult, Projectile.rotation, rectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}