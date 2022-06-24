using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.UIs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OrchidMod.Gambler.UI
{
	// I'm sure it can be written much better

	public class CroupierUI : OrchidUIState
	{
		public Texture2D backgroundTexture;
		public Texture2D borderTexture;
		public Texture2D deckTexture;
		public Texture2D deckBlockTexture;

		public RasterizerState rasterizerState;

		public Rectangle drawZone;

		public const int fontScale = 8;

		public int linesCount = -1;
		public int emptyLinesCount = -1;
		public int scrollValue = 0;
		public int scrollMax = 0;
		public int hoverCardType = -1;

		//public bool Visible { get; set; }
		public int FontOffsetY => (int)(30 * (fontScale / 10f));

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			backgroundTexture = ModContent.Request<Texture2D>("OrchidMod/Gambler/UI/Textures/CroupierGUIBackground", AssetRequestMode.ImmediateLoad).Value;
			borderTexture = ModContent.Request<Texture2D>("OrchidMod/Gambler/UI/Textures/CroupierGUIBorder", AssetRequestMode.ImmediateLoad).Value;
			deckTexture = ModContent.Request<Texture2D>("OrchidMod/Gambler/UI/Textures/CroupierGUIDeck", AssetRequestMode.ImmediateLoad).Value;
			deckBlockTexture = ModContent.Request<Texture2D>("OrchidMod/Gambler/UI/Textures/DeckUIBlock", AssetRequestMode.ImmediateLoad).Value;

			rasterizerState = new RasterizerState
			{
				CullMode = CullMode.None,
				ScissorTestEnable = true
			};
		}

		public void Update()
		{
			// drawZone = new Rectangle(Main.screenWidth / 2 - Main.chatBackTexture.Width / 2 + 2, 120 + (linesCount - emptyLinesCount) * 30, Main.chatBackTexture.Width - 4, emptyLinesCount * 30); [S¨]
			drawZone = new Rectangle(Main.screenWidth / 2 - 50 + 2, 120 + (linesCount - emptyLinesCount) * 30, 100 - 4, emptyLinesCount * 30);

			if (drawZone.Contains(Main.MouseScreen.ToPoint())) Main.LocalPlayer.GetModPlayer<OrchidPlayer>().ignoreScrollHotbar = true;
		}

		public void UpdateOnChatButtonClicked()
		{
			List<List<TextSnippet>> list = Utils.WordwrapStringSmart(Main.npcChatText, Color.White, FontAssets.MouseText.Value, 460, 10);

			linesCount = list.Count;
			emptyLinesCount = list.FindAll(i => i.Contains(i.Find(j => j.Text == ""))).Count;
		}

		public void OnCardClick(Item item, Player player, OrchidGambler orchidPlayer)
		{
			player.QuickSpawnItem(null, item.type, 1);
			orchidPlayer.RemoveGamblerCard(item);

			if (orchidPlayer.GetNbGamblerCards() > 0)
			{
				orchidPlayer.ClearGamblerCardCurrent();
				orchidPlayer.ClearGamblerCardsNext();
				orchidPlayer.gamblerShuffleCooldown = 0;
				orchidPlayer.gamblerRedraws = 0;
				orchidPlayer.DrawGamblerCard();
			}
			else
			{
				orchidPlayer.OnRespawn(player);
			}
		}

		public string GetCardInfo(Item item, int maxReq, bool canRemove)
		{
			string hmm = $"[c/{Color.Gray.Hex3()}: | ]";

			var player = Main.player[Main.myPlayer];
			var gamblerPlayer = player.GetModPlayer<OrchidGambler>();
			var gamblerItem = item.ModItem as OrchidModGamblerItem;

			string knockbackText;
			{
				float num = player.GetWeaponKnockback(item, item.knockBack);
				if (num == 0f) knockbackText = Language.GetTextValue("LegacyTooltip.14");
				else if (num <= 1.5) knockbackText = Language.GetTextValue("LegacyTooltip.15");
				else if (num <= 3f) knockbackText = Language.GetTextValue("LegacyTooltip.16");
				else if (num <= 4f) knockbackText = Language.GetTextValue("LegacyTooltip.17");
				else if (num <= 6f) knockbackText = Language.GetTextValue("LegacyTooltip.18");
				else if (num <= 7f) knockbackText = Language.GetTextValue("LegacyTooltip.19");
				else if (num <= 9f) knockbackText = Language.GetTextValue("LegacyTooltip.20");
				else if (num <= 11f) knockbackText = Language.GetTextValue("LegacyTooltip.21");
				else knockbackText = Language.GetTextValue("LegacyTooltip.22");
			}

			// Card Name
			string cardTooltipText = item.HoverName.Replace("Playing Card : ", "") + "\n";

			// Can be removed / Cannot be removed
			cardTooltipText += (canRemove ? "Can be removed" : "Cannot be removed\nMost expensive card in deck: " + $"[c/{new Color(215, 65, 65).Hex3()}:{maxReq}]") + "\n";

			// Damage + Crit
			cardTooltipText += $"Damage: {player.GetWeaponDamage(item)}" + hmm +
				$"Crit: {item.crit + gamblerPlayer.gamblerCrit}%" + "\n"; // TODO ... [CRIT]

			// Knockback
			cardTooltipText += knockbackText + "\n";

			// Set + Required Cards
			{
				int tagCount = gamblerItem.gamblerCardSets.Count - 1;
				if (tagCount > -1)
				{
					string text = "";
					List<string> alreadyDone = new List<string>();
					foreach (string tag in gamblerItem.gamblerCardSets)
					{
						if (!alreadyDone.Contains(tag))
						{
							text += alreadyDone.Count > 0 ? ", " : "";
							text += tag;
							tagCount--;
							alreadyDone.Add(tag);
						}
					}
					cardTooltipText += (alreadyDone.Count > 1 ? "Sets: " : "Set: ") + $"[c/{new Color(175, 255, 175).Hex3()}:{text}]";
				}
				else cardTooltipText += "Set: -";

				cardTooltipText += hmm + "Required cards: " + $"[c/{new Color(255, 200, 100).Hex3()}:{item.GetGlobalItem<OrchidModGlobalItem>().gamblerCardRequirement}]" + "\n";
			}

			/*// Tooltips
			{
				for (int j = 0; j < item.ToolTip.Lines; j++) cardTooltipText += item.ToolTip.GetLine(j) + "\n";
			}*/

			return cardTooltipText;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			/* [SP]
			Vector2 deckPosition = new Vector2(drawZone.X + 12, drawZone.Y + drawZone.Height * 0.5f - deckTexture.Height * 0.5f);

			// Background
			{
				Color color = new Color(25, 25, 45, 160);
				DrawSeparation(spriteBatch, new Vector2(drawZone.X + 6, drawZone.Y), 6, color);
				DrawSeparation(spriteBatch, new Vector2(drawZone.X + deckTexture.Width + 12, drawZone.Y), 6, color);
				DrawSeparation(spriteBatch, new Vector2(drawZone.X + drawZone.Width - 12, drawZone.Y), 6, color);

				DrawPanel(spriteBatch, backgroundTexture, new Color(25, 25, 60, 140));
				DrawPanel(spriteBatch, borderTexture, new Color(25, 25, 45, 160));

				spriteBatch.Draw(deckTexture, new Vector2(deckPosition.X, deckPosition.Y) + deckTexture.Size() * 0.5f, null, Color.White, 0f, deckTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			}

			// Cards and Info
			{
				Player player = Main.player[Main.myPlayer];
				OrchidPlayer orchidPlayer = player.GetModPlayer<OrchidPlayer>();

				if (!player.dead)
				{
					int[] nbCards = new int[20];
					for (int i = 0; i < 20; i++) nbCards[i] = 0;

					int maxReq = 0;
					int playerNbCards = OrchidModGamblerHelper.getNbGamblerCards(player, orchidPlayer);

					for (int i = 0; i < playerNbCards; i++)
					{
						Item currentItem = orchidPlayer.gamblerCardsItem[i];
						if (currentItem.type != ItemID.None)
						{
							OrchidModGlobalItem orchidItem = currentItem.GetGlobalItem<OrchidModGlobalItem>();
							int cardReq = orchidItem.gamblerCardRequirement;
							nbCards[cardReq]++;
							maxReq = cardReq > maxReq ? cardReq : maxReq;
						}
					}

					for (int i = 0; i < playerNbCards; i++)
					{
						Item currentItem = orchidPlayer.gamblerCardsItem[i];
						if (currentItem.type != ItemID.None)
						{
							OrchidModGlobalItem orchidItem = currentItem.GetGlobalItem<OrchidModGlobalItem>();
							int cardReq = orchidItem.gamblerCardRequirement;
							bool canRemove = (playerNbCards > maxReq + 1) || (cardReq == maxReq);

							DrawCard(currentItem, spriteBatch, new Vector2(deckPosition.X + 8 + (i % 5 * 26), deckPosition.Y + 8 + (i == 0 ? 0 : i / 5) * 32), player, orchidPlayer, maxReq, canRemove);
						}
					}
				}
			}
			*/
		}

		public void DrawCard(Item item, SpriteBatch spriteBatch, Vector2 position, Player player, OrchidGambler orchidPlayer, int maxReq, bool canRemove = true)
		{
			if (item == null || item.type < ItemID.None) return;

			var cardTexture = TextureAssets.Item[item.type].Value;
			var cardRect = new Rectangle((int)position.X, (int)position.Y, cardTexture.Width, cardTexture.Height);

			spriteBatch.Draw(cardTexture, cardRect, Color.White);
			if (!canRemove) spriteBatch.Draw(deckBlockTexture, new Rectangle(cardRect.X - 2, cardRect.Y - 2, deckBlockTexture.Width, deckBlockTexture.Height), Color.White);

			if (cardRect.Contains(Main.MouseScreen.ToPoint()))
			{
				player.mouseInterface = true;

				if (hoverCardType != item.type)
				{
					hoverCardType = item.type;
					scrollValue = 0;
				}

				// Scrolling
				{
					int num = -PlayerInput.ScrollWheelDelta / 120;
					int sign = Math.Sign(num);
					int progr = FontOffsetY;

					while (num != 0)
					{
						if (num < 0)
						{
							if (scrollValue >= progr) scrollValue -= progr;
						}
						else
						{
							if (scrollValue < (scrollMax - 5 * progr)) scrollValue += progr;
						}
						num -= sign;
					}
				}

				Rectangle tooltipRectangle = new Rectangle(drawZone.X + deckTexture.Width + 24, drawZone.Y + 14, Math.Abs(drawZone.Width - deckTexture.Width - 42), drawZone.Height - 28);
				DrawCardInfo(item, spriteBatch, tooltipRectangle, GetCardInfo(item, maxReq, canRemove));

				if ((Main.mouseLeft && Main.mouseLeftRelease || Main.mouseRight && Main.mouseRightRelease) && canRemove) OnCardClick(item, player, orchidPlayer);
			}
		}

		public void DrawCardInfo(Item item, SpriteBatch spriteBatch, Rectangle rect, string text)
		{
			if (item == null || item.type < ItemID.None) return;

			List<List<TextSnippet>> list = Utils.WordwrapStringSmart(text, Color.White, FontAssets.MouseText.Value, (int)(rect.Width * 1.22f), fontScale);
			foreach (var elem in list[0]) elem.Color = ItemRarity.GetColor(item.rare);

			int offsetY = FontOffsetY;
			scrollMax = (list.Count - 1) * offsetY;
			Color backgroundColor = new Color(25, 25, 45, 160) * 0.75f;

			Rectangle nameRectangle = new Rectangle(rect.X - 2, rect.Y - 2, rect.Width + 4, offsetY + 4);
			Rectangle tooltipRectangle = new Rectangle(nameRectangle.X, nameRectangle.Y + offsetY + 6, nameRectangle.Width + 1, rect.Height + 4 - offsetY - 6);

			// Card Name
			{
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, nameRectangle, backgroundColor);
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, list[0].ToArray(), new Vector2(nameRectangle.X + 4, nameRectangle.Y + 6), 0f, Vector2.Zero, Vector2.One * (fontScale / 10f), out _, -1f, 2f);
			}

			Rectangle oldScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(tooltipRectangle, spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);

			// Info
			{
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, tooltipRectangle, backgroundColor);
				for (int i = 0; i < list.Count - 1; i++)
				{
					Vector2 drawPosition = new Vector2(tooltipRectangle.X + 4, (float)(tooltipRectangle.Y + offsetY * i - scrollValue) + 4);
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, list[i + 1].ToArray(), drawPosition, 0f, Vector2.Zero, Vector2.One * (fontScale / 10f), out _, -1f, 2f);
				}
			}

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = oldScissorRectangle;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, spriteBatch.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
		}

		private void DrawSeparation(SpriteBatch spriteBatch, Vector2 position, int width, Color color)
		{
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X, (int)position.Y, width, drawZone.Height), color);
		}

		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			const int CORNER_SIZE = 12;
			const int BAR_SIZE = 4;

			Point point = new Point(drawZone.X, drawZone.Y);
			Point point2 = new Point(point.X + drawZone.Width - CORNER_SIZE, point.Y + drawZone.Height - CORNER_SIZE);
			int width = point2.X - point.X - CORNER_SIZE;
			int height = point2.Y - point.Y - CORNER_SIZE;

			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, 0, CORNER_SIZE, CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, 0, BAR_SIZE, CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point2.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE + BAR_SIZE, BAR_SIZE, CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(0, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y + CORNER_SIZE, width, height), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE, BAR_SIZE, BAR_SIZE)), color);
		}

		private Rectangle GetClippingRectangle(Rectangle rect, SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((int)rect.X, (int)rect.Y);
			Vector2 vector2 = new Vector2((int)rect.Width, (int)rect.Height) + vector;
			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			vector2 = Vector2.Transform(vector2, Main.UIScaleMatrix);
			Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, (int)(vector2.X - vector.X), (int)(vector2.Y - vector.Y));
			int width = spriteBatch.GraphicsDevice.Viewport.Width;
			int height = spriteBatch.GraphicsDevice.Viewport.Height;
			rectangle.X = Utils.Clamp<int>(rectangle.X, 0, width);
			rectangle.Y = Utils.Clamp<int>(rectangle.Y, 0, height);
			rectangle.Width = Utils.Clamp<int>(rectangle.Width, 0, width - rectangle.X);
			rectangle.Height = Utils.Clamp<int>(rectangle.Height, 0, height - rectangle.Y);
			return rectangle;
		}
	}
}