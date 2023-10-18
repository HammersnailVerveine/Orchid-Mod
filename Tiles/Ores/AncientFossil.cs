using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Tiles.Ores
{
	public class AncientFossil : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 415;
			//Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 2400; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Ancient Fossil");
			AddMapEntry(new Color(178, 178, 138), name);

			DustType = 18;
			HitSound = SoundID.Dig;
			MineResist = 1f;
			MinPick = 35;
		}
	}
}