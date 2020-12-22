using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Catalysts
{
	public class IronCatalyst : OrchidModAlchemistCatalyst
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Catalytic Dagger");
			Tooltip.SetDefault("Used to interact with alchemist catalytic elements");
		}
		
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 4, 50);
			this.catalystType = 1;
		}
		
		public override void CatalystInteractionEffect(Player player) {}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);		
			recipe.AddRecipeGroup("IronBar", 8);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
