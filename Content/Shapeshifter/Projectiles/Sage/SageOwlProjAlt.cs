using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageOwlProjAlt : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
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

			float scalemult = (float)Math.Sin(Projectile.timeLeft * 0.1046f) * 0.75f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, (30 - Projectile.timeLeft) * 1.41667f);
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.Aqua * scalemult, MathHelper.PiOver4, TextureMain.Size() * 0.5f, Projectile.scale * (1f + (30 - Projectile.timeLeft) * 0.02f), SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);


			return false;
		}
	}
}