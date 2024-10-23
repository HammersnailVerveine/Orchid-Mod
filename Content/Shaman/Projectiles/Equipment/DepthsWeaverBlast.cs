using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Projectiles.Equipment
{

	public class DepthsWeaverBlast : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;

		public override void SafeSetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 20;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture + "_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0)
			{
				ResetIFrames();
				Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
				Projectile.ai[1] = Main.rand.NextFloat(MathHelper.Pi);
				return;
			}

			Projectile.friendly = false;
		}


		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.timeLeft < 8) colorMult *= Projectile.timeLeft / 8f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, new Color(255, 155, 55) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.2f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, new Color(153, 0, 2) * colorMult * 0.5f, Projectile.rotation + MathHelper.PiOver2, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAlt, drawPosition, null, new Color(199, 17, 3) * colorMult * 0.8f, Projectile.ai[1], TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.22f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}