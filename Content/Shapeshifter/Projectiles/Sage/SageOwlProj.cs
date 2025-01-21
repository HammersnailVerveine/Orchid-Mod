using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageOwlProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 45;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 3;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item64, Projectile.Center);
			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.05f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);


			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, lightColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}