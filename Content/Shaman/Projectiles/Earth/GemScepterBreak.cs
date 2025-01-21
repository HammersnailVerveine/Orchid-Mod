using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Projectiles.Earth
{
	public class GemScepterBreak : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;
		private Color DrawColor;

		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 15;
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

				DrawColor = Color.White;

				switch (Projectile.ai[0])
				{
					case 1: // Amber
						DrawColor = new Color(207, 109, 0);
						break;
					case 2: // Amethist
						DrawColor = new Color(207, 0, 236);
						break;
					case 3: // Topaz
						DrawColor = new Color(255, 221, 62);
						break;
					case 4: // Sapphire
						DrawColor = new Color(107, 210, 252);
						break;
					case 5: // Emerald
						DrawColor = new Color(10, 152, 98);
						break;
					case 6: // Ruby
						DrawColor = new Color(238, 51, 53);
						break;
					case 7: // Diamond
						DrawColor = new Color(223, 230, 238);
						break;
					default: 
						break;
				}
				return;
			}

			Projectile.friendly = false;
		}


		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.timeLeft < 5) colorMult *= Projectile.timeLeft / 5f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, DrawColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.2f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, DrawColor * colorMult * 0.5f, Projectile.rotation + MathHelper.PiOver2, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAlt, drawPosition, null, DrawColor * colorMult * 0.8f, Projectile.ai[1], TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.22f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}
