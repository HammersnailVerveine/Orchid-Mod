using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OrchidMod.Content.Items.Tools
{
	public class MineshaftPickaxe : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Can mine meteorite");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SilverPickaxe);

			Item.damage = 6;
			Item.DamageType = DamageClass.Melee;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.pick = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
	}

	public class MineshaftPickaxeTile : ModTile
	{
		public override string Texture => OrchidAssets.TilesPath + Name;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileID.Sets.DisableSmartCursor[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Mineshaft Pickaxe");
			AddMapEntry(new Color(100, 75, 50), name);

			DustType = 7;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<MineshaftPickaxe>());
		}
	}
}