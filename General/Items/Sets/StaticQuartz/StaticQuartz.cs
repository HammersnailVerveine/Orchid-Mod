using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartz : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
			DisplayName.SetDefault("Static Quartz");
			Tooltip.SetDefault("Has electromagnetic properties");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 28;
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = TileType<Tiles.Ores.StaticQuartzGem>();
			Item.rare = 0;
			Item.value = Item.sellPrice(0, 0, 1, 50);
		}
	}
}
