using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Materials
{
	public class AncientFossil : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
			DisplayName.SetDefault("Ancient Fossil");
			Tooltip.SetDefault("Seems kinda old");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = TileType<Tiles.Ores.AncientFossil>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = 1;
			Item.value = 3000;
		}
	}
}
