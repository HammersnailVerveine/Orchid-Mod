﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace OrchidMod.Assets
{
	public static class OrchidAssets
	{
		public const string Path = "OrchidMod/Assets/";

		public const string EffectsPath = Path + "Effects/";
		public const string SoundsPath = Path + "Sounds/";
		public const string TexturesPath = Path + "Textures/";

		public const string MiscPath = TexturesPath + "Misc/";
		public const string InvisiblePath = TexturesPath + "Misc/Invisible";

		public const string BuffsPath = TexturesPath + "Buffs/";
		public const string DustsPath = TexturesPath + "Dusts/";
		public const string ProjectilesPath = TexturesPath + "Projectiles/";
		public const string ItemsPath = TexturesPath + "Items/";
		public const string MountsPath = TexturesPath + "Mounts/";
		public const string NPCsPath = TexturesPath + "NPCs/";
		public const string TilesPath = TexturesPath + "Tiles/";
		public const string UIsPath = TexturesPath + "UIs/";

		// ...

		public static Asset<Texture2D> GetExtraTexture(int type, AssetRequestMode mode = AssetRequestMode.AsyncLoad) => ModContent.Request<Texture2D>(MiscPath + "Extra_" + type, mode);
	}
}