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
	public class ShamanCharacterUIState : OrchidUIState
	{
		public Color backgroundColor = Color.White;
		public static Texture2D symbolAttack;
		public static Texture2D symbolDefense;
		public static Texture2D symbolCritical;
		public static Texture2D symbolRegeneration;
		public static Texture2D symbolSpeed;
		public static Texture2D symbolWeak;
		public static Texture2D fireLoaded;
		public static Texture2D waterLoaded;
		public static Texture2D airLoaded;
		public static Texture2D earthLoaded;
		public static Texture2D spiritLoaded;
		public static Texture2D resource;
		public static Texture2D resourceEnd;
		public static Texture2D resourceBar;
		public int[] shamanTimers;

		public override void Draw(SpriteBatch spriteBatch)
		{
			Player player = Main.LocalPlayer;

			Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.gfxOffY + player.gravDir > 0 ? player.height - 10 + player.gfxOffY : 10 + player.gfxOffY)).Floor();
			position = Vector2.Transform(position - Main.screenPosition, Main.GameViewMatrix.EffectMatrix * Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;

			this.Left.Set(position.X, 0f);
			this.Top.Set(position.Y, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (!player.dead)
			{
				if (modPlayer.UIDisplayTimer > 0)
				{

					int buffTimer = (60 * modPlayer.shamanBuffTimer) / 3;
					int attackTimer = modPlayer.shamanFireTimer;
					int defenseTimer = modPlayer.shamanWaterTimer;
					int criticalTimer = modPlayer.shamanAirTimer;
					int regenTimer = modPlayer.shamanEarthTimer;
					int speedTimer = modPlayer.shamanSpiritTimer;
					int[] currentTimers = new int[] { 0, 0, 0, 0, 0 };
					int lastElement = 0;

					currentTimers[0] = attackTimer > buffTimer * 2 ? 3 : attackTimer > buffTimer ? 2 : attackTimer > 0 ? 1 : 0;
					currentTimers[1] = defenseTimer > buffTimer * 2 ? 3 : defenseTimer > buffTimer ? 2 : defenseTimer > 0 ? 1 : 0;
					currentTimers[2] = criticalTimer > buffTimer * 2 ? 3 : criticalTimer > buffTimer ? 2 : criticalTimer > 0 ? 1 : 0;
					currentTimers[3] = regenTimer > buffTimer * 2 ? 3 : regenTimer > buffTimer ? 2 : regenTimer > 0 ? 1 : 0;
					currentTimers[4] = speedTimer > buffTimer * 2 ? 3 : speedTimer > buffTimer ? 2 : speedTimer > 0 ? 1 : 0;

					for (int i = 0; i < 5; i++)
					{
						if (currentTimers[i] != this.shamanTimers[i])
						{
							String str = "";
							this.shamanTimers[i] = currentTimers[i];
							if (i == 0)
							{
								switch (currentTimers[i])
								{
									case 1:
										str = "Fire3";
										break;
									case 2:
										str = "Fire2";
										break;
									case 3:
										str = "Fire1";
										break;
									default:
										str = "Disabled";
										break;
								}
								symbolAttack = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/" + str).Value;
							}
							else if (i == 1)
							{
								switch (currentTimers[i])
								{
									case 1:
										str = "Water3";
										break;
									case 2:
										str = "Water2";
										break;
									case 3:
										str = "Water1";
										break;
									default:
										str = "Disabled";
										break;
								}
								symbolDefense = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/" + str).Value;
							}
							else if (i == 2)
							{
								switch (currentTimers[i])
								{
									case 1:
										str = "Wind3";
										break;
									case 2:
										str = "Wind2";
										break;
									case 3:
										str = "Wind1";
										break;
									default:
										str = "Disabled";
										break;
								}
								symbolCritical = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/" + str).Value;
							}
							else if (i == 3)
							{
								switch (currentTimers[i])
								{
									case 1:
										str = "Earth3";
										break;
									case 2:
										str = "Earth2";
										break;
									case 3:
										str = "Earth1";
										break;
									default:
										str = "Disabled";
										break;
								}
								symbolRegeneration = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/" + str).Value;
							}
							else if (i == 4)
							{
								switch (currentTimers[i])
								{
									case 1:
										str = "Spirit3";
										break;
									case 2:
										str = "Spirit2";
										break;
									case 3:
										str = "Spirit1";
										break;
									default:
										str = "Disabled";
										break;
								}
								symbolSpeed = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/" + str).Value;
							}
						}
					}

					int drawSize = 12;
					int offSetY = modPlayer.alchemistPotencyDisplayTimer > 0 ? 50 : 20;

					spriteBatch.Draw(symbolAttack, new Rectangle(point.X - 38, point.Y + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(symbolDefense, new Rectangle(point.X - 24, point.Y + 10 + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(symbolCritical, new Rectangle(point.X - 6, point.Y + 14 + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(symbolRegeneration, new Rectangle(point.X + 12, point.Y + 10 + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(symbolSpeed, new Rectangle(point.X + 26, point.Y + offSetY, drawSize, drawSize), backgroundColor);

					offSetY += drawSize + 2;

					if (modPlayer.shamanPollFireMax)
						spriteBatch.Draw(fireLoaded, new Rectangle(point.X - 38, point.Y + offSetY, drawSize, drawSize - 4), backgroundColor);

					if (modPlayer.shamanPollWaterMax)
						spriteBatch.Draw(waterLoaded, new Rectangle(point.X - 24, point.Y + 10 + offSetY, drawSize, drawSize - 4), backgroundColor);

					if (modPlayer.shamanPollAirMax)
						spriteBatch.Draw(airLoaded, new Rectangle(point.X - 6, point.Y + 14 + offSetY, drawSize, drawSize - 4), backgroundColor);

					if (modPlayer.shamanPollEarthMax)
						spriteBatch.Draw(earthLoaded, new Rectangle(point.X + 12, point.Y + 10 + offSetY, drawSize, drawSize - 4), backgroundColor);

					if (modPlayer.shamanPollSpiritMax)
						spriteBatch.Draw(spiritLoaded, new Rectangle(point.X + 26, point.Y + offSetY, drawSize, drawSize - 4), backgroundColor);

					Item heldItem = player.HeldItem;
					if (heldItem.type != 0)
					{
						OrchidModGlobalItem orchidItem = heldItem.GetGlobalItem<OrchidModGlobalItem>();
						if (orchidItem.shamanWeapon)
						{
							spriteBatch.Draw(resourceBar, new Rectangle(point.X - (resourceBar.Width / 2), point.Y + offSetY + 20, resourceBar.Width, resourceBar.Height), backgroundColor);
							Point newPoint = new Point(point.X - (resourceBar.Width / 2) + 6, point.Y + offSetY + 20 + 6);
							int val = 0;

							switch (orchidItem.shamanWeaponElement)
							{
								case 1:
									if (lastElement != 1)
									{
										resource = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBar").Value;
										resourceEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBarEnd").Value;
										lastElement = 1;
									}
									val = 100 - modPlayer.shamanFireBondLoading;
									this.drawnBondEffectBar(val, spriteBatch, newPoint);
									this.drawWeakSymbol(val, spriteBatch, newPoint);
									break;
								case 2:
									if (lastElement != 2)
									{
										resource = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBar").Value;
										resourceEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/WaterBarEnd").Value;
										lastElement = 2;
									}
									val = 100 - modPlayer.shamanWaterBondLoading;
									this.drawnBondEffectBar(val, spriteBatch, newPoint);
									this.drawWeakSymbol(val, spriteBatch, newPoint);
									break;
								case 3:
									if (lastElement != 3)
									{
										resource = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBar").Value;
										resourceEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/AirBarEnd").Value;
										lastElement = 3;
									}
									val = 100 - modPlayer.shamanAirBondLoading;
									this.drawnBondEffectBar(val, spriteBatch, newPoint);
									this.drawWeakSymbol(val, spriteBatch, newPoint);
									break;
								case 4:
									if (lastElement != 4)
									{
										resource = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBar").Value;
										resourceEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/EarthBarEnd").Value;
										lastElement = 4;
									}
									val = 100 - modPlayer.shamanEarthBondLoading;
									this.drawnBondEffectBar(val, spriteBatch, newPoint);
									this.drawWeakSymbol(val, spriteBatch, newPoint);
									break;
								case 5:
									if (lastElement != 5)
									{
										resource = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBar").Value;
										resourceEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/SpiritBarEnd").Value;
										lastElement = 5;
									}
									val = 100 - modPlayer.shamanSpiritBondLoading;
									this.drawnBondEffectBar(val, spriteBatch, newPoint);
									this.drawWeakSymbol(val, spriteBatch, newPoint);
									break;
								default:
									break;
							}
						}
					}
				}
			}

			//base.Update(gameTime);
		}

		public void drawnBondEffectBar(int val, SpriteBatch spriteBatch, Point point)
		{
			int resourceOffSet = 0;
			while (val > 0)
			{
				Texture2D usedTexture = val - 4 <= 0 ? resourceEnd : resource;
				spriteBatch.Draw(usedTexture, new Rectangle(point.X + resourceOffSet, point.Y, 2, 2), backgroundColor);
				spriteBatch.Draw(usedTexture, new Rectangle(point.X + resourceOffSet, point.Y + 2, 2, 2), backgroundColor);
				val -= 4;
				resourceOffSet += 2;
			}
		}
		
		public void drawWeakSymbol(int val, SpriteBatch spriteBatch, Point point)
		{
			if (val > 0) {
				return;
			} else {
				spriteBatch.Draw(symbolWeak, new Rectangle(point.X - 28, point.Y - 10, symbolWeak.Width, symbolWeak.Height), backgroundColor);
			}
		}

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
					=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void Load()
		{
			if (symbolAttack == null) symbolAttack = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/Disabled").Value;
			if (symbolDefense == null) symbolDefense = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/Disabled").Value;
			if (symbolCritical == null) symbolCritical = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/Disabled").Value;
			if (symbolRegeneration == null) symbolRegeneration = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/Disabled").Value;
			if (symbolWeak == null) symbolWeak = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/SymbolWeak").Value;
			if (symbolSpeed == null) symbolSpeed = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/Disabled").Value;
			if (fireLoaded == null) fireLoaded = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/FireLoaded").Value;
			if (waterLoaded == null) waterLoaded = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/WaterLoaded").Value;
			if (airLoaded == null) airLoaded = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/WindLoaded").Value;
			if (earthLoaded == null) earthLoaded = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/EarthLoaded").Value;
			if (spiritLoaded == null) spiritLoaded = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/SpiritLoaded").Value;
			if (resource == null) resource = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBar").Value;
			if (resourceEnd == null) resourceEnd = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/ModUIMain/FireBarEnd").Value;
			if (resourceBar == null) resourceBar = ModContent.Request<Texture2D>("OrchidMod/Shaman/UI/ModUITextures/Character/ResourceBar").Value;

			this.shamanTimers = new int[] { 0, 0, 0, 0, 0 };

			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(Main.screenWidth / 2, 0f);
			Top.Set(Main.screenHeight / 2, 0f);
			backgroundColor = Color.White;
			Recalculate(); // ?
		}

		public override void Unload()
		{
			symbolAttack = null;
			symbolDefense = null;
			symbolCritical = null;
			symbolRegeneration = null;
			symbolWeak = null;
			symbolSpeed = null;
			fireLoaded = null;
			waterLoaded = null;
			airLoaded = null;
			earthLoaded = null;
			spiritLoaded = null;
			resource = null;
			resourceEnd = null;
			resourceBar = null;
			shamanTimers = null;
		}
	}
}