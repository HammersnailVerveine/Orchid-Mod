using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.UIs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
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

		public static readonly HashSet<string> TooltipsWhitelist = new() { "Damage", "CritChance", "Speed", "Knockback", "TagsTag" };
		public static readonly float FontScale = 0.8f;

		public bool Visible;

		private Asset<Texture2D> cardBlockTexture;
		private Asset<Texture2D> cardSlotsTexture;
		private Asset<Texture2D> shadingTexture;

		private Rectangle drawRect;
		private int linesCount;
		private int emptyLinesCount;
		private int scrollValue;
		private int scrollMax;

		// ...

		void ILoadable.Load(Mod mod)
		{
			cardBlockTexture = ModContent.Request<Texture2D>(OrchidAssets.UIsPath + "CroupierUICardBlock", AssetRequestMode.ImmediateLoad);
			cardSlotsTexture = ModContent.Request<Texture2D>(OrchidAssets.UIsPath + "CroupierUICardSlots", AssetRequestMode.ImmediateLoad);
			shadingTexture = OrchidAssets.GetExtraTexture(18, AssetRequestMode.ImmediateLoad);

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

			linesCount = 0;
			emptyLinesCount = 0;
			scrollMax = 0;
			scrollValue = 0;

			Main.LocalPlayer.currentShoppingSettings.HappinessReport = string.Empty;
		}

		public void Deactivate(int npcIndex)
		{
			Main.LocalPlayer.SetTalkNPC(npcIndex, false);
			Main.npcChatText = Main.npc[npcIndex].GetChat();
		}

		private void Draw(SpriteBatch spriteBatch)
		{
			if (TextLinesInfo is null) return;

			var textDisplayCache = TextDisplayCacheInfo.GetValue(Main.instance);
			var lines = TextLinesInfo.GetValue(textDisplayCache) as List<List<TextSnippet>>;

			linesCount = lines.Count;
			emptyLinesCount = lines.Count(i => i.Any(j => j.Text.Equals(string.Empty)));

			var texture = TextureAssets.ChatBack;
			var distanceFromEdges = 16;

			drawRect = new Rectangle(Main.screenWidth / 2 - texture.Width() / 2 + distanceFromEdges, 120 + (linesCount - emptyLinesCount) * 30, texture.Width() - distanceFromEdges * 2, emptyLinesCount * 30);

			var whiteLineOffset = new Vector2(-2f, 0);
			var whiteLineWidth = drawRect.Width + 4;

			if (drawRect.Contains(Main.MouseScreen.ToPoint()))
			{
				Main.LocalPlayer.mouseInterface = true;
				UISystem.RequestIgnoreHotbarScroll();
			}

			// ...

			DrawBackground(spriteBatch);

			DrawHorizontalLine(spriteBatch, 4);
			DrawHorizontalLine(spriteBatch, 16 + cardSlotsTexture.Width());
			DrawHorizontalLine(spriteBatch, drawRect.Width - 8);

			DrawCardsWithSlots(spriteBatch, out Item hoverItem, out int maxReq, out bool canRemoveHoverCard);
			DrawCardTooltips(spriteBatch, hoverItem, maxReq, canRemoveHoverCard);

			DrawShadows(spriteBatch);

			DrawWhiteLine(spriteBatch, drawRect.TopLeft() + whiteLineOffset, whiteLineWidth);
			DrawWhiteLine(spriteBatch, drawRect.BottomLeft() + whiteLineOffset, whiteLineWidth);
		}

		private void DrawBackground(SpriteBatch spriteBatch)
			=> spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawRect, Color.Black * 0.3f);

		private void DrawHorizontalLine(SpriteBatch spriteBatch, int offsetX)
			=> spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(drawRect.X + offsetX, drawRect.Y, 4, drawRect.Height), Color.Black * 0.3f);

		private void DrawCardsWithSlots(SpriteBatch spriteBatch, out Item hoverItem, out int maxReq, out bool canRemoveHoverCard)
		{
			hoverItem = null;
			maxReq = 0;
			canRemoveHoverCard = true;

			var cardSlotsPosition = new Vector2(drawRect.X, drawRect.Y) + new Vector2(12, drawRect.Height * 0.5f - cardSlotsTexture.Height() * 0.5f);
			var modPlayer = Main.LocalPlayer.GetModPlayer<OrchidGambler>();
			var playerNbCards = modPlayer.GetNbGamblerCards();
			var nbCards = new int[20];

			spriteBatch.Draw(cardSlotsTexture.Value, cardSlotsPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

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
				var item = modPlayer.gamblerCardsItem[i];

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

				if (cardRect.Contains(Main.MouseScreen.ToPoint()))
				{
					OnHoverCard(item, canRemove);

					hoverItem = item;
					canRemoveHoverCard = canRemove;
				}
			}
		}

		private void OnHoverCard(Item item, bool canRemoveHoverCard)
		{
			int num = -PlayerInput.ScrollWheelDelta / 120;
			int sign = Math.Sign(num);
			int progress = (int)(30 * FontScale);

			while (num != 0)
			{
				scrollValue += sign * progress;
				scrollValue = Math.Clamp(scrollValue, 0, scrollMax);

				num -= sign;
			}

			// ...

			if (canRemoveHoverCard && (PlayerInput.Triggers.JustReleased.MouseLeft || PlayerInput.Triggers.JustReleased.MouseRight))
			{
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
		}

		private void DrawCardTooltips(SpriteBatch spriteBatch, Item hoverItem, int maxReq, bool canRemove)
		{
			scrollMax = 0;

			var tooltipsPosX = (int)(drawRect.X + 20 + cardSlotsTexture.Width());
			var tooltipsRect = new Rectangle(tooltipsPosX, drawRect.Y, drawRect.Width - (tooltipsPosX - drawRect.X) - 8, drawRect.Height);

			var sbInfo = new SpriteBatchInfo(spriteBatch);
			var oldScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(tooltipsRect, spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
			sbInfo.Begin(spriteBatch, null, null, null, null, UISystem.OverflowHiddenRasterizerState, null, null);

			List<TooltipLine> tooltips;

			if (hoverItem is null)
			{
				scrollValue = 0;
				scrollMax = 0;

				tooltips = new();
				tooltips.Add(new TooltipLine(OrchidMod.Instance, "Eula C6 WHEN???", "..."));
			}
			else
			{
				int yoyoLogo = 0, researchLine = 0, numLines = 0;
				string[] toolTipLine = new string[30], tooltipNames = new string[30];
				bool[] preFixLine = new bool[30], badPreFixLine = new bool[30];
				var triangleHexColor = Colors.AlphaDarken(Color.White).Hex3();

				Main.MouseText_DrawItemTooltip_GetLinesInfo(hoverItem, ref yoyoLogo, ref researchLine, hoverItem.knockBack, ref numLines, toolTipLine, preFixLine, badPreFixLine, tooltipNames);

				tooltips = ItemLoader.ModifyTooltips(hoverItem, ref numLines, tooltipNames, ref toolTipLine, ref preFixLine, ref badPreFixLine, ref yoyoLogo, out _);
				tooltips = tooltips.Where(i => TooltipsWhitelist.Contains(i.Name) || i.Name.StartsWith("Tooltip") || i.Name.StartsWith("Prefix")).ToList();

				tooltips.Insert(0, new TooltipLine(OrchidMod.Instance, "ItemName", hoverItem.HoverName.Replace("Playing Card : ", "")) { OverrideColor = ItemRarity.GetColor(hoverItem.rare) });
				tooltips.Insert(1, new TooltipLine(OrchidMod.Instance, "CanRemove",
					canRemove ?
					$"[c/{triangleHexColor}:‣] Can be removed" :
					$"[c/{triangleHexColor}:‣] Cannot be removed\n" +
					$"[c/{triangleHexColor}:‣] Most expensive card in deck: {maxReq}"
				)
				{ OverrideColor = new Color(250, 150, 100) });
			}

			var counter = -1;

			for (int i = 0; i < tooltips.Count; i++)
			{
				var text = Utils.WordwrapStringSmart(tooltips[i].Text, tooltips[i].OverrideColor ?? Color.White, FontAssets.MouseText.Value, (int)((drawRect.Width - 6) * FontScale), 30);

				foreach (var elem in text)
				{
					var drawPosition = new Vector2(tooltipsRect.X + 6, (float)(tooltipsRect.Y + 30 * ++counter * FontScale - scrollValue) + 10);

					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, elem.ToArray(), drawPosition, 0f, Vector2.Zero, Vector2.One * FontScale, out _, -1f, 2f);
				}
			}

			scrollMax = (int)(Math.Max(counter - emptyLinesCount, 0) * 30 * FontScale);

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = oldScissorRectangle;
			sbInfo.Begin(spriteBatch);
		}

		private void DrawShadows(SpriteBatch spriteBatch)
		{
			var topLeft = drawRect.TopLeft();
			var bottomLeft = drawRect.BottomLeft();

			spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)topLeft.X, (int)topLeft.Y, drawRect.Width, shadingTexture.Height()), Color.White);
			spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)bottomLeft.X, (int)bottomLeft.Y - shadingTexture.Height(), drawRect.Width, shadingTexture.Height()), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
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