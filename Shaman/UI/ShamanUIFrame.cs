using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Shaman.UI
{
	public class ShamanUIFrame : UIElement
	{
		public Color backgroundColor = Color.White;
		public static Texture2D shamanUIMainFrame;

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
		public static Texture2D SymbolAmethyst;
		public static Texture2D SymbolTopaz;
		public static Texture2D SymbolSapphire;
		public static Texture2D SymbolEmerald;
		public static Texture2D SymbolRuby;

		public enum MouseHoverLocation { NULL, ATTACKLEVEL, ARMORLEVEL, CRITICALLEVEL, REGENERATIONLEVEL, SPEEDLEVEL, EMPOWERMENTDURATION }

		public float usedScale = 0f;

		Player player = Main.player[Main.myPlayer];

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this.usedScale != Main.UIScale)
			{
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
			if (!Main.playerInventory && !player.dead)
			{
				int buffTimerVal = ((int)(modPlayer.shamanBuffTimer * 60 / 25));
				buffTimerVal += buffTimerVal == 0 ? 1 : 0;
				int buffTimerValRef = modPlayer.shamanBuffTimer * 60 - buffTimerVal;

				if (modPlayer.UIDisplayTimer > 0)
				{
					spriteBatch.Draw(shamanUIMainFrame, new Rectangle(point.X, point.Y, width, height), backgroundColor);

					this.drawnBondEffectBar(1, modPlayer.shamanFireBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(2, modPlayer.shamanWaterBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(3, modPlayer.shamanAirBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(4, modPlayer.shamanEarthBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(5, modPlayer.shamanSpiritBondLoading, spriteBatch, point);

					if (modPlayer.shamanFireTimer > 0)
					{
						spriteBatch.Draw(FireSymbolBasic, new Rectangle(point.X + 70, point.Y + 8, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanFireTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 24, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanFire) symbolsList.Add(SymbolFire);
						if (modPlayer.shamanIce) symbolsList.Add(SymbolIce);
						if (modPlayer.shamanPoison) symbolsList.Add(SymbolPoison);
						if (modPlayer.shamanDemonite) symbolsList.Add(SymbolDemonite);
						if (modPlayer.shamanVenom) symbolsList.Add(SymbolVenom);
						if (modPlayer.shamanSmite) symbolsList.Add(SymbolSmite);
						if (modPlayer.shamanRuby) symbolsList.Add(SymbolRuby);

						this.drawSymbols(2, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanWaterTimer > 0)
					{
						spriteBatch.Draw(WaterSymbolBasic, new Rectangle(point.X + 70, point.Y + 44, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanWaterTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 60, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanWaterHoney) symbolsList.Add(SymbolWaterHoney);
						if (modPlayer.shamanSkull) symbolsList.Add(SymbolSkull);
						if (modPlayer.shamanDestroyer) symbolsList.Add(SymbolDestroyer);
						if (modPlayer.shamanSapphire) symbolsList.Add(SymbolSapphire);

						this.drawSymbols(38, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanAirTimer > 0)
					{
						spriteBatch.Draw(AirSymbolBasic, new Rectangle(point.X + 70, point.Y + 80, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanAirTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 96, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanFeather) symbolsList.Add(SymbolFeather);
						if (modPlayer.shamanHarpyAnklet) symbolsList.Add(SymbolAnklet);
						if (modPlayer.shamanDripping) symbolsList.Add(SymbolLava);
						if (modPlayer.shamanWyvern) symbolsList.Add(SymbolWyvern);
						if (modPlayer.shamanEmerald) symbolsList.Add(SymbolEmerald);

						this.drawSymbols(74, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanEarthTimer > 0)
					{
						spriteBatch.Draw(EarthSymbolBasic, new Rectangle(point.X + 70, point.Y + 116, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanEarthTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 132, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanHoney) symbolsList.Add(SymbolBee);
						if (modPlayer.shamanAmber) symbolsList.Add(SymbolAmber);
						if (modPlayer.shamanCrimtane) symbolsList.Add(SymbolCrimtane);
						if (modPlayer.shamanRage) symbolsList.Add(SymbolRage);
						if (modPlayer.shamanHeavy) symbolsList.Add(SymbolHeavy);
						if (modPlayer.shamanForest) symbolsList.Add(SymbolForest);
						if (modPlayer.shamanDiabolist) symbolsList.Add(SymbolDiabolist);
						if (modPlayer.shamanTopaz) symbolsList.Add(SymbolTopaz);

						this.drawSymbols(110, symbolsList, spriteBatch, point);
					}

					if (modPlayer.shamanSpiritTimer > 0)
					{
						spriteBatch.Draw(SpiritSymbolBasic, new Rectangle(point.X + 70, point.Y + 152, width / (94 / 18), height / (180 / 18)), backgroundColor);
						int timerRef = modPlayer.shamanSpiritTimer - buffTimerVal;
						timerRef = timerRef > modPlayer.shamanBuffTimer * 60 - buffTimerVal ? buffTimerValRef - buffTimerVal : timerRef;
						this.drawDuration(timerRef, 168, buffTimerVal, spriteBatch, point);

						List<Texture2D> symbolsList = new List<Texture2D>();
						if (modPlayer.shamanAmethyst) symbolsList.Add(SymbolAmethyst);

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

		public ShamanUIFrame()
		{
			if (shamanUIMainFrame == null) shamanUIMainFrame = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/MainGrey");

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
			if (SymbolAmethyst == null) SymbolAmethyst = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Amethyst");
			if (SymbolTopaz == null) SymbolTopaz = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Topaz");
			if (SymbolSapphire == null) SymbolSapphire = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Sapphire");
			if (SymbolEmerald == null) SymbolEmerald = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Emerald");
			if (SymbolRuby == null) SymbolRuby = ModContent.GetTexture("OrchidMod/Shaman/UI/ModUITextures/Symbols/Ruby");
		}
	}
}