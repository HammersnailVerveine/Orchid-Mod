using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Materials
{
	public class AncientFossil : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[item.type] = 59;
			DisplayName.SetDefault("Ancient Fossil");
			Tooltip.SetDefault("Seems kinda old");
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = TileType<Tiles.Ores.AncientFossil>();
			item.width = 12;
			item.height = 12;
			item.rare = 1;
			item.value = 3000;
		}
	}
}
