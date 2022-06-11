using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class EmptyTiara : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 12;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 10, 0);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Tiara");
			Tooltip.SetDefault("Used to make shamanic Tiaras");
		}
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
