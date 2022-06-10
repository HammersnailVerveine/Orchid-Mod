using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod
{
	public static class OrchidAssets
	{
		public const string Path = "OrchidMod/Assets/";

		public const string EffectsPath = Path + "Effects/";
		public const string TexturesPath = Path + "Textures/";
		public const string SoundsPath = Path + "Sounds/";

		public const string ItemsPath = TexturesPath + "Items/";
		public const string ProjectilesPath = TexturesPath + "Projectiles/";
		public const string DustsPath = TexturesPath + "Dusts/";
		public const string TilesPath = TexturesPath + "Tiles/";
		public const string MiscPath = TexturesPath + "Misc/";
		public const string InvisiblePath = TexturesPath + "Misc/Invisible";

		// ...

		public static Asset<Texture2D> GetExtraTexture(int type, AssetRequestMode mode = AssetRequestMode.AsyncLoad) => ModContent.Request<Texture2D>(MiscPath + "Extra_" + type, mode);
		public static Effect GetEffect(string name) => EffectLoader.GetEffect(name);
	}
}