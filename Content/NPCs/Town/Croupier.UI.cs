using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.UIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace OrchidMod.Content.NPCs.Town
{
	public class CroupierUI : ILoadable
	{
		public static CroupierUI Instance => ModContent.GetInstance<CroupierUI>();

		// ...

		public bool Visible;

		private int linesCount;
		private int emptyLinesCount;

		// ...

		void ILoadable.Load(Mod mod)
		{
			linesCount = -1;
			emptyLinesCount = -1;

			On.Terraria.Player.SetTalkNPC += ModifySetTalkNPC;

			if (Main.dedServ) return;

			On.Terraria.Main.GUIChatDrawInner += DrawOverVanillaNPCChat;
		}

		void ILoadable.Unload()
		{
			On.Terraria.Player.SetTalkNPC -= ModifySetTalkNPC;

			if (Main.dedServ) return;

			On.Terraria.Main.GUIChatDrawInner -= DrawOverVanillaNPCChat;
		}

		// ...

		public void Activate()
		{
			var list = Utils.WordwrapStringSmart(Main.npcChatText, Color.White, FontAssets.MouseText.Value, 460, 10);
			linesCount = list.Count;
			emptyLinesCount = list.FindAll(i => i.Contains(i.Find(j => j.Text == ""))).Count;

			Visible = true;

			Main.LocalPlayer.currentShoppingSettings.HappinessReport = string.Empty;
		}

		public void Deactivate(int npcIndex)
		{
			Main.LocalPlayer.SetTalkNPC(npcIndex, false);
			Main.npcChatText = Main.npc[npcIndex].GetChat();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var texture = TextureAssets.ChatBack;
			var distanceFromEdges = 16;
			var drawRect = new Rectangle(Main.screenWidth / 2 - texture.Width() / 2 + distanceFromEdges, 120 + (linesCount - emptyLinesCount) * 30, texture.Width() - distanceFromEdges * 2, emptyLinesCount * 30);

			spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawRect, Color.Black * 0.3f);

			var shadingTexture = OrchidAssets.GetExtraTexture(18);
			var topLeft = drawRect.TopLeft();
			var bottomLeft = drawRect.BottomLeft();

			spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)drawRect.Width, shadingTexture.Height()), Color.White);
			spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)bottomLeft.X, (int)bottomLeft.Y - shadingTexture.Height(), (int)drawRect.Width, shadingTexture.Height()), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

			var sbInfo = new SpriteBatchInfo(spriteBatch);
			var oldScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(drawRect, spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
			sbInfo.Begin(spriteBatch, null, null, null, null, UISystem.OverflowHiddenRasterizerState, null, null);

			// ...

			string str = string.Empty;
			for (int i = 0; i < 12; i++) str += "Eula C2 WHEN???\n";

			var t = Utils.WordwrapStringSmart(str, Main.DiscoColor, FontAssets.MouseText.Value, 460, 10).ToArray();
			var offsetY = 0f;

			foreach (var elem in t)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, elem.ToArray(), topLeft + new Vector2(4, -4) + Vector2.UnitY * offsetY, 0f, Vector2.Zero, Vector2.One, out _, -1f, 2f);
				offsetY += 30;
			}

			// ...

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = oldScissorRectangle;
			sbInfo.Begin(spriteBatch);

			var lineOffset = new Vector2(-2f, 0);
			var lineWidth = drawRect.Width + 4;

			DrawWhiteLine(spriteBatch, topLeft + lineOffset, lineWidth);
			DrawWhiteLine(spriteBatch, bottomLeft + lineOffset, lineWidth);
		}

		// ...

		private static void ModifySetTalkNPC(On.Terraria.Player.orig_SetTalkNPC orig, Player player, int npcIndex, bool fromNet)
		{
			orig(player, npcIndex, fromNet);

			Instance.Visible = false;
		}

		private static void DrawOverVanillaNPCChat(On.Terraria.Main.orig_GUIChatDrawInner orig, Main main)
		{
			var ui = Instance;
			var isVisible = ui.Visible;

			orig(main); // Visibility check should be done before drawing... Otherwise it will lead to visual bugs

			if (isVisible)
			{
				ui.Draw(Main.spriteBatch);
			}
		}

		private static void DrawWhiteLine(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			var lineRectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, 2);

			for (int i = 0; i < 4; i++)
			{
				var shadowRectangle = lineRectangle;
				shadowRectangle.X += (int)ChatManager.ShadowDirections[i].X * 2;
				shadowRectangle.Y += (int)ChatManager.ShadowDirections[i].Y * 2;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, shadowRectangle, Color.Black);
			}

			spriteBatch.Draw(TextureAssets.MagicPixel.Value, lineRectangle, Colors.AlphaDarken(Color.White));
		}

		private static Rectangle GetClippingRectangle(Rectangle rect, SpriteBatch spriteBatch)
		{
			var vector = new Vector2(rect.X, rect.Y);
			var vector2 = new Vector2(rect.Width, rect.Height) + vector;

			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			vector2 = Vector2.Transform(vector2, Main.UIScaleMatrix);

			var rectangle = new Rectangle((int)vector.X, (int)vector.Y, (int)(vector2.X - vector.X), (int)(vector2.Y - vector.Y));
			var width = spriteBatch.GraphicsDevice.Viewport.Width;
			var height = spriteBatch.GraphicsDevice.Viewport.Height;

			rectangle.X = Utils.Clamp<int>(rectangle.X, 0, width);
			rectangle.Y = Utils.Clamp<int>(rectangle.Y, 0, height);
			rectangle.Width = Utils.Clamp<int>(rectangle.Width, 0, width - rectangle.X);
			rectangle.Height = Utils.Clamp<int>(rectangle.Height, 0, height - rectangle.Y);

			return rectangle;
		}
	}
}