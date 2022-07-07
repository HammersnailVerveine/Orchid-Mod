using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.UIs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace OrchidMod.Content.NPCs.Town
{
	[Autoload(Side = ModSide.Client)]
	public class CroupierUI : ILoadable
	{
		public static CroupierUI Instance => ModContent.GetInstance<CroupierUI>();

		public bool Visible;

		private Asset<Texture2D> cardBlockTexture;
		private Asset<Texture2D> cardSlotsTexture;
		private Asset<Texture2D> shadingTexture;

		private int scrollValue;
		private int scrollMax;

		// ...

		void ILoadable.Load(Mod mod)
		{
			cardBlockTexture = ModContent.Request<Texture2D>(OrchidAssets.UIsPath + "CroupierUICardBlock", AssetRequestMode.ImmediateLoad);
			cardSlotsTexture = ModContent.Request<Texture2D>(OrchidAssets.UIsPath + "CroupierUICardSlots", AssetRequestMode.ImmediateLoad);
			shadingTexture = OrchidAssets.GetExtraTexture(18);

			On.Terraria.Player.SetTalkNPC += ModifySetTalkNPC;
			On.Terraria.Main.GUIChatDrawInner += DrawOverVanillaNPCChat;
		}

		void ILoadable.Unload()
		{
			On.Terraria.Player.SetTalkNPC -= ModifySetTalkNPC;
			On.Terraria.Main.GUIChatDrawInner -= DrawOverVanillaNPCChat;
		}

		// ...

		public void Activate()
		{
			Visible = true;

			scrollMax = 0;
			scrollValue = 0;

			Main.LocalPlayer.currentShoppingSettings.HappinessReport = string.Empty;
		}

		public void Deactivate(int npcIndex)
		{
			Main.LocalPlayer.SetTalkNPC(npcIndex, false);
			Main.npcChatText = Main.npc[npcIndex].GetChat();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (TextLinesInfo is null) return;

			var textDisplayCache = TextDisplayCacheInfo.GetValue(Main.instance);
			var lines = TextLinesInfo.GetValue(textDisplayCache) as List<List<TextSnippet>>;
			var linesCount = lines.Count;
			var emptyLinesCount = lines.Count(i => i.Any(j => j.Text.Equals(string.Empty)));

			var texture = TextureAssets.ChatBack;
			var distanceFromEdges = 16;
			var drawRect = new Rectangle(Main.screenWidth / 2 - texture.Width() / 2 + distanceFromEdges, 120 + (linesCount - emptyLinesCount) * 30, texture.Width() - distanceFromEdges * 2, emptyLinesCount * 30);
			var topLeft = drawRect.TopLeft();
			var bottomLeft = drawRect.BottomLeft();

			if (drawRect.Contains(Main.MouseScreen.ToPoint()))
			{
				Main.LocalPlayer.mouseInterface = true;
				UISystem.RequestIgnoreHotbarScroll();
			}

			DrawBackground(spriteBatch, drawRect);
			DrawContent(spriteBatch, drawRect);

			var lineOffset = new Vector2(-2f, 0);
			var lineWidth = drawRect.Width + 4;

			DrawShadows(spriteBatch, drawRect, topLeft, bottomLeft);
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

		private void DrawContent(SpriteBatch spriteBatch, Rectangle drawRect)
		{
			var drawPosition = new Vector2(drawRect.X, drawRect.Y);
			var cardSlotsCenter = drawPosition + new Vector2(cardSlotsTexture.Width() * 0.5f + 6, drawRect.Height * 0.5f);

			void DrawHorizontalLine(float xPos)
				=> spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)xPos, (int)drawPosition.Y, 4, drawRect.Height), Color.Black * 0.3f);

			spriteBatch.Draw(cardSlotsTexture.Value, cardSlotsCenter, null, Color.White, 0f, cardSlotsTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

			DrawHorizontalLine(drawRect.X + 4);
			DrawHorizontalLine(cardSlotsCenter.X + cardSlotsTexture.Width() * 0.5f - 2);
			DrawHorizontalLine(drawRect.X + drawRect.Width - 8);

			DrawCards(spriteBatch, cardSlotsCenter - cardSlotsTexture.Size() * 0.5f, out (Item, int, bool) hoverItemInfo);

			var sbInfo = new SpriteBatchInfo(spriteBatch);
			var oldScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(drawRect, spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
			sbInfo.Begin(spriteBatch, null, null, null, null, UISystem.OverflowHiddenRasterizerState, null, null);

			//DrawCardTooltips(spriteBatch, drawPosition);

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = oldScissorRectangle;
			sbInfo.Begin(spriteBatch);

			/*var topLeft = drawRect.TopLeft();
			string str = string.Empty;
			for (int i = 0; i < 12; i++) str += "Eula C2 WHEN???\n";

			var t = Utils.WordwrapStringSmart(str, Main.DiscoColor, FontAssets.MouseText.Value, 460, 10).ToArray();
			var offsetY = 0f;

			foreach (var elem in t)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, elem.ToArray(), topLeft + new Vector2(4, -4) + Vector2.UnitY * offsetY, 0f, Vector2.Zero, Vector2.One, out _, -1f, 2f);
				offsetY += 30;
			}*/
		}

		private void DrawCards(SpriteBatch spriteBatch, Vector2 cardSlotsPosition, out (Item hoverItem, int maxReq, bool canRemove) hoverItemInfo)
		{
			hoverItemInfo = (null, 0, false);

			var modPlayer = Main.LocalPlayer.GetModPlayer<OrchidGambler>();
			var maxReq = 0;
			var playerNbCards = modPlayer.GetNbGamblerCards();
			var nbCards = new int[playerNbCards];

			for (int i = 0; i < playerNbCards; i++)
			{
				ref var item = ref modPlayer.gamblerCardsItem[i];

				if (item.type.Equals(ItemID.None)) continue;

				var orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
				var cardReq = orchidItem.gamblerCardRequirement;

				nbCards[cardReq]++;
				maxReq = cardReq > maxReq ? cardReq : maxReq;
			}

			for (int i = 0; i < playerNbCards; i++)
			{
				ref var item = ref modPlayer.gamblerCardsItem[i];

				if (item.type.Equals(ItemID.None)) continue;

				var orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
				var cardReq = orchidItem.gamblerCardRequirement;
				var canRemove = (playerNbCards > maxReq + 1) || (cardReq == maxReq);

				var cardTexture = TextureAssets.Item[item.type].Value;
				var cardRect = new Rectangle((int)(cardSlotsPosition.X + 8 + (i % 5 * 26)), (int)(cardSlotsPosition.Y + 8 + (i == 0 ? 0 : i / 5) * 32), cardTexture.Width, cardTexture.Height);

				spriteBatch.Draw(cardTexture, cardRect, Color.White);

				if (!canRemove)
				{
					spriteBatch.Draw(cardBlockTexture.Value, new Rectangle(cardRect.X - 2, cardRect.Y - 2, cardBlockTexture.Width(), cardBlockTexture.Height()), Color.White);
				}

				if (!cardRect.Contains(Main.MouseScreen.ToPoint())) continue;

				UpdateScrollValues();

				//Rectangle tooltipRectangle = new Rectangle(drawZone.X + deckTexture.Width + 24, drawZone.Y + 14, Math.Abs(drawZone.Width - deckTexture.Width - 42), drawZone.Height - 28);
				//DrawCardInfo(item, spriteBatch, tooltipRectangle, GetCardInfo(item, maxReq, canRemove));

				if (PlayerInput.Triggers.JustReleased.MouseLeft || PlayerInput.Triggers.JustReleased.MouseRight)
				{
					OnCardClick(item, canRemove);

					hoverItemInfo = (item, maxReq, canRemove);
				}
			}
		}

		private void UpdateScrollValues()
		{
			int num = -PlayerInput.ScrollWheelDelta / 120;
			int sign = Math.Sign(num);
			int progress = 30;

			while (num != 0)
			{
				if (num < 0)
				{
					if (scrollValue >= progress) scrollValue -= progress;
				}
				else
				{
					if (scrollValue < (scrollMax - 5 * progress)) scrollValue += progress;
				}
				num -= sign;
			}
		}

		private void OnCardClick(Item item, bool canRemove)
		{
			if (!canRemove) return;

			var player = Main.LocalPlayer;
			var modPlayer = player.GetModPlayer<OrchidGambler>();

			player.QuickSpawnItem(null, item.type, 1);
			modPlayer.RemoveGamblerCard(item);

			if (modPlayer.GetNbGamblerCards() > 0)
			{
				modPlayer.ClearGamblerCardCurrent();
				modPlayer.ClearGamblerCardsNext();
				modPlayer.gamblerShuffleCooldown = 0;
				modPlayer.gamblerRedraws = 0;
				modPlayer.DrawGamblerCard();
			}
			else
			{
				modPlayer.OnRespawn(player);
			}
		}

		private void DrawCardTooltips(SpriteBatch spriteBatch, Vector2 drawPosition, Item hoverItem)
		{

		}

		private void DrawBackground(SpriteBatch spriteBatch, Rectangle drawRect)
		{
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawRect, Color.Black * 0.3f);
		}

		private void DrawShadows(SpriteBatch spriteBatch, Rectangle drawRect, Vector2 topLeft, Vector2 bottomLeft)
		{
			spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)drawRect.Width, shadingTexture.Height()), Color.White);
			spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)bottomLeft.X, (int)bottomLeft.Y - shadingTexture.Height(), (int)drawRect.Width, shadingTexture.Height()), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
		}

		private void DrawWhiteLine(SpriteBatch spriteBatch, Vector2 position, float width)
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

		// ...

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

		private static readonly FieldInfo TextDisplayCacheInfo = typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance);
		private static readonly PropertyInfo TextLinesInfo = TextDisplayCacheInfo?.GetValue(Main.instance).GetType().GetProperty("TextLines", BindingFlags.Public | BindingFlags.Instance);
	}
}