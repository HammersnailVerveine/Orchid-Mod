using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class GuardianHorizonLanceProjAlt : OrchidModGuardianProjectile
	{
		public Color DrawColor;
		private static Texture2D TextureMain;

		public override void AltSetDefaults()
		{
			Projectile.width = 140;
			Projectile.height = 140;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			DrawColor = new Color(216, 61, 5);
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.ai[0];
			Projectile.friendly = Projectile.timeLeft > 20;
			DrawColor.R -= 4;
			DrawColor.G += 4;
			DrawColor.B += 10;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;


			Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.DarkGray.MultiplyRGB(DrawColor) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * 1.8f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, DrawColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * 1.8f, SpriteEffects.None, 0f);

			for (int i = -1; i < 2; i += 2)
			{
				Vector2 drawPosition2 = drawPosition + Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver4 * i + MathHelper.PiOver2) * 40f;
				spriteBatch.Draw(TextureMain, drawPosition2, null, Color.DarkGray.MultiplyRGB(DrawColor) * colorMult * 0.75f, Projectile.rotation - MathHelper.PiOver4 * i, TextureMain.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureMain, drawPosition2, null, DrawColor * colorMult * 0.75f, Projectile.rotation - MathHelper.PiOver4 * i, TextureMain.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None, 0f);
			}

			for (int i = -1; i < 2; i += 2)
			{
				Vector2 drawPosition2 = drawPosition + Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver2) * 52f + Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.PiOver2 * i + MathHelper.PiOver2) * 40f;
				spriteBatch.Draw(TextureMain, drawPosition2, null, Color.DarkGray.MultiplyRGB(DrawColor) * colorMult * 0.5f, Projectile.rotation - MathHelper.PiOver2 * i, TextureMain.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureMain, drawPosition2, null, DrawColor * colorMult * 0.5f, Projectile.rotation - MathHelper.PiOver2 * i, TextureMain.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}