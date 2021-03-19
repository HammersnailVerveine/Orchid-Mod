using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OrchidMod
{
	public static class OrchidHelper
	{
		public static void DrawSimpleHeadGlowmask(PlayerDrawInfo drawInfo, Texture2D texture, Color color)
		{
			Player player = drawInfo.drawPlayer;
			if (player.head > 0 && !player.invis)
			{
				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.headPosition + drawInfo.headOrigin;
				var drawData = new DrawData(texture, position, new Rectangle?(player.bodyFrame), color, player.headRotation, drawInfo.headOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.headArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		}

		public static void DrawSimpleBodyGlowmask(PlayerDrawInfo drawInfo, Texture2D texture, Color color)
		{
			Player player = drawInfo.drawPlayer;
			if (player.body > 0 && !player.invis)
			{
				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (player.bodyFrame.Width / 2) + (player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + player.height - player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2(player.bodyFrame.Width / 2, player.bodyFrame.Height / 2);
				var drawData = new DrawData(texture, position, new Rectangle?(player.bodyFrame), color, player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.bodyArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		}

		public static void DrawSimpleLegsGlowmask(PlayerDrawInfo drawInfo, Texture2D texture, Color color)
		{
			Player player = drawInfo.drawPlayer;
			if (player.legs > 0 && (player.shoe != 15 || player.wearsRobe) && !player.invis)
			{
				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(player.legFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.legFrame.Height + 4f))) + player.legPosition + drawInfo.legOrigin;
				var drawData = new DrawData(texture, position, new Rectangle?(player.legFrame), color, player.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.legArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		}

		public static void DrawSimpleArmsGlowmask(PlayerDrawInfo drawInfo, Texture2D texture, Color color)
		{
			Player player = drawInfo.drawPlayer;
			if (player.body > 0 && !player.invis)
			{
				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2));
				var drawData = new DrawData(texture, position, new Rectangle?(player.bodyFrame), color, player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.bodyArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		}

		public static void DrawSimpleItemGlowmaskInWorld(Item item, SpriteBatch spriteBatch, Texture2D texture, Color color, float rotation, float scale)
		{
			Vector2 offset = new Vector2(0, 2); // Unnecessary in 1.4
			spriteBatch.Draw(texture, item.position - Main.screenPosition + item.Size * 0.5f + offset, null, color, rotation, item.Size * 0.5f, scale, SpriteEffects.None, 0f);
		}

		// public static void DrawSimpleItemGlowmaskOnPlayer(PlayerDrawInfo drawInfo, Texture2D texture, Color color) { }
	}
}
