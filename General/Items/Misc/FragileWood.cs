using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Misc
{
	public class FragileWood : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragile Wood");
			Tooltip.SetDefault("Breaks when walked on.");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 22;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = TileType<Tiles.Ambient.FragileWood>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(283);
			recipe.AddIngredient(ItemID.Wood, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
