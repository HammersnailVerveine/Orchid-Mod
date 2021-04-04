using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Effects
{
	public static class EffectsManager
	{
		public static void Load()
		{
			PerlinNoiseTexture = ModContent.GetTexture("OrchidMod/Effects/Textures/Perlin");
			RadialGradientTexture = ModContent.GetTexture("OrchidMod/Effects/Textures/RadialGradient");

			if (Main.netMode != NetmodeID.Server)
			{
				ShroomiteZoneEffect = OrchidMod.Instance.GetEffect("Effects/ShroomiteScepter");
				ShroomiteZoneEffect.Parameters["perlinTexture"].SetValue(PerlinNoiseTexture);
			}
		}

		public static void Unload()
		{
			PerlinNoiseTexture = null;
			RadialGradientTexture = null;

			ShroomiteZoneEffect = null;
		}

		public static void SetSpriteBatchEffectSettings(SpriteBatch spriteBatch, Effect effect = null, BlendState blendState = null)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, blendState ?? BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, effect, Main.GameViewMatrix.TransformationMatrix);
		}

		public static void SetSpriteBatchVanillaSettings(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
		}

		// Effects
		public static Effect ShroomiteZoneEffect { get; private set; }

		// Textures
		public static Texture2D PerlinNoiseTexture { get; private set; }
		public static Texture2D RadialGradientTexture { get; private set; }
	}
}
