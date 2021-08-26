using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OrchidMod.Gambler.UI
{
	public class GamblerUIFrame : UIElement
	{
		public Color backgroundColor = Color.White;
		public static Texture2D ressourceBar;
		public static Texture2D ressourceBarFull;
		public static Texture2D ressourceBarDiceFull;
		public static Texture2D ressourceBarTop;
		public static Texture2D ressourceBarDiceTop;

		public static Texture2D chip1;
		public static Texture2D chip2;
		public static Texture2D chip3;
		public static Texture2D chip4;
		public static Texture2D chip5;

		public static Texture2D dice1;
		public static Texture2D dice2;
		public static Texture2D dice3;
		public static Texture2D dice4;
		public static Texture2D dice5;
		public static Texture2D dice6;

		public static Texture2D UIDeck;
		public static Texture2D UICard;
		public static Texture2D UIRedraw;

		public static Texture2D UICardNext1;
		public static Texture2D UICardNext2;
		public static Texture2D UICardNext3;

		public static Texture2D UIDeckbuilding;
		public static Texture2D UIDeckbuildingBlock;

		public int cardID = -1;
		public int cardIDNext1 = -1;
		public int cardIDNext2 = -1;
		public int cardIDNext3 = -1;

		Player player = Main.player[Main.myPlayer];

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2((float)((int)(Main.LocalPlayer.position.X - Main.screenPosition.X) - Main.GameViewMatrix.Translation.X - (float)(Main.LocalPlayer.bodyFrame.Width / 2) + (float)(Main.LocalPlayer.width / 2)), (float)((int)(Main.LocalPlayer.position.Y - Main.screenPosition.Y) - Main.GameViewMatrix.Translation.Y + (float)Main.LocalPlayer.height - (float)Main.LocalPlayer.bodyFrame.Height + 12f)) + Main.LocalPlayer.bodyPosition + new Vector2((float)(Main.LocalPlayer.bodyFrame.Width / 2));
			vector *= Main.GameViewMatrix.Zoom;
			vector /= Main.UIScale;

			this.Left.Set(vector.X + 30f, 0f);
			this.Top.Set(vector.Y - 50f, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y - 100);
			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (!player.dead)
			{
				if (modPlayer.gamblerUIDeckDisplay)
				{
					if (player.talkNPC != -1)
					{
						spriteBatch.Draw(UIDeckbuilding, new Rectangle(point.X - 100, point.Y - 60, 140, 138), backgroundColor);
						int offSetX = 0;
						int offSetY = 0;
						string msg = "";
						string msg2 = "";
						string msg3 = "";
						int[] nbCards = new int[20];
						int maxReq = 0;
						int playerNbCards = OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer);
						for (int i = 0; i < 20; i++)
						{
							nbCards[i] = 0;
						}
						for (int i = 0; i < playerNbCards; i++)
						{
							Item currentItem = modPlayer.gamblerCardsItem[i];
							Texture2D currentTexture = Main.itemTexture[currentItem.type];
							Rectangle currentRectangle = new Rectangle(point.X - 92 + offSetX, point.Y - 52 + offSetY, 20, 26);
							spriteBatch.Draw(currentTexture, currentRectangle, backgroundColor);
							offSetX += 26;
							if ((i - 4) % 5 == 0)
							{
								offSetX = 0;
								offSetY += 32;
							}
							if (currentItem.type != 0)
							{
								OrchidModGlobalItem orchidItem = currentItem.GetGlobalItem<OrchidModGlobalItem>();
								int cardReq = orchidItem.gamblerCardRequirement;
								nbCards[cardReq]++;
								maxReq = cardReq > maxReq ? cardReq : maxReq;
							}
						}

						offSetX = 0;
						offSetY = 0;

						for (int i = 0; i < playerNbCards; i++)
						{
							Item currentItem = modPlayer.gamblerCardsItem[i];
							Rectangle currentRectangle = new Rectangle(point.X - 92 + offSetX, point.Y - 52 + offSetY, 20, 26);
							offSetX += 26;
							if ((i - 4) % 5 == 0)
							{
								offSetX = 0;
								offSetY += 32;
							}
							if (currentItem.type != 0)
							{
								OrchidModGlobalItem orchidItem = currentItem.GetGlobalItem<OrchidModGlobalItem>();
								int cardReq = orchidItem.gamblerCardRequirement;
								bool canRemove = (playerNbCards > maxReq + 1) || (cardReq == maxReq);
								if (currentRectangle.Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y)))
								{
									if (((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease)) && canRemove)
									{
										player.QuickSpawnItem(currentItem.type, 1);
										OrchidModGamblerHelper.removeGamblerCard(currentItem, player, modPlayer);
										if (OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) > 0)
										{
											OrchidModGamblerHelper.clearGamblerCardCurrent(player, modPlayer);
											OrchidModGamblerHelper.clearGamblerCardsNext(player, modPlayer);
											modPlayer.gamblerShuffleCooldown = 0;
											modPlayer.gamblerRedraws = 0;
											OrchidModGamblerHelper.drawGamblerCard(player, modPlayer);
										}
										else
										{
											OrchidModGamblerHelper.onRespawnGambler(player, modPlayer);
										}
									}
									msg = currentItem.Name;
									msg2 = canRemove ? "Can be removed" : "Cannot be removed";
									msg3 = canRemove ? "" : "Your highest cost card requires at least " + maxReq + " cards";
								}
								if (!canRemove)
								{
									Rectangle blockRectangle = new Rectangle(currentRectangle.X - 2, currentRectangle.Y - 2, currentRectangle.Width + 4, currentRectangle.Height + 4);
									spriteBatch.Draw(UIDeckbuildingBlock, blockRectangle, backgroundColor);
								}
							}
						}
						if (msg != "")
						{
							ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, Main.MouseScreen + new Vector2(15f, 15f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
						}
						if (msg2 != "")
						{
							ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg2, Main.MouseScreen + new Vector2(15f, 40f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
						}
						if (msg3 != "")
						{
							ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg2, Main.MouseScreen + new Vector2(15f, 40f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
						}
					}
					else
					{
						modPlayer.gamblerUIDeckDisplay = false;
					}
				}

				if ((modPlayer.gamblerUIDisplayTimer > 0 || modPlayer.gamblerChips > 0) && modPlayer.gamblerUIFightDisplay)
				{

					if (modPlayer.gamblerCardCurrent.type != this.cardID)
					{
						UICard = this.getUiCardTexture(modPlayer, 0);
						this.cardID = modPlayer.gamblerCardCurrent.type;
					}

					if (modPlayer.gamblerCardNext[0].type != this.cardIDNext1)
					{
						UICardNext1 = this.getUiCardTexture(modPlayer, 1);
						this.cardIDNext1 = modPlayer.gamblerCardNext[0].type;
					}

					if (modPlayer.gamblerCardNext[1].type != this.cardIDNext2)
					{
						UICardNext2 = this.getUiCardTexture(modPlayer, 2);
						this.cardIDNext2 = modPlayer.gamblerCardNext[1].type;
					}

					if (modPlayer.gamblerCardNext[2].type != this.cardIDNext3)
					{
						UICardNext3 = this.getUiCardTexture(modPlayer, 3);
						this.cardIDNext3 = modPlayer.gamblerCardNext[2].type;
					}

					int drawHeight = 70;
					int drawSide = 0;
					int leftOffset = 0;
					int leftOffset2 = 0;
					Texture2D chip = chip1;

					spriteBatch.Draw(UIDeck, new Rectangle(point.X + 8, point.Y + 160, 26, 32), backgroundColor);
					spriteBatch.Draw(UICard, new Rectangle(point.X + 8, point.Y + 160, 20, 26), backgroundColor);

					if (modPlayer.gamblerSeeCards > 2) spriteBatch.Draw(UICardNext3, new Rectangle(point.X + 40, point.Y + 194, 20, 26), backgroundColor);
					if (modPlayer.gamblerSeeCards > 1) spriteBatch.Draw(UICardNext2, new Rectangle(point.X + 24, point.Y + 194, 20, 26), backgroundColor);
					if (modPlayer.gamblerSeeCards > 0) spriteBatch.Draw(UICardNext1, new Rectangle(point.X + 8, point.Y + 194, 20, 26), backgroundColor);

					for (int i = 0; i < modPlayer.gamblerRedraws; i++)
					{
						spriteBatch.Draw(UIRedraw, new Rectangle(point.X + 20 - drawSide, point.Y + 94, 14, 20), backgroundColor);
						drawSide += 4;
					}

					int unit = (int)(modPlayer.gamblerShuffleCooldownMax / 34) + 1;
					int val = 0;
					spriteBatch.Draw(ressourceBar, new Rectangle(point.X + 36, point.Y + 116, 12, 76), backgroundColor);
					while (val < modPlayer.gamblerShuffleCooldown)
					{
						spriteBatch.Draw(ressourceBarFull, new Rectangle(point.X + 40, point.Y + 116 + drawHeight, 4, 2), backgroundColor);
						drawHeight -= 2;
						val += unit;
					}

					if (val > 0)
					{
						drawHeight += 2;
						spriteBatch.Draw(ressourceBarTop, new Rectangle(point.X + 40, point.Y + 116 + drawHeight, 4, 2), backgroundColor);
					}

					if (modPlayer.gamblerDiceValue > 0)
					{
						Texture2D dice = dice1;
						switch (modPlayer.gamblerDiceValue)
						{
							case 1:
								dice = dice1;
								break;
							case 2:
								dice = dice2;
								break;
							case 3:
								dice = dice3;
								break;
							case 4:
								dice = dice4;
								break;
							case 5:
								dice = dice5;
								break;
							case 6:
								dice = dice6;
								break;
							default:
								break;
						}
						spriteBatch.Draw(dice, new Rectangle(point.X + 36, point.Y + 90, 24, 24), backgroundColor);

						drawHeight = 70;
						val = 0;

						spriteBatch.Draw(ressourceBar, new Rectangle(point.X + 50, point.Y + 116, 12, 76), backgroundColor);
						while (val < modPlayer.gamblerDiceDuration)
						{
							spriteBatch.Draw(ressourceBarDiceFull, new Rectangle(point.X + 54, point.Y + 116 + drawHeight, 4, 2), backgroundColor);
							drawHeight -= 2;
							val += 60;
						}

						if (val > 0)
						{
							spriteBatch.Draw(ressourceBarDiceTop, new Rectangle(point.X + 54, point.Y + 116 + drawHeight, 4, 2), backgroundColor);
						}
					}

					drawHeight = 36;
					for (int i = 1; i < modPlayer.gamblerChips + 1; i++)
					{
						spriteBatch.Draw(chip, new Rectangle(point.X + 18 - leftOffset - leftOffset2, point.Y + 116 + drawHeight, 14, 6), backgroundColor);

						drawHeight -= 4;
						if (i % 10 == 0)
						{
							drawHeight = 36;
							leftOffset2 = leftOffset2 == 4 ? 0 : 4;
							switch (i)
							{
								case 10:
									chip = chip2;
									break;
								case 20:
									chip = chip3;
									break;
								case 30:
									chip = chip4;
									break;
								case 40:
									chip = chip5;
									break;
								default:
									break;
							}
						}
						leftOffset = leftOffset == 2 ? 0 : 2;
					}
				}
			}
		}

		public Texture2D getUiCardTexture(OrchidModPlayer modPlayer, int cardNb)
		{
			Texture2D cardTexture;
			Item card = new Item();
			if (modPlayer.gamblerCardCurrent.type != 0)
			{
				switch (cardNb)
				{
					case 0:
						card = modPlayer.gamblerCardCurrent;
						break;
					case 1:
						card = modPlayer.gamblerCardNext[0];
						break;
					case 2:
						card = modPlayer.gamblerCardNext[1];
						break;
					case 3:
						card = modPlayer.gamblerCardNext[2];
						break;
					default:
						cardTexture = UIRedraw;
						return cardTexture;
				}
				cardTexture = Main.itemTexture[card.type];
			}
			else
			{
				cardTexture = UIRedraw;
			}
			return cardTexture;
		}

		public GamblerUIFrame()
		{
			if (ressourceBar == null) ressourceBar = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/GamblerUIBar");
			if (ressourceBarFull == null) ressourceBarFull = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/GamblerUIBarFilled");
			if (ressourceBarTop == null) ressourceBarTop = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/GamblerUIBarTop");
			if (ressourceBarDiceFull == null) ressourceBarDiceFull = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/GamblerUIBarDiceFilled");
			if (ressourceBarDiceTop == null) ressourceBarDiceTop = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/GamblerUIBarDiceTop");

			if (chip1 == null) chip1 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIChip1");
			if (chip2 == null) chip2 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIChip2");
			if (chip3 == null) chip3 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIChip3");
			if (chip4 == null) chip4 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIChip4");
			if (chip5 == null) chip5 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIChip5");

			if (dice1 == null) dice1 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDice1");
			if (dice2 == null) dice2 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDice2");
			if (dice3 == null) dice3 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDice3");
			if (dice4 == null) dice4 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDice4");
			if (dice5 == null) dice5 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDice5");
			if (dice6 == null) dice6 = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDice6");

			if (UIDeck == null) UIDeck = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/UIDeck");
			if (UIRedraw == null) UIRedraw = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/GamblerUIRedraw");
			if (UICard == null) UICard = ModContent.GetTexture("OrchidMod/Gambler/GamblerReset");
			if (UICardNext1 == null) UICardNext1 = ModContent.GetTexture("OrchidMod/Gambler/GamblerReset");
			if (UICardNext2 == null) UICardNext2 = ModContent.GetTexture("OrchidMod/Gambler/GamblerReset");
			if (UICardNext3 == null) UICardNext3 = ModContent.GetTexture("OrchidMod/Gambler/GamblerReset");

			if (UIDeckbuilding == null) UIDeckbuilding = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/DeckUI");
			if (UIDeckbuildingBlock == null) UIDeckbuildingBlock = ModContent.GetTexture("OrchidMod/Gambler/UI/Textures/DeckUIBlock");

		}
	}
}