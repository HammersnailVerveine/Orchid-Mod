using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Tiles.Ambient
{
	public class MineshaftStaticTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileValue[Type] = 280;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 500;
			Main.tileObsidianKill[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Static Quartz Node");
			AddMapEntry(new Color(216, 21, 54), name);
			dustType = 60;
			disableSmartCursor = true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ItemType<General.Items.Accessories.MiniatureTools>());
		}
	}
}