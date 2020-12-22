using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Tools
{
	public class AbyssPickaxe : ModItem
	{
		public override void SetDefaults() {
			item.CloneDefaults(2786); // Solar Flare Pickaxe
		}
		
		public override void SetStaticDefaults() {
		  DisplayName.SetDefault("Abyss Pickaxe");
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(null, "AbyssFragment", 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}