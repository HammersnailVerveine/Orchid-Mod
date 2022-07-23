using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.UI
{
	public partial class AlchemistSelectUI
	{
		private static class Fields
		{
			// Textures

			public static Asset<Texture2D> PotionBackgroundTexture;
			public static Asset<Texture2D> PotionBorderTexture;
			public static Asset<Texture2D> PotionElementTexture;
			public static Asset<Texture2D> PotionMarkedTexture;
			public static Asset<Texture2D> QuickMixBackgroundTexture;
			public static Asset<Texture2D> TooltipsBackgroundTexture;

			// Misc

			public static InterfaceTimer InitTimer;
			public static ITooltipsStyle TooltipsStyle;

			// ...

			public static void Load()
			{
				PotionBackgroundTexture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectUIPotionBackground");
				PotionBorderTexture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectUIPotionBorder");
				PotionElementTexture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectUIPotionElement");
				PotionMarkedTexture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectUIPotionMarked");
				QuickMixBackgroundTexture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectUIQuickMixBackground");
				TooltipsBackgroundTexture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectUITooltipsBackground");

				InitTimer = 10;
				TooltipsStyle = new AlchemistSelectUITooltipsStyle();
			}

			public static void Unload()
			{
				PotionBackgroundTexture = null;
				PotionBorderTexture = null;
				PotionElementTexture = null;
				PotionMarkedTexture = null;
				QuickMixBackgroundTexture = null;
				TooltipsBackgroundTexture = null;

				InitTimer = null;
				TooltipsStyle = null;
			}
		}
	}
}
