using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Tiles.Ambient
{
	public class JungleLilyTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.AnchorValidTiles = new int[]{60}; // Jungle Grass
			TileObjectData.addTile(Type);
			
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Jungle Lily");
			AddMapEntry(new Color(177, 46, 77), name);
			dustType = DustType<Dusts.BloomingDust>();
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Alchemist.Misc.JungleLilyItem>());
		}
	}
}