using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Misc
{
	public class CouldInAVial : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloud in a Flask Token");
			Tooltip.SetDefault("Sorry, I broke your potion !"
							+ "\nCan be used to craft a Cloud in a Flask");
		}

		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.SetResult(ItemType<Alchemist.Weapons.Air.CloudInAVial>());
			recipe.AddRecipe();
        }
	}
}
