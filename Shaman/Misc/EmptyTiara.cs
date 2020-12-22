using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class EmptyTiara : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 18;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 10, 0);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Tiara");
		  Tooltip.SetDefault("Used to make shamanic Tiaras");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
