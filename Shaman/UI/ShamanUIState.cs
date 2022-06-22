using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.UIs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Shaman.UI
{
	public class ShamanUIState : OrchidUIState
	{
		private Color backgroundColor = Color.White;

		private Texture2D shamanUIMainFrame;

		private Texture2D resourceDuration;
		private Texture2D resourceDurationEnd;
		private Texture2D resourceFire;
		private Texture2D resourceFireEnd;
		private Texture2D resourceWater;
		private Texture2D resourceWaterEnd;
		private Texture2D resourceAir;
		private Texture2D resourceAirEnd;
		private Texture2D resourceEarth;
		private Texture2D resourceEarthEnd;
		private Texture2D resourceSpirit;
		private Texture2D resourceSpiritEnd;

		private Texture2D fireSymbolBasic;
		private Texture2D waterSymbolBasic;
		private Texture2D airSymbolBasic;
		private Texture2D earthSymbolBasic;
		private Texture2D spiritSymbolBasic;

		private Texture2D symbolIce;
		private Texture2D symbolFire;
		private Texture2D symbolPoison;
		private Texture2D symbolVenom;
		private Texture2D symbolDemonite;
		private Texture2D symbolHeavy;
		private Texture2D symbolForest;
		private Texture2D symbolDiabolist;
		private Texture2D symbolSkull;
		private Texture2D symbolWaterHoney;
		private Texture2D symbolDestroyer;
		private Texture2D symbolBee;
		private Texture2D symbolAmber;
		private Texture2D symbolSmite;
		private Texture2D symbolCrimtane;
		private Texture2D symbolRage;
		private Texture2D symbolLava;
		private Texture2D symbolFeather;
		private Texture2D symbolAnklet;
		private Texture2D symbolWyvern;
		private Texture2D symbolAmethyst;
		private Texture2D symbolTopaz;
		private Texture2D symbolSapphire;
		private Texture2D symbolEmerald;
		private Texture2D symbolRuby;

		public enum MouseHoverLocation { NULL, ATTACKLEVEL, ARMORLEVEL, CRITICALLEVEL, REGENERATIONLEVEL, SPEEDLEVEL, EMPOWERMENTDURATION }

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			shamanUIMainFrame = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/MainGrey", AssetRequestMode.ImmediateLoad).Value;

			resourceDuration = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/DurationBar", AssetRequestMode.ImmediateLoad).Value;
			resourceDurationEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/DurationBarEnd", AssetRequestMode.ImmediateLoad).Value;

			resourceFire = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBar", AssetRequestMode.ImmediateLoad).Value;
			resourceFireEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBarEnd", AssetRequestMode.ImmediateLoad).Value;
			resourceWater = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBar", AssetRequestMode.ImmediateLoad).Value;
			resourceWaterEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBarEnd", AssetRequestMode.ImmediateLoad).Value;
			resourceAir = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBar", AssetRequestMode.ImmediateLoad).Value;
			resourceAirEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBarEnd", AssetRequestMode.ImmediateLoad).Value;
			resourceEarth = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBar", AssetRequestMode.ImmediateLoad).Value;
			resourceEarthEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBarEnd", AssetRequestMode.ImmediateLoad).Value;
			resourceSpirit = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBar", AssetRequestMode.ImmediateLoad).Value;
			resourceSpiritEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBarEnd", AssetRequestMode.ImmediateLoad).Value;

			fireSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/FireActive", AssetRequestMode.ImmediateLoad).Value;
			waterSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/WaterActive", AssetRequestMode.ImmediateLoad).Value;
			airSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/AirActive", AssetRequestMode.ImmediateLoad).Value;
			earthSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/EarthActive", AssetRequestMode.ImmediateLoad).Value;
			spiritSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/SpiritActive", AssetRequestMode.ImmediateLoad).Value;

			symbolFire = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Fire", AssetRequestMode.ImmediateLoad).Value;
			symbolIce = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Ice", AssetRequestMode.ImmediateLoad).Value;
			symbolPoison = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Poison", AssetRequestMode.ImmediateLoad).Value;
			symbolVenom = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Venom", AssetRequestMode.ImmediateLoad).Value;
			symbolDemonite = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Demonite", AssetRequestMode.ImmediateLoad).Value;
			symbolHeavy = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Heavy", AssetRequestMode.ImmediateLoad).Value;
			symbolForest = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Forest", AssetRequestMode.ImmediateLoad).Value;
			symbolDiabolist = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Diabolist", AssetRequestMode.ImmediateLoad).Value;
			symbolWaterHoney = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/WaterHoney", AssetRequestMode.ImmediateLoad).Value;
			symbolSkull = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Skull", AssetRequestMode.ImmediateLoad).Value;
			symbolDestroyer = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Destroyer", AssetRequestMode.ImmediateLoad).Value;
			symbolBee = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Bee", AssetRequestMode.ImmediateLoad).Value;
			symbolAmber = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Amber", AssetRequestMode.ImmediateLoad).Value;
			symbolSmite = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Smite", AssetRequestMode.ImmediateLoad).Value;
			symbolCrimtane = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Crimtane", AssetRequestMode.ImmediateLoad).Value;
			symbolRage = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Rage", AssetRequestMode.ImmediateLoad).Value;
			symbolLava = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Lava", AssetRequestMode.ImmediateLoad).Value;
			symbolFeather = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Feather", AssetRequestMode.ImmediateLoad).Value;
			symbolAnklet = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Anklet", AssetRequestMode.ImmediateLoad).Value;
			symbolWyvern = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Wyvern", AssetRequestMode.ImmediateLoad).Value;
			symbolAmethyst = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Amethyst", AssetRequestMode.ImmediateLoad).Value;
			symbolTopaz = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Topaz", AssetRequestMode.ImmediateLoad).Value;
			symbolSapphire = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Sapphire", AssetRequestMode.ImmediateLoad).Value;
			symbolEmerald = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Emerald", AssetRequestMode.ImmediateLoad).Value;
			symbolRuby = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Ruby", AssetRequestMode.ImmediateLoad).Value;

			backgroundColor = Color.White;

			Width.Set(94f, 0f);
			Height.Set(180f, 0f);
			Left.Set(Main.screenWidth - 100f, 0f);
			Top.Set(Main.screenHeight - 130f, 0f);

			Recalculate();
		}

		public override void OnUIScaleChanged()
		{
			Left.Set(Main.screenWidth - 100f, 0f);
			Top.Set(Main.screenHeight - 130f, 0f);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y - 100);
			if (Main.invasionType != 0 || Main.pumpkinMoon || Main.snowMoon)
				point.Y -= 60;
			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);
			if (!Main.playerInventory && !player.dead)
			{
				int buffTimerVal = ((int)(modPlayer.shamanBuffTimer * 60 / 25));
				buffTimerVal += buffTimerVal == 0 ? 1 : 0;
				int buffTimerValRef = modPlayer.shamanBuffTimer * 60 - buffTimerVal;

				if (modPlayer.UIDisplayTimer > 0)
				{
					spriteBatch.Draw(shamanUIMainFrame, new Rectangle(point.X, point.Y, width, height), backgroundColor);

					this.drawnBondEffectBar(1, 100 - modPlayer.shamanFireBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(2, 100 - modPlayer.shamanWaterBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(3, 100 - modPlayer.shamanAirBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(4, 100 - modPlayer.shamanEarthBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(5, 100 - modPlayer.shamanSpiritBondLoading, spriteBatch, point);

					if (modPlayer.shamanFireTimer > 0)
					{
						spriteBatch.Draw(fireSymbolBasic, new Rectangle(point.X + 70, point.Y + 8, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanFireTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 24, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanFire) symbolsList.Add(symbolFire);
						if (modPlayer.shamanIce) symbolsList.Add(symbolIce);
						if (modPlayer.shamanPoison) symbolsList.Add(symbolPoison);
						if (modPlayer.shamanDemonite) symbolsList.Add(symbolDemonite);
						if (modPlayer.shamanVenom) symbolsList.Add(symbolVenom);
						if (modPlayer.shamanSmite) symbolsList.Add(symbolSmite);
						if (modPlayer.shamanRuby) symbolsList.Add(symbolRuby);

						this.drawSymbols(2, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanWaterTimer > 0)
					{
						spriteBatch.Draw(waterSymbolBasic, new Rectangle(point.X + 70, point.Y + 44, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanWaterTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 60, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanWaterHoney) symbolsList.Add(symbolWaterHoney);
						if (modPlayer.shamanSkull) symbolsList.Add(symbolSkull);
						if (modPlayer.shamanDestroyer) symbolsList.Add(symbolDestroyer);
						if (modPlayer.shamanSapphire) symbolsList.Add(symbolSapphire);

						this.drawSymbols(38, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanAirTimer > 0)
					{
						spriteBatch.Draw(airSymbolBasic, new Rectangle(point.X + 70, point.Y + 80, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanAirTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 96, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanFeather) symbolsList.Add(symbolFeather);
						if (modPlayer.shamanHarpyAnklet) symbolsList.Add(symbolAnklet);
						if (modPlayer.shamanDripping) symbolsList.Add(symbolLava);
						if (modPlayer.shamanWyvern) symbolsList.Add(symbolWyvern);
						if (modPlayer.shamanEmerald) symbolsList.Add(symbolEmerald);

						this.drawSymbols(74, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanEarthTimer > 0)
					{
						spriteBatch.Draw(earthSymbolBasic, new Rectangle(point.X + 70, point.Y + 116, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanEarthTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 132, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanHoney) symbolsList.Add(symbolBee);
						if (modPlayer.shamanAmber) symbolsList.Add(symbolAmber);
						if (modPlayer.shamanCrimtane) symbolsList.Add(symbolCrimtane);
						if (modPlayer.shamanRage) symbolsList.Add(symbolRage);
						if (modPlayer.shamanHeavy) symbolsList.Add(symbolHeavy);
						if (modPlayer.shamanForest) symbolsList.Add(symbolForest);
						if (modPlayer.shamanDiabolist) symbolsList.Add(symbolDiabolist);
						if (modPlayer.shamanTopaz) symbolsList.Add(symbolTopaz);

						this.drawSymbols(110, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanSpiritTimer > 0)
					{
						spriteBatch.Draw(spiritSymbolBasic, new Rectangle(point.X + 70, point.Y + 152, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanSpiritTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 168, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanAmethyst) symbolsList.Add(symbolAmethyst);

						this.drawSymbols(146, symbolsList, spriteBatch, point);
					}
				}
			}
		}

		public void drawDuration(int timerRef, int offSetY, int buffTimerVal, SpriteBatch spriteBatch, Point point)
		{
			int resourceOffSet = 0;
			while (timerRef > 0)
			{
				Texture2D barTexture = timerRef - buffTimerVal < 0 ? resourceDurationEnd : resourceDuration;
				spriteBatch.Draw(barTexture, new Rectangle(point.X + 54 - resourceOffSet, point.Y + offSetY, 2, 4), backgroundColor);
				timerRef -= buffTimerVal;
				resourceOffSet += 2;
			}
		}

		public void drawSymbols(int offSetY, List<Texture2D> list, SpriteBatch spriteBatch, Point point)
		{
			int iconsNumber = 0;
			foreach (Texture2D texture in list)
			{
				spriteBatch.Draw(texture, new Rectangle(point.X + 38 - (12 * iconsNumber), point.Y + offSetY, 10, 14), backgroundColor);
				iconsNumber++;
			}
		}

		public void drawnBondEffectBar(byte type, int val, SpriteBatch spriteBatch, Point point)
		{
			int offSetY = 0;
			int resourceOffSet = 0;
			Texture2D[] barTexture = new Texture2D[] { resourceFire, resourceFireEnd };
			Texture2D usedTexture = barTexture[1];

			switch (type)
			{
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

			while (val > 0)
			{
				usedTexture = val - 4 <= 0 ? barTexture[1] : barTexture[0];
				spriteBatch.Draw(usedTexture, new Rectangle(point.X + 54 - resourceOffSet, point.Y + offSetY, 2, 2), backgroundColor);
				val -= 4;
				resourceOffSet += 2;
			}
		}
	}
}