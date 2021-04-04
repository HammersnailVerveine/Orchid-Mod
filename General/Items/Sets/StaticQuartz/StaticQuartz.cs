using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartz : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[item.type] = 59;
			DisplayName.SetDefault("Static Quartz");
			Tooltip.SetDefault("Has electromagnetic properties");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 28;
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = TileType<Tiles.Ores.StaticQuartzGem>();
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 1, 50);
		}
	}
}
