using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.General.Items.Consumables
{
    public class HarpyPotion : ModItem
    {
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;  
            item.useStyle = 2;    
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 30;         
            item.consumable = true; 
			item.value = Item.sellPrice(0, 0, 2, 0);
            item.width = 20;
            item.height = 28;
            item.rare = 1;
            item.buffType = mod.BuffType("HarpyAgility"); 
            item.buffTime = 60 * 180;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Cloud Burst Potion");
	  Tooltip.SetDefault("Your first bonus jump will release a burst of damaging feathers"
					+    "\nAllows you to double jump, if you cannot already");
    }
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Bottles);	
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);	
			recipe.AddIngredient(null, "HarpyTalon", 1);
			recipe.AddIngredient(ItemID.Feather, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
