using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Tiles.Ores
{
	public class AncientFossil : ModTile
	{
		public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileValue[Type] = 415;
			//Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 2400; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ancient Fossil");
			AddMapEntry(new Color(178, 178, 138), name);

			dustType = 18;
			drop = ItemType<General.Items.Materials.AncientFossil>();
			soundType = 21;
			soundStyle = 1;
			mineResist = 1f;
			minPick = 35;
		}
	}
}