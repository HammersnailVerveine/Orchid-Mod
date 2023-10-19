using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.UIs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Content.Shaman.UI
{
	public class ShamanCharacterUIState : OrchidUIState
	{
		private Texture2D SymbolFire;
		private Texture2D SymbolWater;
		private Texture2D SymbolWind;
		private Texture2D SymbolEarth;
		private Texture2D SymbolSpirit;
		private Texture2D BondBar;

		private Color backgroundColor;
		private int[] shamanTimers;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			SymbolFire = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/Disabled", AssetRequestMode.ImmediateLoad).Value;
			SymbolWater = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/Disabled", AssetRequestMode.ImmediateLoad).Value;
			SymbolWind = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/Disabled", AssetRequestMode.ImmediateLoad).Value;
			SymbolEarth = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/Disabled", AssetRequestMode.ImmediateLoad).Value;
			SymbolSpirit = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/Disabled", AssetRequestMode.ImmediateLoad).Value;
			BondBar = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/BondBar", AssetRequestMode.ImmediateLoad).Value;

			shamanTimers = new int[] { 0, 0, 0, 0, 0 };
			backgroundColor = Color.White;

			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(Main.screenWidth / 2, 0f);
			Top.Set(Main.screenHeight / 2, 0f);

			Recalculate();
		}

		public override void OnResolutionChanged(int width, int height)
		{
			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(width / 2, 0f);
			Top.Set(height / 2, 0f);

			Recalculate();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.LocalPlayer;

			Vector2 vector = (player.position + new Vector2(player.width * 0.5f, player.gravDir > 0 ? player.height - 10 + player.gfxOffY : 10 + player.gfxOffY)).Floor();
			vector = Vector2.Transform(vector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix * Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;

			this.Left.Set(vector.X, 0f);
			this.Top.Set(vector.Y, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (!player.dead)
			{
				if (modPlayer.UIDisplayTimer > 0)
				{

					int buffTimer = (60 * modPlayer.ShamanBondDuration) / 3;
					int timerFire = modPlayer.ShamanFireBondReleased ? (int)modPlayer.ShamanFireBond : 0;
					int timerWater = modPlayer.ShamanWaterBondReleased ? (int)modPlayer.ShamanFireBond : 0;
					int timerAir = modPlayer.ShamanAirBondReleased ? (int)modPlayer.ShamanFireBond : 0;
					int timerEarth = modPlayer.ShamanEarthBondReleased ? (int)modPlayer.ShamanFireBond : 0;
					int timerSpirit = modPlayer.ShamanSpiritBondReleased ? (int)modPlayer.ShamanFireBond : 0;
					int[] currentTimers = new int[] { 0, 0, 0, 0, 0 };

					currentTimers[0] = timerFire > buffTimer * 2 ? 3 : timerFire > buffTimer ? 2 : timerFire > 0 ? 1 : 0;
					currentTimers[1] = timerWater > buffTimer * 2 ? 3 : timerWater > buffTimer ? 2 : timerWater > 0 ? 1 : 0;
					currentTimers[2] = timerAir > buffTimer * 2 ? 3 : timerAir > buffTimer ? 2 : timerAir > 0 ? 1 : 0;
					currentTimers[3] = timerEarth > buffTimer * 2 ? 3 : timerEarth > buffTimer ? 2 : timerEarth > 0 ? 1 : 0;
					currentTimers[4] = timerSpirit > buffTimer * 2 ? 3 : timerSpirit > buffTimer ? 2 : timerSpirit > 0 ? 1 : 0;

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
								SymbolFire = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/" + str, AssetRequestMode.ImmediateLoad).Value;
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
								SymbolWater = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/" + str, AssetRequestMode.ImmediateLoad).Value;
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
								SymbolWind = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/" + str, AssetRequestMode.ImmediateLoad).Value;
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
								SymbolEarth = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/" + str, AssetRequestMode.ImmediateLoad).Value;
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
								SymbolSpirit = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Character/" + str, AssetRequestMode.ImmediateLoad).Value;
							}
						}
					}

					int drawSize = 12;
					int offSetY = (modPlayer.modPlayer.modPlayerAlchemist.alchemistPotencyDisplayTimer > 0 || modPlayer.modPlayer.modPlayerGuardian.guardianDisplayUI > 0) ? 60 : 20;
						
					spriteBatch.Draw(SymbolFire, new Rectangle(point.X - 38, point.Y + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(SymbolWater, new Rectangle(point.X - 24, point.Y + 10 + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(SymbolWind, new Rectangle(point.X - 6, point.Y + 14 + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(SymbolEarth, new Rectangle(point.X + 12, point.Y + 10 + offSetY, drawSize, drawSize), backgroundColor);
					spriteBatch.Draw(SymbolSpirit, new Rectangle(point.X + 26, point.Y + offSetY, drawSize, drawSize), backgroundColor);

					Item heldItem = player.HeldItem;
					if (heldItem.type != ItemID.None)
					{
						if (heldItem.ModItem is OrchidModShamanItem shamanHeldItem)
						{
							Color ColorInside = new Color(20, 16, 23);
							Color ColorBorder = new Color(59, 45, 58);
							int segments = 50;
							float scale = 100f / segments;

							for (int i = 0; i < segments; i++)
							{
								float angle = MathHelper.ToRadians(50 - i * scale);
								Vector2 position = new Vector2(point.X, point.Y) + new Vector2(0f, 60f).RotatedBy(angle);
								spriteBatch.Draw(BondBar, position, null, ColorInside, angle, BondBar.Size() * 0.5f, 1.75f, SpriteEffects.None, 0f);
							}

							for (int i = 0; i < segments; i ++)
							{
								float angle = MathHelper.ToRadians(50 - i * scale);
								Vector2 position = new Vector2(point.X, point.Y) + new Vector2(0f, 60f).RotatedBy(angle);
								ShamanElement element = (ShamanElement)shamanHeldItem.Element;
								float bondValue = modPlayer.GetShamanicBondValue(element);
								if (modPlayer.IsShamanicBondReleased(element)) {
									bondValue /= modPlayer.ShamanBondDuration * 6000f;
									if (bondValue > 100) bondValue = 100;
								}
								Color color = bondValue > i * scale ? ShamanElementUtils.GetColor(element) : ColorBorder;
								spriteBatch.Draw(BondBar, position, null, color, angle, BondBar.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
							}
						}
					}
				}
			}
		}
	}
}