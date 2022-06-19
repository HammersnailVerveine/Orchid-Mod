using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.UIs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Shaman.UI
{
	public class ShamanUIState : OrchidUIState
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

		public override void Draw(SpriteBatch spriteBatch)
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

					this.drawnBondEffectBar(1, 100 - modPlayer.shamanFireBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(2, 100 - modPlayer.shamanWaterBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(3, 100 - modPlayer.shamanAirBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(4, 100 - modPlayer.shamanEarthBondLoading, spriteBatch, point);
					this.drawnBondEffectBar(5, 100 - modPlayer.shamanSpiritBondLoading, spriteBatch, point);

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

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
					=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void Load()
		{
			if (shamanUIMainFrame == null) shamanUIMainFrame = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/MainGrey").Value;

			if (resourceDuration == null) resourceDuration = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/DurationBar").Value;
			if (resourceDurationEnd == null) resourceDurationEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/DurationBarEnd").Value;

			if (resourceFire == null) resourceFire = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBar").Value;
			if (resourceFireEnd == null) resourceFireEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBarEnd").Value;
			if (resourceWater == null) resourceWater = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBar").Value;
			if (resourceWaterEnd == null) resourceWaterEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBarEnd").Value;
			if (resourceAir == null) resourceAir = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBar").Value;
			if (resourceAirEnd == null) resourceAirEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBarEnd").Value;
			if (resourceEarth == null) resourceEarth = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBar").Value;
			if (resourceEarthEnd == null) resourceEarthEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBarEnd").Value;
			if (resourceSpirit == null) resourceSpirit = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBar").Value;
			if (resourceSpiritEnd == null) resourceSpiritEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBarEnd").Value;

			if (FireSymbolBasic == null) FireSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/FireActive").Value;
			if (WaterSymbolBasic == null) WaterSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/WaterActive").Value;
			if (AirSymbolBasic == null) AirSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/AirActive").Value;
			if (EarthSymbolBasic == null) EarthSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/EarthActive").Value;
			if (SpiritSymbolBasic == null) SpiritSymbolBasic = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/SpiritActive").Value;

			if (SymbolFire == null) SymbolFire = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Fire").Value;
			if (SymbolIce == null) SymbolIce = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Ice").Value;
			if (SymbolPoison == null) SymbolPoison = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Poison").Value;
			if (SymbolVenom == null) SymbolVenom = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Venom").Value;
			if (SymbolDemonite == null) SymbolDemonite = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Demonite").Value;
			if (SymbolHeavy == null) SymbolHeavy = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Heavy").Value;
			if (SymbolForest == null) SymbolForest = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Forest").Value;
			if (SymbolDiabolist == null) SymbolDiabolist = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Diabolist").Value;
			if (SymbolWaterHoney == null) SymbolWaterHoney = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/WaterHoney").Value;
			if (SymbolSkull == null) SymbolSkull = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Skull").Value;
			if (SymbolDestroyer == null) SymbolDestroyer = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Destroyer").Value;
			if (SymbolBee == null) SymbolBee = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Bee").Value;
			if (SymbolAmber == null) SymbolAmber = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Amber").Value;
			if (SymbolSmite == null) SymbolSmite = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Smite").Value;
			if (SymbolCrimtane == null) SymbolCrimtane = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Crimtane").Value;
			if (SymbolRage == null) SymbolRage = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Rage").Value;
			if (SymbolLava == null) SymbolLava = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Lava").Value;
			if (SymbolFeather == null) SymbolFeather = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Feather").Value;
			if (SymbolAnklet == null) SymbolAnklet = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Anklet").Value;
			if (SymbolWyvern == null) SymbolWyvern = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Wyvern").Value;
			if (SymbolAmethyst == null) SymbolAmethyst = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Amethyst").Value;
			if (SymbolTopaz == null) SymbolTopaz = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Topaz").Value;
			if (SymbolSapphire == null) SymbolSapphire = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Sapphire").Value;
			if (SymbolEmerald == null) SymbolEmerald = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Emerald").Value;
			if (SymbolRuby == null) SymbolRuby = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Symbols/Ruby").Value;

			Width.Set(94f, 0f);
			Height.Set(180f, 0f);
			Left.Set(Main.screenWidth - 100f, 0f);
			Top.Set(Main.screenHeight - 130f, 0f);
			backgroundColor = Color.White;
			Recalculate(); // ?
		}

		public override void Unload()
		{
			shamanUIMainFrame = null;
			resourceDuration = null;
			resourceDurationEnd = null;
			resourceFire = null;
			resourceFireEnd = null;
			resourceWater = null;
			resourceWaterEnd = null;
			resourceAir = null;
			resourceAirEnd = null;
			resourceEarth = null;
			resourceEarthEnd = null;
			resourceSpirit = null;
			resourceSpiritEnd = null;
			FireSymbolBasic = null;
			WaterSymbolBasic = null;
			AirSymbolBasic = null;
			EarthSymbolBasic = null;
			SpiritSymbolBasic = null;
			SymbolFire = null;
			SymbolIce = null;
			SymbolPoison = null;
			SymbolVenom = null;
			SymbolDemonite = null;
			SymbolHeavy = null;
			SymbolForest = null;
			SymbolDiabolist = null;
			SymbolWaterHoney = null;
			SymbolSkull = null;
			SymbolDestroyer = null;
			SymbolBee = null;
			SymbolAmber = null;
			SymbolSmite = null;
			SymbolCrimtane = null;
			SymbolRage = null;
			SymbolLava = null;
			SymbolFeather = null;
			SymbolAnklet = null;
			SymbolWyvern = null;
			SymbolAmethyst = null;
			SymbolTopaz = null;
			SymbolSapphire = null;
			SymbolEmerald = null;
			SymbolRuby = null;
		}
	}
}