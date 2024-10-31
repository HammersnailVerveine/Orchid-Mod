using OrchidMod.Content.General.Misc;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class HorizonHamaxe : LuminiteTool
	{
		public HorizonHamaxe() : base(lightColor: new(229, 181, 142), itemCloneType: ItemID.SolarFlareHammer) { }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<HorizonFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
