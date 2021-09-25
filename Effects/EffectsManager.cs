using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Effects
{
	public static class EffectsManager
	{
		public static void Load(Mod mod)
		{
			if (!Main.dedServ)
			{
				ShroomiteZoneEffect = mod.GetEffect("Effects/ShroomiteScepter");
				ShroomiteZoneEffect.Parameters["perlinTexture"].SetValue(ModContent.GetTexture("Terraria/Misc/Perlin"));

				WyvernMorayEffect = mod.GetEffect("Effects/WyvernMoray");
				WyvernMorayEffect.Parameters["texture0"].SetValue(OrchidHelper.GetExtraTexture(5));
				WyvernMorayEffect.Parameters["texture1"].SetValue(OrchidHelper.GetExtraTexture(2));

				WyvernMorayLingeringEffect = mod.GetEffect("Effects/WyvernMorayLingering");
				WyvernMorayLingeringEffect.Parameters["texture1"].SetValue(OrchidHelper.GetExtraTexture(10));
			}
		}

		public static void Unload()
		{
			ShroomiteZoneEffect = null;
			WyvernMorayEffect = null;
			WyvernMorayLingeringEffect = null;
		}

		// Effects
		public static Effect ShroomiteZoneEffect { get; private set; }
		public static Effect WyvernMorayEffect { get; private set; }
		public static Effect WyvernMorayLingeringEffect { get; private set; }
	}
}
