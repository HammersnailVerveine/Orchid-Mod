using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistUIFrame : UIElement
	{
		public Color backgroundColor = Color.White;
		public static Texture2D ressourceBottom;
		public static Texture2D ressourceTop;
		public static Texture2D ressourceFull;
		public static Texture2D ressourceFullTop;
		public static Texture2D ressourceFullBorder;
		public static Texture2D ressourceEmpty;
		public static Texture2D reactionCooldown;
		public static Texture2D reactionCooldownLiquid;

		public static Texture2D symbolWater;
		public static Texture2D symbolFire;
		public static Texture2D symbolNature;
		public static Texture2D symbolAir;
		public static Texture2D symbolLight;
		public static Texture2D symbolDark;

		Player player = Main.player[Main.myPlayer];

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Player player = Main.LocalPlayer;

			Vector2 vector = (player.position + new Vector2(player.width * 0.5f, player.gravDir > 0 ? player.height - 10 + player.gfxOffY : 10 + player.gfxOffY)).Floor();
			vector = Vector2.Transform(vector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix * Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;

			this.Left.Set(vector.X - 60f, 0f);
			this.Top.Set(vector.Y - 58f, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y - 100);
			// if (Main.invasionType != 0)
			// point.Y -= 60;
			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			bool[] elements = modPlayer.alchemistElements;

			if (!player.dead)
			{
				if (modPlayer.alchemistPotencyDisplayTimer > 0)
				{

					int drawHeight = 176;
					int symbolHeight = 170;

					int textureWidth = 16;
					int textureHeight = 4;
					int symbolSize = 12;

					Color liquidColor = new Color(modPlayer.alchemistColorRDisplay, modPlayer.alchemistColorGDisplay, modPlayer.alchemistColorBDisplay);

					if (player.FindBuffIndex(BuffType<Alchemist.Buffs.Debuffs.ReactionCooldown>()) > -1)
					{
						spriteBatch.Draw(reactionCooldown, new Rectangle(point.X, point.Y + drawHeight + 6, 20, 22), backgroundColor);
						spriteBatch.Draw(reactionCooldownLiquid, new Rectangle(point.X, point.Y + drawHeight + 6, 20, 22), liquidColor);
					}

					spriteBatch.Draw(ressourceBottom, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight), backgroundColor);
					drawHeight = this.incrementDrawHeight(drawHeight);

					for (int i = 0; i < modPlayer.alchemistPotency - 1; i++)
					{
						spriteBatch.Draw(ressourceFull, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight), liquidColor);
						spriteBatch.Draw(ressourceFullBorder, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight), backgroundColor);
						drawHeight = this.incrementDrawHeight(drawHeight);
					}

					spriteBatch.Draw(ressourceFullTop, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight), liquidColor);
					spriteBatch.Draw(ressourceFullBorder, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight), backgroundColor);
					drawHeight = this.incrementDrawHeight(drawHeight);

					for (int i = 0; i < modPlayer.alchemistPotencyMax - modPlayer.alchemistPotency; i++)
					{
						spriteBatch.Draw(ressourceEmpty, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight), backgroundColor);
						drawHeight = this.incrementDrawHeight(drawHeight);
					}

					drawHeight = this.incrementDrawHeight(drawHeight);
					spriteBatch.Draw(ressourceTop, new Rectangle(point.X, point.Y + drawHeight, textureWidth, textureHeight * 2), backgroundColor);

					if (elements[0])
					{
						spriteBatch.Draw(symbolWater, new Rectangle(point.X + textureWidth + 2, point.Y + symbolHeight, symbolSize, symbolSize), backgroundColor);
						symbolHeight = this.incrementSymbolDrawHeight(symbolHeight);
					}

					if (elements[1])
					{
						spriteBatch.Draw(symbolFire, new Rectangle(point.X + textureWidth + 2, point.Y + symbolHeight, symbolSize, symbolSize), backgroundColor);
						symbolHeight = this.incrementSymbolDrawHeight(symbolHeight);
					}

					if (elements[2])
					{
						spriteBatch.Draw(symbolNature, new Rectangle(point.X + textureWidth + 2, point.Y + symbolHeight, symbolSize, symbolSize), backgroundColor);
						symbolHeight = this.incrementSymbolDrawHeight(symbolHeight);
					}

					if (elements[3])
					{
						spriteBatch.Draw(symbolAir, new Rectangle(point.X + textureWidth + 2, point.Y + symbolHeight, symbolSize, symbolSize), backgroundColor);
						symbolHeight = this.incrementSymbolDrawHeight(symbolHeight);
					}

					if (elements[4])
					{
						spriteBatch.Draw(symbolLight, new Rectangle(point.X + textureWidth + 2, point.Y + symbolHeight, symbolSize, symbolSize), backgroundColor);
						symbolHeight = this.incrementSymbolDrawHeight(symbolHeight);
					}

					if (elements[5])
					{
						spriteBatch.Draw(symbolDark, new Rectangle(point.X + textureWidth + 2, point.Y + symbolHeight, symbolSize, symbolSize), backgroundColor);
					}
				}
			}
		}

		public int incrementDrawHeight(int drawHeight)
		{
			return (drawHeight - 4);
		}

		public int incrementSymbolDrawHeight(int drawHeight)
		{
			return (drawHeight - 14);
		}

		public AlchemistUIFrame()
		{
			if (ressourceBottom == null) ressourceBottom = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUIBottom");
			if (ressourceTop == null) ressourceTop = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUITop");
			if (ressourceFull == null) ressourceFull = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUIFull");
			if (ressourceFullTop == null) ressourceFullTop = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUIFullTop");
			if (ressourceFullBorder == null) ressourceFullBorder = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUIFullBorder");
			if (ressourceEmpty == null) ressourceEmpty = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUIEmpty");
			if (reactionCooldown == null) reactionCooldown = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUICooldown");
			if (reactionCooldownLiquid == null) reactionCooldownLiquid = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUICooldownLiquid");

			if (symbolWater == null) symbolWater = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUISymbolWater");
			if (symbolFire == null) symbolFire = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUISymbolFire");
			if (symbolNature == null) symbolNature = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUISymbolNature");
			if (symbolAir == null) symbolAir = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUISymbolAir");
			if (symbolLight == null) symbolLight = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUISymbolLight");
			if (symbolDark == null) symbolDark = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistUISymbolDark");

		}
	}
}