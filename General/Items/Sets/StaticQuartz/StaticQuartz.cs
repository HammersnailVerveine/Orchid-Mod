using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartz : ModItem
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
			item.createTile = TileType<Tiles.Ores.StaticQuartzOre>();
			item.rare = 1;
			item.value = 500;
		}
	}
}
