using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Assets;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace OrchidMod.Common.Graphics
{
	[Autoload(Side = ModSide.Client)]
	public class EffectLoader : ILoadable
	{
		private static Dictionary<string, Asset<Effect>> effectsByName = new();
		private static ArmorShaderData transparentDustShaderData = null;

		// ...

		void ILoadable.Load(Mod mod)
		{
			var methodInfo = typeof(Mod).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true);
			var modFile = (TmodFile)methodInfo.Invoke(mod, null);
			var entries = modFile.Where(i => i.Name.StartsWith("Assets/Effects/") && i.Name.EndsWith(".xnb"));

			foreach (var entry in entries)
			{
				var path = entry.Name.Replace(".xnb", "");
				var name = path.Replace("Assets/Effects/", "");

				if (effectsByName.ContainsKey(name)) continue;

				if (ModContent.RequestIfExists<Effect>($"{mod.Name}/{path}", out Asset<Effect> asset, AssetRequestMode.ImmediateLoad))
				{
					effectsByName.Add(name, asset);
					SetEffectInitParameters(name, asset.Value.Parameters);
				}
			}

			transparentDustShaderData = new ArmorShaderData(new Ref<Effect>(GetEffect("TransparentDust").Value), "TransparentDust");
		}

		void ILoadable.Unload()
		{
			effectsByName.Clear();
			effectsByName = null;
			transparentDustShaderData = null;
		}

		// ...

		public static Asset<Effect> GetEffect(string name)
			=> effectsByName[name];

		public static ArmorShaderData GetTransparentDustShaderData()
			=> transparentDustShaderData;

		public static void CreateSceneFilter(string effect, EffectPriority priority)
			=> Filters.Scene[$"{OrchidMod.Instance.Name}:{effect}"] = new(new ScreenShaderData(new Ref<Effect>(GetEffect(effect).Value), effect), priority);

		private static void SetEffectInitParameters(string name, EffectParameterCollection parameters)
		{
			switch (name)
			{
				case "ShroomiteScepter":
					//parameters["PerlinTexture"].SetValue(ModContent.Request<Texture2D>("Terraria/Misc/Perlin").Value); [SP]
					parameters["PerlinTexture"].SetValue(ModContent.Request<Texture2D>("OrchidMod/Content/Shaman/Weapons/AdornedBranch").Value); // random texture placeholder
					break;
				case "WyvernMoray":
					parameters["Texture0"].SetValue(OrchidAssets.GetExtraTexture(5, AssetRequestMode.ImmediateLoad).Value);
					parameters["Texture1"].SetValue(OrchidAssets.GetExtraTexture(2, AssetRequestMode.ImmediateLoad).Value);
					break;
				case "WyvernMorayLingering":
					parameters["Texture1"].SetValue(OrchidAssets.GetExtraTexture(10, AssetRequestMode.ImmediateLoad).Value);
					break;
			}
		}
	}
}