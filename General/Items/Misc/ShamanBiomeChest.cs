using Terraria;

namespace OrchidMod.General.Items.Misc
{
	public class ShamanBiomeChest : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.createTile = mod.TileType("ShamanBiomeChest");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroom Chest");
		}
	}
}