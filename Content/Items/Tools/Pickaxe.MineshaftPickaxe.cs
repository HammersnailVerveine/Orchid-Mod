using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OrchidMod.Content.Items.Tools
{
	public class MineshaftPickaxe : OrchidItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Can mine meteorite");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SilverPickaxe);

			Item.damage = 6;
			Item.melee = true;
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

	public class MineshaftPickaxeTile : OrchidTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			this.CreateMapEntry("Mineshaft Pickaxe", new Color(100, 75, 50));

			DustType = 7;
			disableSmartCursor = true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<MineshaftPickaxe>());
		}
	}
}