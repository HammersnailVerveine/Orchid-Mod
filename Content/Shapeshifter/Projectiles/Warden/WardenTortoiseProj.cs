using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenTortoiseProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<float> OldAI;
		public Color drawColor;

		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			OldAI = new List<float>();
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 30)
			{
				Projectile.rotation = Main.rand.NextFloat(-1f, 1f);
				Projectile.localAI[0] = -24f;

				if (Main.rand.NextBool())
				{
					drawColor = new Color(114, 132, 51);
				}
				else
				{
					drawColor = new Color(170, 105, 70);
				}

				if (Projectile.ai[0] > 0)
				{
					Projectile.scale = 1.25f;
					Projectile.width = 40;
					Projectile.height = 40;
					Projectile.localAI[0] = -32f;
				}
			}

			Projectile.friendly = Projectile.timeLeft > 20;
			Projectile.localAI[0] *= 0.8f;
			Projectile.velocity *= 0.8f;

			OldAI.Add(Projectile.localAI[0]);

			if (OldAI.Count > 7)
			{
				OldAI.RemoveAt(0);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			if (lightColor.R < 96) lightColor.R = 96;
			if (lightColor.G < 96) lightColor.G = 96;
			if (lightColor.B < 96) lightColor.B = 96;

			for (int i = -1; i < 2 ; i += 2) 
			{
				SpriteEffects effect = i < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

				for (int j = 0; j < OldAI.Count; j++)
				{
					Vector2 drawPosition2 = Vector2.Transform(Projectile.Center - new Vector2(0f, OldAI[j] * i).RotatedBy(Projectile.rotation) - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
					spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.075f * (j + 1) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, effect, 0f);
				}

				Vector2 drawPosition = Vector2.Transform(Projectile.Center - new Vector2(0f, Projectile.localAI[0] * i).RotatedBy(Projectile.rotation) - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, lightColor.MultiplyRGBA(drawColor) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, effect, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}