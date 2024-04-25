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
		private Texture2D SymbolFireEmpty;
		private Texture2D SymbolWaterEmpty;
		private Texture2D SymbolWindEmpty;
		private Texture2D SymbolEarthEmpty;
		private Texture2D SymbolSpiritEmpty;
		private Texture2D BondBar;

		private Color backgroundColor;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			SymbolFire = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Fire", AssetRequestMode.ImmediateLoad).Value;
			SymbolWater = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Water", AssetRequestMode.ImmediateLoad).Value;
			SymbolWind = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Air", AssetRequestMode.ImmediateLoad).Value;
			SymbolEarth = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Earth", AssetRequestMode.ImmediateLoad).Value;
			SymbolSpirit = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/Spirit", AssetRequestMode.ImmediateLoad).Value;
			SymbolFireEmpty = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/FireEmpty", AssetRequestMode.ImmediateLoad).Value;
			SymbolWaterEmpty = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/WaterEmpty", AssetRequestMode.ImmediateLoad).Value;
			SymbolWindEmpty = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/AirEmpty", AssetRequestMode.ImmediateLoad).Value;
			SymbolEarthEmpty = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/EarthEmpty", AssetRequestMode.ImmediateLoad).Value;
			SymbolSpiritEmpty = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/SpiritEmpty", AssetRequestMode.ImmediateLoad).Value;
			BondBar = ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/UI/ModUITextures/BondBar", AssetRequestMode.ImmediateLoad).Value;

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
			vector = Vector2.Transform(vector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix * Main.GameViewMatrix.ZoomMatrix);

			this.Left.Set(vector.X, 0f);
			this.Top.Set(vector.Y, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (!player.dead && (modPlayer.HasAnyBondLoaded() || player.HeldItem.ModItem is OrchidModShamanItem))
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

				int offSetY = (modPlayer.modPlayer.modPlayerAlchemist.alchemistPotencyDisplayTimer > 0 || modPlayer.modPlayer.modPlayerGuardian.guardianDisplayUI > 0) ? 60 : 20;

				spriteBatch.Draw(SymbolWaterEmpty, new Rectangle(point.X - 39, point.Y + offSetY, 14, 16), backgroundColor);
				spriteBatch.Draw(SymbolFireEmpty, new Rectangle(point.X - 25, point.Y + 10 + offSetY, 14, 16), backgroundColor);
				spriteBatch.Draw(SymbolSpiritEmpty, new Rectangle(point.X - 6, point.Y + 14 + offSetY, 16, 16), backgroundColor);
				spriteBatch.Draw(SymbolWindEmpty, new Rectangle(point.X + 13, point.Y + 10 + offSetY, 14, 16), backgroundColor);
				spriteBatch.Draw(SymbolEarthEmpty, new Rectangle(point.X + 27, point.Y + offSetY, 14, 16), backgroundColor);

				if (modPlayer.ShamanWaterBondReleased)
				{
					float colormult = 1f;
					if (modPlayer.ShamanWaterBond < 300) colormult = Math.Abs((float)Math.Sin(modPlayer.ShamanWaterBond * 0.0365));
					spriteBatch.Draw(SymbolWater, new Rectangle(point.X - 39, point.Y + offSetY, 14, 16), backgroundColor * colormult);
				}

				if (modPlayer.ShamanFireBondReleased)
				{
					float colormult = 1f;
					if (modPlayer.ShamanFireBond < 300) colormult = Math.Abs((float)Math.Sin(modPlayer.ShamanFireBond * 0.0365));
					spriteBatch.Draw(SymbolFire, new Rectangle(point.X - 25, point.Y + 10 + offSetY, 14, 16), backgroundColor * colormult);
				}

				if (modPlayer.ShamanSpiritBondReleased)
				{
					float colormult = 1f;
					if (modPlayer.ShamanSpiritBond < 300) colormult = Math.Abs((float)Math.Sin(modPlayer.ShamanSpiritBond * 0.0365));
					spriteBatch.Draw(SymbolSpirit, new Rectangle(point.X - 6, point.Y + 14 + offSetY, 16, 16), backgroundColor * colormult);
				}

				if (modPlayer.ShamanAirBondReleased)
				{
					float colormult = 1f;
					if (modPlayer.ShamanAirBond < 300) colormult = Math.Abs((float)Math.Sin(modPlayer.ShamanAirBond * 0.0365));
					spriteBatch.Draw(SymbolWind, new Rectangle(point.X + 13, point.Y + 10 + offSetY, 14, 16), backgroundColor * colormult);
				}

				if (modPlayer.ShamanEarthBondReleased)
				{
					float colormult = 1f;
					if (modPlayer.ShamanEarthBond < 300) colormult = Math.Abs((float)Math.Sin(modPlayer.ShamanEarthBond * 0.0365));
					spriteBatch.Draw(SymbolEarth, new Rectangle(point.X + 27, point.Y + offSetY, 14, 16), backgroundColor * colormult);
				}


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
							spriteBatch.Draw(BondBar, position, null, ColorInside, angle, BondBar.Size() * 0.5f, 1.5f, SpriteEffects.None, 0f);
						}

						for (int i = 0; i < segments; i++)
						{
							float angle = MathHelper.ToRadians(50 - i * scale);
							Vector2 position = new Vector2(point.X, point.Y) + new Vector2(0f, 60f).RotatedBy(angle);
							ShamanElement element = (ShamanElement)shamanHeldItem.Element;
							float bondValue = modPlayer.GetShamanicBondValue(element);
							if (modPlayer.IsShamanicBondReleased(element))
							{
								bondValue = bondValue / modPlayer.ShamanBondDuration * (5 / 3f);
								if (bondValue > 100) bondValue = 100;
							}
							Color color = bondValue > i * scale ? ShamanElementUtils.GetColor(element) : ColorBorder;
							spriteBatch.Draw(BondBar, position, null, color, angle, BondBar.Size() * 0.5f, 0.8f, SpriteEffects.None, 0f);
						}
					}
				}

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			}
		}
	}
}