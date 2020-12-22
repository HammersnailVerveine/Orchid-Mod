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
using OrchidMod.Alchemist;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistSelectUIFrame : UIElement
    {
		public Color backgroundColor = Color.White;	
		public Color backgroundColorGrayed = new Color(90, 90, 90);	
		public Color backgroundColorDark = new Color(180, 180, 180);
		public Point mouseDiff = new Point(0, 0);
		private static int drawOffSet = 21;
		private static int drawSize = 42;
		public int nbAlchemistWeapons = 0;
		public double displayAngle = 0;
		public int distanceToPoint = 0;
		public Point displayPoint = new Point(0, 0);
		public List<Rectangle> displayRectangles = new List<Rectangle>();
		public List<Item> displayItems = new List<Item>();
		
		public static Texture2D resourceBack;
		public static Texture2D resourceItem;
		public static Texture2D resourceCross;
		public static Texture2D resourceSelected;
		public static Texture2D resourceBorder;
		
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Player player = Main.LocalPlayer;
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			CalculatedStyle dimensions = GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			bool noReposition = false;
			
			if (!player.dead) {
				if (modPlayer.alchemistSelectUIDisplay && modPlayer.alchemistSelectUIItem) {
					if (Main.mouseLeft && Main.mouseLeftRelease) {
						if (modPlayer.alchemistNbElements > 0) {
							float shootSpeed = 10f * modPlayer.alchemistVelocity;
							int projType = ProjectileType<Alchemist.Projectiles.AlchemistProj>();
							Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 106);
							modPlayer.alchemistSelectUIDisplay = false;
							modPlayer.alchemistShootProjectile = true;
							return;
						}
					}
					
					if (Main.mouseRight && Main.mouseRightRelease && !modPlayer.alchemistSelectUIInitialize) {
						for (int i = 0 ; i < this.displayRectangles.Count(); i++) {
							if (displayRectangles[i].Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y)) && i <= displayItems.Count()) {
								if (i == 0) {
									modPlayer.alchemistSelectUIDisplay = false;
								} else {
									Item item = displayItems[i - 1];
									if (item.type != 0) {
										OrchidModGlobalItem orchidItem = displayItems[i - 1].GetGlobalItem<OrchidModGlobalItem>();
										AlchemistElement element = orchidItem.alchemistElement;
										int damage = item.damage;
										int flaskType = item.type;
										int potencyCost = orchidItem.alchemistPotencyCost;
										int rightClickDust = orchidItem.alchemistRightClickDust;
										int colorR = orchidItem.alchemistColorR;
										int colorG = orchidItem.alchemistColorG;
										int colorB = orchidItem.alchemistColorB;
										bool noPotency = modPlayer.alchemistPotency < potencyCost + 1;
										bool alreadyContains = false;
										if ((int)element > 0 && (int)element < 7) {
											alreadyContains = modPlayer.alchemistElements[(int)element - 1];
										}
										if (alreadyContains || noPotency 
										|| modPlayer.alchemistNbElements >= modPlayer.alchemistNbElementsMax 
										|| element == AlchemistElement.NULL || flaskType == 0) {
											if (noPotency && !alreadyContains) {
												Main.PlaySound(19, (int)player.Center.X ,(int)player.Center.Y, 1);
											} else {
												if (Main.rand.Next(2) == 0) {
													Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 112);
												} else {
													Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 111);
												}
											}
										} else {
											OrchidModAlchemistItem.playerAddFlask(player, element, flaskType, damage, potencyCost, rightClickDust, colorR, colorG, colorB);
											int rand = Main.rand.Next(3);
											switch (rand) {
												case 1:
													rand = 86;
													break;
												case 2:
													rand = 87;
													break;
												default:
													rand = 85;
													break;
											}
											Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, rand);
											for(int k = 0; k < 5; k ++) {
												int dust = Dust.NewDust(player.Center, 10, 10, rightClickDust);
											}	
										}
									}
								}
								noReposition = true;
							}
						}
					}
					
					if (modPlayer.alchemistSelectUIInitialize) {
						this.initUI(player, modPlayer, ref mouseDiff, ref displayPoint);
						return;
					}
					
					displayPoint.X = point.X - mouseDiff.X;
					displayPoint.Y = point.Y - mouseDiff.Y;
					
					String msg = "";
					bool anySelected = false;
					for (int i = 0 ; i < this.nbAlchemistWeapons + 1 ; i ++) {
						bool selected = displayRectangles[i].Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y));
						anySelected = anySelected ? anySelected : selected;
						if (i > 0) {
							Item item = this.displayItems[i - 1];
							if (item.type != 0) {
								OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
								AlchemistElement element = orchidItem.alchemistElement;
								bool elementSelected = false;
								if ((int)element > 0 && (int)element < 7) {
									elementSelected = modPlayer.alchemistElements[(int)element - 1];
								}
								Color borderColor = new Color(0, 0, 0);
								switch (element) {
									case AlchemistElement.WATER:
										borderColor = selected ? new Color(0, 119, 190) : new Color(0, 69, 140);
										break;
									case AlchemistElement.FIRE:
										borderColor = selected ? new Color(194, 38, 31) : new Color(104, 0, 0);
										break;
									case AlchemistElement.NATURE:
										borderColor = selected ? new Color(75, 139, 59) : new Color(25, 89, 9);
										break;
									case AlchemistElement.AIR:
										borderColor = selected ? new Color(116, 181, 205) : new Color(66, 131, 155);
										break;
									case AlchemistElement.LIGHT:
										borderColor = selected ? new Color(255, 255, 102) : new Color(205, 205, 52);
										break;
									case AlchemistElement.DARK:
										borderColor = selected ? new Color(138, 43, 226) : new Color(88, 0, 176);
										break;
									default:
										break;
								}
								Color color = selected ? elementSelected ? backgroundColorDark : backgroundColor : elementSelected ? backgroundColorGrayed : backgroundColorDark;
								spriteBatch.Draw(resourceBack, this.displayRectangles[i], color);
								spriteBatch.Draw(resourceBorder, this.displayRectangles[i], borderColor);
								resourceItem = Main.itemTexture[item.type];
								Rectangle insideRectangle = new Rectangle(displayRectangles[i].X + 6, displayRectangles[i].Y + 4, 30, 30);
								spriteBatch.Draw(resourceItem, insideRectangle, color);
								msg = selected ? item.Name : msg;
							}
						} else {
							Color color = selected ? backgroundColor : backgroundColorDark;
							spriteBatch.Draw(resourceCross, this.displayRectangles[i], color);
							if (selected) {
								spriteBatch.Draw(resourceSelected, this.displayRectangles[i], color);
								msg = "Close";
							}
						}
					}
					if (msg != "") {
						ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, Main.MouseScreen + new Vector2(15f, 15f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					}
							
					if (!noReposition && !anySelected && ((Main.mouseRight && Main.mouseRightRelease) || (Main.mouseLeft && Main.mouseLeftRelease))) {
						this.initUI(player, modPlayer, ref mouseDiff, ref displayPoint);
						return;
					}
					
					if (Main.mouseLeft && Main.mouseLeftRelease) {
						if (displayRectangles[0].Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y)) && Main.mouseLeft && Main.mouseLeftRelease) {
							modPlayer.alchemistSelectUIDisplay = false;
						}
					}
				}
			}
		}
		
		public void initUI(Player player, OrchidModPlayer modPlayer, ref Point mouseDiff, ref Point displayPoint) {
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			modPlayer.alchemistSelectUIInitialize = false;
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y, 7);
			mouseDiff = new Point(point.X - (int)Main.MouseScreen.X, point.Y - (int)Main.MouseScreen.Y);
			displayPoint.X = point.X - mouseDiff.X;
			displayPoint.Y = point.Y - mouseDiff.Y;
			this.checkInventory();
			this.setRectangles();
		}
		
		public void checkInventory() {
			this.nbAlchemistWeapons = 0;
			int val = this.displayRectangles.Count() - 1;
			this.displayItems = new List<Item>();
			
			for (int i = 0; i < Main.maxInventory; i++) {
				Item item = Main.LocalPlayer.inventory[i];
				if (item.type != 0) {
					OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
					if (orchidItem.alchemistWeapon) {
						this.nbAlchemistWeapons ++;
						this.displayItems.Add(item);
						if (this.nbAlchemistWeapons >= val) {
							break;
						}
					}
				}
			}
			if (this.nbAlchemistWeapons > val) {
				this.nbAlchemistWeapons = val;
			}
			if (this.nbAlchemistWeapons > 0) {
				this.displayAngle = 360 / this.nbAlchemistWeapons;
			} else {
				this.displayAngle = 360;
			}
			this.distanceToPoint = (int)(drawOffSet * 3 + (drawOffSet * this.nbAlchemistWeapons / 4));
		}
		
		public void setRectangles() {
			this.displayRectangles = new List<Rectangle>();
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet, displayPoint.Y - drawOffSet, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet - drawSize / 2 + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - 0, displayPoint.Y - drawOffSet - drawSize + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet - drawSize / 2 + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet + drawSize / 2 + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + 0, displayPoint.Y - drawOffSet + drawSize + 2, drawSize, drawSize));
			
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet + drawSize / 2 + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - 0, displayPoint.Y - drawOffSet - drawSize * 2 + 4, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet - drawSize - drawSize / 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet - drawSize - drawSize / 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - 0, displayPoint.Y - drawOffSet + drawSize * 2 + 4, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet + drawSize + drawSize / 2 + 6, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet + drawSize + drawSize / 2 + 6, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize * 2, displayPoint.Y - drawOffSet - drawSize + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize * 2, displayPoint.Y - drawOffSet - 0, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize * 2, displayPoint.Y - drawOffSet + drawSize + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize * 2, displayPoint.Y - drawOffSet - drawSize + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize * 2, displayPoint.Y - drawOffSet + 0, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize * 2, displayPoint.Y - drawOffSet + drawSize + 2, drawSize, drawSize));
		}

		public AlchemistSelectUIFrame ()
        {
			if (resourceBack == null) resourceBack = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackground");
			if (resourceItem == null) resourceItem = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackground");
			if (resourceCross == null) resourceCross = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackgroundCross");
			if (resourceSelected == null) resourceSelected = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackgroundSelected");
			if (resourceBorder == null) resourceBorder = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackgroundBorder");
			this.setRectangles();
		}
	}
}