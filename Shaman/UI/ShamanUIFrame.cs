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
using Terraria.GameInput;
using Terraria.Localization;

namespace OrchidMod.Shaman.UI
{
	public class ShamanUIFrame : UIElement
    {
		public Color backgroundColor = Color.White;
		public static Texture2D shamanUIMainFrame;
		public static Texture2D shamanUILevel;
		
		public static Texture2D resourceDuration;
		public static Texture2D resourceDurationEnd;
		public static Texture2D resourceFire;
		public static Texture2D resourceFireEnd;
		public static Texture2D resourceWater;
		public static Texture2D resourceWaterEnd;
		public static Texture2D resourceAir;
		public static Texture2D resourceAirEnd;
		public static Texture2D resourceEarth;
		public static Texture2D resourceEarthEnd;
		public static Texture2D resourceSpirit;
		public static Texture2D resourceSpiritEnd;

		public static Texture2D FireSymbolBasic;
		public static Texture2D WaterSymbolBasic;
		public static Texture2D AirSymbolBasic;
		public static Texture2D EarthSymbolBasic;
		public static Texture2D SpiritSymbolBasic;
		
		public static Texture2D Bonus1Symbol;
		public static Texture2D Bonus2Symbol;
		public static Texture2D Bonus3Symbol;
		public static Texture2D Bonus4Symbol;
		public static Texture2D Bonus5Symbol;
		public static Texture2D Bonus6Symbol;
		public static Texture2D Bonus7Symbol;
		public static Texture2D Bonus8Symbol;
		public static Texture2D Bonus9Symbol;
		public static Texture2D Bonus10Symbol;
		public static Texture2D Bonus11Symbol;
		public static Texture2D Bonus12Symbol;
		public static Texture2D Bonus13Symbol;
		public static Texture2D Bonus14Symbol;
		public static Texture2D Bonus15Symbol;
		
		public static Texture2D Level1Symbol;
		public static Texture2D Level2Symbol;
		public static Texture2D Level3Symbol;
		public static Texture2D Level4Symbol;
		public static Texture2D Level5Symbol;
		
		public static Texture2D SymbolIce;
		public static Texture2D SymbolFire;
		public static Texture2D SymbolPoison;
		public static Texture2D SymbolVenom;
		public static Texture2D SymbolDemonite;
		public static Texture2D SymbolHeavy;
		public static Texture2D SymbolForest;
		public static Texture2D SymbolDiabolist;
		public static Texture2D SymbolSkull;
		public static Texture2D SymbolWaterHoney;
		public static Texture2D SymbolDestroyer;
		public static Texture2D SymbolBee;
		public static Texture2D SymbolAmber;
		public static Texture2D SymbolSmite;
		public static Texture2D SymbolCrimtane;
		public static Texture2D SymbolRage;
		public static Texture2D SymbolLava;
		public static Texture2D SymbolFeather;
		public static Texture2D SymbolAnklet;
		public static Texture2D SymbolWyvern;
		
		public enum MouseHoverLocation {NULL,ATTACKLEVEL,ARMORLEVEL,CRITICALLEVEL,REGENERATIONLEVEL,SPEEDLEVEL,EMPOWERMENTDURATION}
		
		public float usedScale = 0f;
		
		Player player = Main.player[Main.myPlayer];
		
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this.usedScale != Main.UIScale) {
				this.usedScale = Main.UIScale;
				this.Left.Set(Main.screenWidth - 100f, 0f);
				this.Top.Set(Main.screenHeight - 130f, 0f);
			}
			
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			CalculatedStyle dimensions = GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y - 100);
			if (Main.invasionType != 0 || Main.pumpkinMoon || Main.snowMoon)
				point.Y -= 60;
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
			if (!Main.playerInventory && !player.dead) {
				int buffTimerVal = ((int)(modPlayer.shamanBuffTimer * 60 / 25));
				buffTimerVal += buffTimerVal == 0 ? 1 : 0;
				int buffTimerValRef = modPlayer.shamanBuffTimer * 60 - buffTimerVal;
				
				if (modPlayer.UIDisplayTimer > 0) {  
					spriteBatch.Draw(shamanUIMainFrame, new Rectangle(point.X, point.Y, width, height), backgroundColor);
					spriteBatch.Draw(shamanUILevel, new Rectangle(point.X - 24, point.Y + 16, 22, 166), backgroundColor);
					
					this.drawnBondEffectBar(1, modPlayer.shamanFireBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(2, modPlayer.shamanWaterBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(3, modPlayer.shamanAirBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(4, modPlayer.shamanEarthBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(5, modPlayer.shamanSpiritBondLoading, spriteBatch, point);
				}
				
				if (modPlayer.shamanFireBuff > 0) {
					spriteBatch.Draw(FireSymbolBasic, new Rectangle(point.X + 70, point.Y + 8, width/(94/18), height/(180/18)), backgroundColor);
					int timerRef = modPlayer.shamanFireTimer - buffTimerVal;
					timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
					this.drawDuration(timerRef, 24, buffTimerVal, spriteBatch, point);
					
					this.drawBuffIcon(1, modPlayer.shamanFireBuff, spriteBatch, point);
					this.drawBonusIcon(1, modPlayer.shamanFireBonus, spriteBatch, point);
					
					List<Texture2D> symbolsList = new List<Texture2D>();
					if (modPlayer.shamanFire) symbolsList.Add(SymbolFire);
					if (modPlayer.shamanIce) symbolsList.Add(SymbolIce);
					if (modPlayer.shamanPoison) symbolsList.Add(SymbolPoison);
					if (modPlayer.shamanDemonite) symbolsList.Add(SymbolDemonite);
					if (modPlayer.shamanVenom) symbolsList.Add(SymbolVenom);
					if (modPlayer.shamanSmite) symbolsList.Add(SymbolSmite);
					
					this.drawSymbols(2, symbolsList, spriteBatch, point);
				}
				
				if (modPlayer.shamanWaterBuff > 0) {
					spriteBatch.Draw(WaterSymbolBasic, new Rectangle(point.X + 70, point.Y + 44, width/(94/18), height/(180/18)), backgroundColor);
					int timerRef = modPlayer.shamanWaterTimer - buffTimerVal;
					timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
					this.drawDuration(timerRef, 60, buffTimerVal, spriteBatch, point);

					this.drawBuffIcon(2, modPlayer.shamanWaterBuff, spriteBatch, point);
					this.drawBonusIcon(2, modPlayer.shamanWaterBonus, spriteBatch, point);
					
					List<Texture2D> symbolsList = new List<Texture2D>();
					if (modPlayer.shamanWaterHoney) symbolsList.Add(SymbolWaterHoney);
					if (modPlayer.shamanSkull) symbolsList.Add(SymbolSkull);
					if (modPlayer.shamanDestroyer) symbolsList.Add(SymbolDestroyer);
					
					this.drawSymbols(38, symbolsList, spriteBatch, point);
				}
				
				if (modPlayer.shamanAirBuff > 0) {
					spriteBatch.Draw(AirSymbolBasic, new Rectangle(point.X + 70, point.Y + 80, width/(94/18), height/(180/18)), backgroundColor);
					int timerRef = modPlayer.shamanAirTimer - buffTimerVal;
					timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
					this.drawDuration(timerRef, 96, buffTimerVal, spriteBatch, point);	
					
					this.drawBuffIcon(3, modPlayer.shamanAirBuff, spriteBatch, point);
					this.drawBonusIcon(3, modPlayer.shamanAirBonus, spriteBatch, point);
					
					List<Texture2D> symbolsList = new List<Texture2D>();
					if (modPlayer.shamanFeather) symbolsList.Add(SymbolFeather);
					if (modPlayer.shamanHarpyAnklet) symbolsList.Add(SymbolAnklet);
					if (modPlayer.shamanDripping) symbolsList.Add(SymbolLava);
					if (modPlayer.shamanWyvern) symbolsList.Add(SymbolWyvern);
					
					this.drawSymbols(74, symbolsList, spriteBatch, point);
				}
				
				if (modPlayer.shamanEarthBuff > 0) {
					spriteBatch.Draw(EarthSymbolBasic, new Rectangle(point.X + 70, point.Y + 116, width/(94/18), height/(180/18)), backgroundColor);
					int timerRef = modPlayer.shamanEarthTimer - buffTimerVal;
					timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
					this.drawDuration(timerRef, 132, buffTimerVal, spriteBatch, point);
					
					this.drawBuffIcon(4, modPlayer.shamanEarthBuff, spriteBatch, point);
					this.drawBonusIcon(4, modPlayer.shamanEarthBonus, spriteBatch, point);
					
					List<Texture2D> symbolsList = new List<Texture2D>();
					if (modPlayer.shamanHoney) symbolsList.Add(SymbolBee);
					if (modPlayer.shamanAmber) symbolsList.Add(SymbolAmber);
					if (modPlayer.shamanCrimtane) symbolsList.Add(SymbolCrimtane);
					if (modPlayer.shamanRage) symbolsList.Add(SymbolRage);
					if (modPlayer.shamanHeavy) symbolsList.Add(SymbolHeavy);
					if (modPlayer.shamanForest) symbolsList.Add(SymbolForest);
					if (modPlayer.shamanDiabolist) symbolsList.Add(SymbolDiabolist );
					
					this.drawSymbols(110, symbolsList, spriteBatch, point);
				}
				
				if (modPlayer.shamanSpiritBuff > 0) {
					spriteBatch.Draw(SpiritSymbolBasic, new Rectangle(point.X + 70, point.Y + 152, width/(94/18), height/(180/18)), backgroundColor);
					int timerRef = modPlayer.shamanSpiritTimer - buffTimerVal;
					timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
					this.drawDuration(timerRef, 168, buffTimerVal, spriteBatch, point);
					
					this.drawBuffIcon(5, modPlayer.shamanSpiritBuff, spriteBatch, point);
					this.drawBonusIcon(5, modPlayer.shamanSpiritBonus, spriteBatch, point);
					
					List<Texture2D> symbolsList = new List<Texture2D>();
					
					this.drawSymbols(146, symbolsList, spriteBatch, point);
				}
			}
			
			if (!Main.playerInventory && !player.dead) {
				if (modPlayer.UIDisplayTimer > 0) {  
					if (Main.mouseX > point.X - 30 && Main.mouseY > point.Y - 30) {
					
						int levelIconSize = 10;
						int barIconWidth = 64;
						int offSet = 8;
						int iconsSpacing = 36;
						MouseHoverLocation mouseHoverLocation = MouseHoverLocation.NULL;
						
						int iconPosX = 18;
						int iconPosY = 22;
						
						for (int i = 0 ; i < 5 ; i ++) {
							if (Main.mouseX > point.X - iconPosX - offSet && Main.mouseX < point.X - iconPosX + levelIconSize + barIconWidth + offSet 
							&& Main.mouseY > point.Y + iconPosY - offSet && Main.mouseY < point.Y + iconPosY + levelIconSize + offSet 
							&& !PlayerInput.IgnoreMouseInterface) {
								switch (i) {
									case 0:
										mouseHoverLocation = MouseHoverLocation.ATTACKLEVEL;
										break;
									case 1:
										mouseHoverLocation = MouseHoverLocation.ARMORLEVEL;
										break;
									case 2:
										mouseHoverLocation = MouseHoverLocation.CRITICALLEVEL;
										break;
									case 3:
										mouseHoverLocation = MouseHoverLocation.REGENERATIONLEVEL;
										break;
									case 4:
										mouseHoverLocation = MouseHoverLocation.SPEEDLEVEL;
										break;
									default:
										break;
								}
							}
							iconPosY += iconsSpacing;
						}
						
						// for (int i = 0 ; i < 5 ; i ++) {
							// if (Main.mouseX > point.X - iconPosX - offSet && Main.mouseX < point.X - iconPosX + barIconWidth + offSet 
							// && Main.mouseY > point.Y + iconPosY - offSet && Main.mouseY < point.Y + iconPosY + barIconHeight + offSet 
							// && !PlayerInput.IgnoreMouseInterface) {
								// mouseHoverLocation = MouseHoverLocation.EMPOWERMENTDURATION;
							// }
							// iconPosY += iconsSpacing;
						// }
					
						string message = "";
						string bonus = "";
						int textLength = 610;
						int offSetY = 0;
						int level = 0;
						Color mouseColor = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
						Color grayColor = new Color(65, 65, 65);
						
						switch (mouseHoverLocation) {
							case MouseHoverLocation.NULL:
								break;
							case MouseHoverLocation.ATTACKLEVEL:
								message = modPlayer.shamanFireBuff == 0 ? "" : "Fire bond level " + modPlayer.shamanFireBuff;
								bonus = modPlayer.shamanFireBonus == 0 ? "" : "Fire bond level bonus " + modPlayer.shamanFireBonus;
								
								level = modPlayer.shamanFireBuff + modPlayer.shamanFireBonus;
								String msg = "Active effect : Hurls a fireball at your cursor";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "Level increases fireball damage and speed";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "Charges over time and in combat";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "(Level 3) : The fireball pierces up to 2 enemies";
								Color color = level >= 3 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 5) : The fireball explodes on first hit";
								color = level >= 5 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 8) : The explosion releases lingering embers";
								color = level >= 8 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 12) : Hit enemies will take heavy damage after a delay";
								color = level >= 12 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								break;
							case MouseHoverLocation.ARMORLEVEL:
								message = modPlayer.shamanWaterBuff == 0 ? "" : "Water bond level " + modPlayer.shamanWaterBuff;
								bonus = modPlayer.shamanWaterBonus == 0 ? "" : "Water bond level bonus " + modPlayer.shamanWaterBonus;

								level = modPlayer.shamanWaterBuff + modPlayer.shamanWaterBonus;
								msg = "Active effect : Releases homing bolts of water";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "Level increases bolts damage and number";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "Charges by filling bond durations";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "(Level 3) : Bolts have knockback";
								color = level >= 3 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 5) : Bolts restores 5 health on hit";
								color = level >= 5 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 8) : Hit enemies take 5% increased damage";
								color = level >= 8 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 12) : Hit enemies have a chance to create more bolts";
								color = level >= 12 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);

								break;
							case MouseHoverLocation.CRITICALLEVEL:
								message = modPlayer.shamanAirBuff == 0 ? "" : "Air bond level " + modPlayer.shamanAirBuff;
								bonus = modPlayer.shamanAirBonus == 0 ? "" : "Air bond level bonus " + modPlayer.shamanAirBonus;
								
								level = modPlayer.shamanAirBuff + modPlayer.shamanAirBonus;
								msg = "Active effect : Summons a wind arrow, releasing powerful tornadoes";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "Level increases damage";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "Charges by moving";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "(Level 3) : Hit players receive a movement speed burst";
								color = level >= 3 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 5) : Hit enemies are slowed for a duration";
								color = level >= 5 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 8) : Hit enemies are stunned for a duration";
								color = level >= 8 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 12) : Stunned enemies take heavy damage over time";
								color = level >= 12 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								break;
							case MouseHoverLocation.REGENERATIONLEVEL:
								message = modPlayer.shamanEarthBuff == 0 ? "" : "Earth bond level " + modPlayer.shamanEarthBuff;
								bonus = modPlayer.shamanEarthBonus == 0 ? "" : "Earth bond level bonus " + modPlayer.shamanEarthBonus;
								
								level = modPlayer.shamanEarthBuff + modPlayer.shamanEarthBonus;
								msg = "Active effect : Recovers some of the shaman's health";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "Level increases healing done";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "Charges by standing still";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "(Level 3) : Gives a defense buff to nearby players";
								color = level >= 3 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 5) : Summons a totem, boosting nearby players defensive stats";
								color = level >= 5 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 8) : The buff now increases life regeneration";
								color = level >= 8 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 12) : Dying close to the totem revives you";
								color = level >= 12 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								break;
							case MouseHoverLocation.SPEEDLEVEL:
								message = modPlayer.shamanSpiritBuff == 0 ? "" : "Spirit bond level " + modPlayer.shamanSpiritBuff;
								bonus = modPlayer.shamanSpiritBonus == 0 ? "" : "Spirit bond level bonus " + modPlayer.shamanSpiritBonus;
								
								level = modPlayer.shamanSpiritBuff + modPlayer.shamanSpiritBonus;
								msg = "Active effect : Partially fills up all shamanic bond timers";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "Level increases amount filled";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "Charges by using orb weapons";
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 35;
								msg = "(Level 3) : Gives a damage buff to nearby players";
								color = level >= 3 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 5) : Summons a spirit, boosting nearby players offensive stats";
								color = level >= 5 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 8) : The buff increases resource generation";
								color = level >= 8 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								offSetY -= 25;
								msg = "(Level 12) : Bonds duration timers are paused while near the spirit";
								color = level >= 12 ? mouseColor : grayColor;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, msg, new Vector2(point.X - textLength, point.Y - offSetY), color, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
								break;
							default:
								break;
						}
						
						if (message != "") {
							offSetY = bonus != "" ? 60 : 35;
							ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
							
							if (bonus != "") {
								offSetY = 35;
								ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, bonus, new Vector2(point.X - textLength, point.Y - offSetY), mouseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
							}
						}
					}
				}
			}
		}
		
		public void drawDuration(int timerRef, int offSetY, int buffTimerVal, SpriteBatch spriteBatch, Point point) {
			int resourceOffSet = 0;
			while (timerRef > 0) {
				Texture2D barTexture = timerRef - buffTimerVal < 0 ? resourceDurationEnd : resourceDuration;
				spriteBatch.Draw(barTexture, new Rectangle(point.X + 54 - resourceOffSet, point.Y + offSetY, 2, 4), backgroundColor);
				timerRef -= buffTimerVal;
				resourceOffSet += 2;
			}	
		}
		
		public void drawSymbols(int offSetY, List<Texture2D> list, SpriteBatch spriteBatch, Point point) {
			int iconsNumber = 0;
			foreach (Texture2D texture in list) {
				spriteBatch.Draw(texture, new Rectangle(point.X + 38 - (12 * iconsNumber), point.Y + offSetY, 10, 14), backgroundColor);
				iconsNumber ++;
			}
		}
		
		public void drawnBondEffectBar(byte type, int val, SpriteBatch spriteBatch, Point point) {
			int offSetY = 0;
			int resourceOffSet = 0;
			Texture2D[] barTexture = new Texture2D[]{resourceFire, resourceFireEnd};
			Texture2D usedTexture = barTexture[1];
			
			switch (type) {
				case 1:
					offSetY = 30;
					barTexture[0] = resourceFire;
					barTexture[1] = resourceFireEnd;
					break;
				case 2:
					offSetY = 66;
					barTexture[0] = resourceWater;
					barTexture[1] = resourceWaterEnd;
					break;
				case 3:
					offSetY = 102;
					barTexture[0] = resourceAir;
					barTexture[1] = resourceAirEnd;
					break;
				case 4:
					offSetY = 138;
					barTexture[0] = resourceEarth;
					barTexture[1] = resourceEarthEnd;
					break;
				case 5:
					offSetY = 174;
					barTexture[0] = resourceSpirit;
					barTexture[1] = resourceSpiritEnd;
					break;
				default:
					break;
			}
			
			while (val > 0) {
				usedTexture = val - 4 <= 0 ? barTexture[1] : barTexture[0];
				spriteBatch.Draw(usedTexture, new Rectangle(point.X + 54 - resourceOffSet, point.Y + offSetY, 2, 2), backgroundColor);
				val -= 4;
				resourceOffSet += 2;
			}
		}
		
		public void drawBuffIcon(byte type, int level, SpriteBatch spriteBatch, Point point) {
			int offSetY = 0;
			Texture2D iconTexture = Level1Symbol;
			switch (type) {
				case 1:
					offSetY = 22;
					break;
				case 2:
					offSetY = 58;
					break;
				case 3:
					offSetY = 94;
					break;
				case 4:
					offSetY = 130;
					break;
				case 5:
					offSetY = 166;
					break;
				default:
					break;
			}
			
			switch (level) {
				case 1:
					iconTexture = Level1Symbol;
					break;
				case 2:
					iconTexture = Level2Symbol;
					break;
				case 3:
					iconTexture = Level3Symbol;
					break;
				case 4:
					iconTexture = Level4Symbol;
					break;
				case 5:
					iconTexture = Level5Symbol;
					break;
				default:
					break;
			}
			
			spriteBatch.Draw(iconTexture, new Rectangle(point.X - 18, point.Y + offSetY, 10, 10), backgroundColor);
		}
		
		public void drawBonusIcon(byte type, int level, SpriteBatch spriteBatch, Point point) {
			if (level > 0) {
				int offSetY = 0;
				Texture2D iconTexture = Level1Symbol;
				switch (type) {
					case 1:
						offSetY = 2;
						break;
					case 2:
						offSetY = 38;
						break;
					case 3:
						offSetY = 74;
						break;
					case 4:
						offSetY = 110;
						break;
					case 5:
						offSetY = 146;
						break;
					default:
						break;
				}

				switch (level) {
					case 1:
						iconTexture = Bonus1Symbol;
						break;
					case 2:
						iconTexture = Bonus2Symbol;
						break;
					case 3:
						iconTexture = Bonus3Symbol;
						break;
					case 4:
						iconTexture = Bonus4Symbol;
						break;
					case 5:
						iconTexture = Bonus5Symbol;
						break;
					case 6:
						iconTexture = Bonus6Symbol;
						break;
					case 7:
						iconTexture = Bonus7Symbol;
						break;
					case 8:
						iconTexture = Bonus8Symbol;
						break;
					case 9:
						iconTexture = Bonus9Symbol;
						break;
					case 10:
						iconTexture = Bonus10Symbol;
						break;
					case 11:
						iconTexture = Bonus11Symbol;
						break;
					case 12:
						iconTexture = Bonus12Symbol;
						break;
					case 13:
						iconTexture = Bonus13Symbol;
						break;
					case 14:
						iconTexture = Bonus14Symbol;
						break;
					case 15:
						iconTexture = Bonus15Symbol;
						break;
					default:
						break;
				}
				
				spriteBatch.Draw(iconTexture, new Rectangle(point.X + 50, point.Y + offSetY, 14, 14), backgroundColor);
			}
		}
		
		public ShamanUIFrame ()
        {
			if (shamanUIMainFrame == null) shamanUIMainFrame = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/MainGrey");
			if (shamanUILevel == null) shamanUILevel = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/MainLevel");
			
			if (resourceDuration == null) resourceDuration = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/DurationBar");
			if (resourceDurationEnd == null) resourceDurationEnd = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/DurationBarEnd");
			
			if (resourceFire == null) resourceFire = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBar");
			if (resourceFireEnd == null) resourceFireEnd = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBarEnd");
			if (resourceWater == null) resourceWater = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBar");
			if (resourceWaterEnd == null) resourceWaterEnd = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBarEnd");
			if (resourceAir == null) resourceAir = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBar");
			if (resourceAirEnd == null) resourceAirEnd = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBarEnd");
			if (resourceEarth == null) resourceEarth = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBar");
			if (resourceEarthEnd == null) resourceEarthEnd = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBarEnd");
			if (resourceSpirit == null) resourceSpirit = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBar");
			if (resourceSpiritEnd == null) resourceSpiritEnd = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBarEnd");
			
			if (FireSymbolBasic == null) FireSymbolBasic = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/FireActive");
			if (WaterSymbolBasic == null) WaterSymbolBasic = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/WaterActive");
			if (AirSymbolBasic == null) AirSymbolBasic = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/AirActive");
			if (EarthSymbolBasic == null) EarthSymbolBasic = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/EarthActive");
			if (SpiritSymbolBasic == null) SpiritSymbolBasic = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/SpiritActive");
			
			if (Bonus1Symbol == null) Bonus1Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus1");
			if (Bonus2Symbol == null) Bonus2Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus2");
			if (Bonus3Symbol == null) Bonus3Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus3");
			if (Bonus4Symbol == null) Bonus4Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus4");
			if (Bonus5Symbol == null) Bonus5Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus5");
			if (Bonus6Symbol == null) Bonus6Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus6");
			if (Bonus7Symbol == null) Bonus7Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus7");
			if (Bonus8Symbol == null) Bonus8Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus8");
			if (Bonus9Symbol == null) Bonus9Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus9");
			if (Bonus10Symbol == null) Bonus10Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus10");
			if (Bonus11Symbol == null) Bonus11Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus11");
			if (Bonus12Symbol == null) Bonus12Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus12");
			if (Bonus13Symbol == null) Bonus13Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus13");
			if (Bonus14Symbol == null) Bonus14Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus14");
			if (Bonus15Symbol == null) Bonus15Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Bonus15");
			
			if (Level1Symbol == null) Level1Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Level1");
			if (Level2Symbol == null) Level2Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Level2");
			if (Level3Symbol == null) Level3Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Level3");
			if (Level4Symbol == null) Level4Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Level4");
			if (Level5Symbol == null) Level5Symbol = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/Level5");
			
			if (SymbolFire == null) SymbolFire = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Fire");
			if (SymbolIce == null) SymbolIce = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Ice");
			if (SymbolPoison == null) SymbolPoison = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Poison");
			if (SymbolVenom == null) SymbolVenom = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Venom");
			if (SymbolDemonite == null) SymbolDemonite = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Demonite");
			if (SymbolHeavy == null) SymbolHeavy = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Heavy");
			if (SymbolForest == null) SymbolForest = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Forest");
			if (SymbolDiabolist == null) SymbolDiabolist = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Diabolist");
			if (SymbolWaterHoney == null) SymbolWaterHoney = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/WaterHoney");
			if (SymbolSkull == null) SymbolSkull = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Skull");
			if (SymbolDestroyer == null) SymbolDestroyer = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Destroyer");
			if (SymbolBee == null) SymbolBee = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Bee");
			if (SymbolAmber == null) SymbolAmber = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Amber");
			if (SymbolSmite == null) SymbolSmite = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Smite");
			if (SymbolCrimtane == null) SymbolCrimtane = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Crimtane");
			if (SymbolRage == null) SymbolRage = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Rage");
			if (SymbolLava == null) SymbolLava = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Lava");
			if (SymbolFeather == null) SymbolFeather = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Feather");
			if (SymbolAnklet == null) SymbolAnklet = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Anklet");
			if (SymbolWyvern == null) SymbolWyvern = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Wyvern");
		}
	}
}