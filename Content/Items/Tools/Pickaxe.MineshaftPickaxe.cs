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
			item.CloneDefaults(ItemID.SilverPickaxe);

			item.damage = 6;
			item.melee = true;
			item.width = 34;
			item.height = 34;
			item.useTime = 19;
			item.useAnimation = 19;
			item.pick = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
	}

	public class MineshaftPickaxeTile : OrchidTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			this.CreateMapEntry("Mineshaft Pickaxe", new Color(100, 75, 50));

			dustType = 7;
			disableSmartCursor = true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<MineshaftPickaxe>());
		}
	}
}