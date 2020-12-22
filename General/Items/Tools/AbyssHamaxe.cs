using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Tools
{
	public class AbyssHamaxe : ModItem
	{
		public override void SetDefaults() {
			item.CloneDefaults(3522); // Solar Flare Hamaxe
		}
		
		public override void SetStaticDefaults() {
		  DisplayName.SetDefault("Abyss Hamaxe");
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(null, "AbyssFragment", 14);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
