using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OrchidMod.Utilities
{
	public static class DrawUtils
	{
		public static void DrawSimpleItemGlowmaskInWorld(this SpriteBatch spriteBatch, Item item, Texture2D texture, Color color, float rotation, float scale)
		{
			spriteBatch.Draw(texture, item.position - Main.screenPosition + item.Size * 0.5f, null, color, rotation, item.Size * 0.5f, scale, SpriteEffects.None, 0f);
		}
	}
}