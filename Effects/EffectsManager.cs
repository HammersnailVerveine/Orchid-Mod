using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Effects
{
	public static class EffectsManager
	{
		public static void Load(Mod mod)
		{
			CloudNoiseTexture = ModContent.GetTexture("OrchidMod/Effects/Textures/CloudNoise");
			LensFlareTexture = ModContent.GetTexture("OrchidMod/Effects/Textures/LensFlare");
			PerlinNoiseTexture = ModContent.GetTexture("Terraria/Misc/Perlin");
			RadialGradientTexture = ModContent.GetTexture("OrchidMod/Effects/Textures/RadialGradient");
			WhiteCircleTexture = ModContent.GetTexture("OrchidMod/Effects/Textures/WhiteCircle");

			ExtraTextures = new Texture2D[4];
			for (int i = 0; i < ExtraTextures.Length; i++) ExtraTextures[i] = ModContent.GetTexture("OrchidMod/Effects/Textures/Extra_" + i);

			TrailTextures = new Texture2D[2];
			for (int i = 0; i < TrailTextures.Length; i++) TrailTextures[i] = ModContent.GetTexture("OrchidMod/Effects/Textures/Trail_" + i);

			if (!Main.dedServ)
			{
				ShroomiteZoneEffect = mod.GetEffect("Effects/ShroomiteScepter");
				ShroomiteZoneEffect.Parameters["perlinTexture"].SetValue(PerlinNoiseTexture);

				WyvernMorayEffect = mod.GetEffect("Effects/WyvernMoray");
				WyvernMorayEffect.Parameters["texture0"].SetValue(TrailTextures[1]);
				WyvernMorayEffect.Parameters["texture1"].SetValue(ExtraTextures[2]);

				WyvernMorayLingeringEffect = mod.GetEffect("Effects/WyvernMorayLingering");
				WyvernMorayLingeringEffect.Parameters["texture1"].SetValue(CloudNoiseTexture);
			}
		}

		public static void Unload()
		{
			CloudNoiseTexture = null;
			LensFlareTexture = null;
			PerlinNoiseTexture = null;
			RadialGradientTexture = null;
			WhiteCircleTexture = null;

			ExtraTextures = null;
			TrailTextures = null;

			ShroomiteZoneEffect = null;
			WyvernMorayEffect = null;
			WyvernMorayLingeringEffect = null;
		}

		// Effects
		public static Effect ShroomiteZoneEffect { get; private set; }
		public static Effect WyvernMorayEffect { get; private set; }
		public static Effect WyvernMorayLingeringEffect { get; private set; }

		// Textures
		public static Texture2D CloudNoiseTexture { get; private set; }
		public static Texture2D LensFlareTexture { get; private set; }
		public static Texture2D PerlinNoiseTexture { get; private set; }
		public static Texture2D RadialGradientTexture { get; private set; }
		public static Texture2D WhiteCircleTexture { get; private set; }

		public static Texture2D[] ExtraTextures { get; private set; }
		public static Texture2D[] TrailTextures { get; private set; }
	}
}
