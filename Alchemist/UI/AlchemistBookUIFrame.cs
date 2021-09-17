using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistBookUIFrame : UIElement
	{
		private int bookPageIndex = 0;
		private AlchemistHiddenReactionRecipe bookPopupRecipe = AlchemistHiddenReaction.NullRecipe;
		private bool drawpause = false;
		public Color backgroundColor = Color.White;
		public static Texture2D ressourceBookPage;
		public static Texture2D ressourceBookSlot;
		public static Texture2D ressourceBookSlotEmpty;
		public static Texture2D ressourceBookPopup;

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.Left.Set(Main.screenWidth / 2, 0f);
			this.Top.Set(Main.screenHeight / 2, 0f);

			CalculatedStyle dimensions = GetDimensions();

			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);

			int bookWidth = 384;
			int bookHeight = 544;
			int baseOffSetX = 25;
			int baseOffSetY = 12;
			int recipesPerPage = 13;

			Point point = new Point((int)dimensions.X - (bookWidth / 2), (int)dimensions.Y - (bookHeight / 2));
			Point mousePoint = new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y);

			Rectangle rectangleArrowLeft = new Rectangle(point.X + 270, point.Y + 478, 36, 34);
			Rectangle rectangleArrowRight = new Rectangle(point.X + 326, point.Y + 478, 36, 34);
			
			Player player = Main.LocalPlayer;
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (!player.dead)
			{
				if (modPlayer.alchemistBookUIDisplay)
				{
					spriteBatch.Draw(ressourceBookPage, new Rectangle(point.X, point.Y, bookWidth, bookHeight), backgroundColor);

					int offSetY = baseOffSetY;
					int offSetX = baseOffSetX;
					int index = 0;
					string msg = "";

					Item item = null;

					foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes) {
						int progression = OrchidModAlchemistHelper.getProgressLevel();
						bool knownRecipe = modPlayer.alchemistKnownReactions.Contains((int)recipe.reactionType);
						bool knownHint = modPlayer.alchemistKnownHints.Contains((int)recipe.reactionType);
						if (index < ((this.bookPageIndex * recipesPerPage) + recipesPerPage) && index >= (this.bookPageIndex * recipesPerPage)
						&& (knownRecipe || knownHint || (progression >= recipe.reactionLevel && recipe.reactionLevel > 0))) {
							foreach (int ingredientID in recipe.reactionIngredients) {
								if (knownRecipe || knownHint) {
									Texture2D itemTexture = Main.itemTexture[ingredientID];
									spriteBatch.Draw(ressourceBookSlot, new Rectangle(point.X + offSetX, point.Y + offSetY, 36, 36), backgroundColor);
									Rectangle itemRectangle = new Rectangle(point.X + offSetX + 2, point.Y + offSetY + 2, 30, 30);
									spriteBatch.Draw(itemTexture, itemRectangle, knownRecipe ? backgroundColor : Color.Gray);
									if (itemRectangle.Contains(mousePoint) && this.bookPopupRecipe.reactionType == AlchemistHiddenReactionType.NULL) {
										item = new Item();
										item.SetDefaults(ingredientID);
									}
									Rectangle lineRectangle = new Rectangle(point.X, point.Y + offSetY + 2, bookWidth, 36);
									if (lineRectangle.Contains(mousePoint) && (Main.mouseLeft && Main.mouseLeftRelease)
									&& this.bookPopupRecipe.reactionType == AlchemistHiddenReactionType.NULL && knownRecipe) {
										this.bookPopupRecipe = recipe;
										Main.PlaySound(10, (int)player.Center.X, (int)player.Center.Y, 0);
										this.drawpause = true;
									}
								} else {
									spriteBatch.Draw(ressourceBookSlotEmpty, new Rectangle(point.X + offSetX, point.Y + offSetY, 36, 36), backgroundColor);
								}
								offSetX += 40;
							}
							msg = knownRecipe ? recipe.reactionText : "Unknown Reaction";
							Color textColor = knownRecipe ? backgroundColor : new Color(175, 175, 175);
							ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X + offSetX, point.Y + offSetY + 7), textColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
							offSetX = baseOffSetX;
							offSetY += 35;
						}
						index += ((progression >= recipe.reactionLevel && recipe.reactionLevel > 0) || knownRecipe || knownHint) ? 1 : 0;
					}

					int maxPages = (int)(index / recipesPerPage);
					if (this.bookPopupRecipe.reactionType == AlchemistHiddenReactionType.NULL)
					{
						if ((Main.mouseLeft && Main.mouseLeftRelease) && rectangleArrowLeft.Contains(mousePoint))
						{
							this.bookPageIndex -= this.bookPageIndex > 0 ? 1 : 0;
							Main.PlaySound(10, (int)player.Center.X, (int)player.Center.Y, 0);
						}

						if ((Main.mouseLeft && Main.mouseLeftRelease) && rectangleArrowRight.Contains(mousePoint))
						{
							this.bookPageIndex += this.bookPageIndex < maxPages ? 1 : 0;
							Main.PlaySound(10, (int)player.Center.X, (int)player.Center.Y, 0);
						}
					}
					else
					{
						int offSetPopupX = 34;
						int offSetPopupY = 158;
						if ((Main.mouseLeft && Main.mouseLeftRelease) && !this.drawpause)
						{
							this.bookPopupRecipe = AlchemistHiddenReaction.NullRecipe;
							Main.PlaySound(10, (int)player.Center.X, (int)player.Center.Y, 0);
						}

						spriteBatch.Draw(ressourceBookPopup, new Rectangle(point.X + offSetPopupX, point.Y + offSetPopupY, 318, 200), backgroundColor);
						offSetX = 194;
						int offSetPopup = 0;
						offSetPopup = ((int)(40 * this.bookPopupRecipe.reactionIngredients.Count / 2));
						foreach (int ingredientID in this.bookPopupRecipe.reactionIngredients)
						{
							Texture2D itemTexturePopup = Main.itemTexture[ingredientID];
							spriteBatch.Draw(ressourceBookSlot, new Rectangle(point.X + offSetX - offSetPopup, point.Y + offSetPopupY + 16, 36, 36), backgroundColor);
							Rectangle itemRectangle = new Rectangle(point.X + offSetX + 2 - offSetPopup, point.Y + offSetPopupY + 18, 30, 30);
							spriteBatch.Draw(itemTexturePopup, itemRectangle, backgroundColor);
							if (itemRectangle.Contains(mousePoint))
							{
								item = new Item();
								item.SetDefaults(ingredientID);
							}
							offSetX += 40;
						}
						Vector2 textPos = new Vector2(point.X + offSetPopupX + 25, point.Y + offSetPopupY + 70);
						msg = this.bookPopupRecipe.reactionText;
						ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, textPos, backgroundColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
						msg = this.bookPopupRecipe.reactionDescription;
						textPos.Y += 40;
						textPos.X -= 12;
						ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, textPos, backgroundColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					}

					if (item != null)
					{
						ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, item.Name, Main.MouseScreen + new Vector2(15f, 15f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					}

					this.drawpause = this.drawpause ? Main.mouseLeftRelease : false;

					msg = "Page " + (this.bookPageIndex + 1) + "/" + (maxPages + 1);
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X + 282, point.Y + 508), backgroundColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				}
			}
		}

		public AlchemistBookUIFrame()
		{
			if (ressourceBookPage == null) ressourceBookPage = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookPage");
			if (ressourceBookSlot == null) ressourceBookSlot = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookSlot");
			if (ressourceBookSlotEmpty == null) ressourceBookSlotEmpty = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookSlotEmpty");
			if (ressourceBookPopup == null) ressourceBookPopup = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookPopup");
		}
	}
}