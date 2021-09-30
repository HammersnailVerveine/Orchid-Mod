using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OrchidMod.Content.Items.Materials
{
	public class JungleLily : OrchidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Bud");
			Tooltip.SetDefault("Maybe the chemist could help you making it bloom?");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.maxStack = 99;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 5, 0);
		}
	}

	public class JungleLilyTile : OrchidTile
	{
		// TODO: Redraw sprite again, make effects and create permanent generation in the world

		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.AnchorValidTiles = new int[] { 60 };
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Jungle Lily");
			AddMapEntry(new Color(177, 46, 77), name);
			dustType = ModContent.DustType<Content.Dusts.BloomingDust>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<JungleLily>());
		}
	}
}