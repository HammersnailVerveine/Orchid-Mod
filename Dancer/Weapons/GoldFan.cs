using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Dancer.Weapons
{
	public class GoldFan : OrchidModDancerItem
	{
		
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 0, 20);
			item.width = 42;
			item.height = 42;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item101;
			item.knockBack = 3f;
			item.damage = 8;
			item.crit = 4; 
			item.rare = 0;
			item.autoReuse = true;
			item.useAnimation = 30;
			item.useTime = 30;
			this.dashTimer = 10;
			this.poiseCost = 0;	
			this.dashVelocity = 30f;
			this.vertical = true;
			this.horizontal = true;
			this.dancerItemType = OrchidModDancerItemType.PHASE;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Fan");
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
