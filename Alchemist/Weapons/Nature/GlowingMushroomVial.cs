using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class GlowingMushroomVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 172;
			this.colorR = 44;
			this.colorG = 26;
			this.colorB = 233;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Mushroom Extract");
		    Tooltip.SetDefault("Grows a mushroom, which aura increases the number of spores released by other alchemic extracts"
							+  "\nOnly one mushroom can exist at once");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(ItemID.MudBlock, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
