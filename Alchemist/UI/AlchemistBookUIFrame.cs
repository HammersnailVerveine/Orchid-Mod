using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ID;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using OrchidMod;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistBookUIFrame : UIElement
    {
		private int bookPageIndex = 0;
		public Color backgroundColor = Color.White;	
		public static Texture2D ressourceBookPage;
		public static Texture2D ressourceBookSlot;
		public static Texture2D ressourceBookSlotEmpty;
		
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			Vector2 vector = new Vector2((float)((int)(Main.LocalPlayer.position.X - Main.screenPosition.X) - Main.GameViewMatrix.Translation.X - (float)(Main.LocalPlayer.bodyFrame.Width / 2) + (float)(Main.LocalPlayer.width / 2)), (float)((int)(Main.LocalPlayer.position.Y - Main.screenPosition.Y) - Main.GameViewMatrix.Translation.Y + (float)Main.LocalPlayer.height - (float)Main.LocalPlayer.bodyFrame.Height + 12f)) + Main.LocalPlayer.bodyPosition + new Vector2((float)(Main.LocalPlayer.bodyFrame.Width / 2));
			vector *= Main.GameViewMatrix.Zoom;
			vector /= Main.UIScale;
				
			this.Left.Set(vector.X, 0f);
			this.Top.Set(vector.Y - 40f, 0f);
			
			CalculatedStyle dimensions = GetDimensions();

            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			int bookWidth = 384;
			int bookHeight = 544;
			int baseOffSetX = 25;
			int baseOffSetY = 12;
			int recipesPerPage = 13;
			
            Point point = new Point((int)dimensions.X - (bookWidth / 2), (int)dimensions.Y - (bookHeight / 2));
			Point mousePoint = new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y);
			
			Rectangle rectangleArrowLeft = new Rectangle(point.X + 270, point.Y + 484, 36, 34);
			Rectangle rectangleArrowRight = new Rectangle(point.X + 326, point.Y + 484, 36, 34);
			
			if (!player.dead) {
				if (modPlayer.alchemistBookUIDisplay) {
					spriteBatch.Draw(ressourceBookPage, new Rectangle(point.X, point.Y , bookWidth, bookHeight), backgroundColor);

					int offSetY = baseOffSetY;
					int offSetX = baseOffSetX;
					int index = 0;
						
					foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes) {
						if (index < ((this.bookPageIndex * recipesPerPage) + recipesPerPage) && index >= (this.bookPageIndex * recipesPerPage)) {
							bool knownRecipe = modPlayer.alchemistKnownReactions.Contains((int)recipe.reactionType);
							foreach(int ingredientID in recipe.reactionIngredients) {
								if (knownRecipe) {
									Texture2D itemTexture = Main.itemTexture[ingredientID];
									spriteBatch.Draw(ressourceBookSlot, new Rectangle(point.X + offSetX, point.Y + offSetY, 36, 36), backgroundColor);
									spriteBatch.Draw(itemTexture, new Rectangle(point.X + offSetX + 2, point.Y + offSetY + 2, 30, 30), backgroundColor);
								} else {
									spriteBatch.Draw(ressourceBookSlotEmpty, new Rectangle(point.X + offSetX, point.Y + offSetY, 36, 36), backgroundColor);
								}
								offSetX += 40;
							}
							string msg = knownRecipe ? recipe.reactionText : "Unknown Reaction";
							//Color textColor = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
							ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X + offSetX, point.Y + offSetY + 7), backgroundColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
							offSetX = baseOffSetX;
							offSetY += 35;
						}
						index ++;
					}
					
					if ((Main.mouseLeft && Main.mouseLeftRelease) && rectangleArrowLeft.Contains(mousePoint)) {
						this.bookPageIndex -= this.bookPageIndex > 0 ? 1 : 0;
					}
					
					if ((Main.mouseLeft && Main.mouseLeftRelease) && rectangleArrowRight.Contains(mousePoint)) {
						int maxPages = (int)(OrchidMod.alchemistReactionRecipes.Count / recipesPerPage);
						this.bookPageIndex += this.bookPageIndex < maxPages ? 1 : 0;
					}
				}
			}
		}
		
		public AlchemistBookUIFrame () {
			if (ressourceBookPage == null) ressourceBookPage = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookPage");
			if (ressourceBookSlot == null) ressourceBookSlot = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookSlot");
			if (ressourceBookSlotEmpty == null) ressourceBookSlotEmpty = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistBookSlotEmpty");
		}
	}
}