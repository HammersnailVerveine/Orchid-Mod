using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Dancer.Weapons
{
	public class WoodenTekko : OrchidModDancerItem
	{
		
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 0, 20);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.knockBack = 3f;
			item.damage = 8;
			item.crit = 4; 
			item.rare = 0;
			item.autoReuse = true;
			item.useAnimation = 30;
			item.useTime = 30;
			this.dashTimer = 30;
			this.poiseCost = 0;	
			this.dashVelocity = 7f;
			this.vertical = false;
			this.horizontal = true;
			this.dancerItemType = OrchidModDancerItemType.IMPACT;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Tekko");
		    Tooltip.SetDefault("Horizontally dashes at your foes"
							+  "\n[c/FF0000:Test Item]"
							+  "\nThe Dancer class is a proof of concept"
							+  "\nDo not expect it to be released soon, if at all");
		}
		
		// public override void AddRecipes()
		// {
		    // ModRecipe recipe = new ModRecipe(mod);
			// recipe.AddTile(TileID.WorkBenches);		
			// recipe.AddIngredient(ItemID.Wood, 8);
			// recipe.SetResult(this);
			// recipe.AddRecipe();
        // }
	}
}
