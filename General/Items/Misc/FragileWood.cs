using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Misc
{
	public class FragileWood : OrchidModItem
	{
		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Fragile Wood");
			Tooltip.SetDefault("Breaks when walked on.");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = TileType<Tiles.Ambient.FragileWood>();
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(283);
			recipe.AddIngredient(ItemID.Wood, 1);
			recipe.Register();
		}
	}
}
