using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Misc
{
    public class ShamanicEmpowermentPotion : OrchidModItem
    {
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;  
            item.useStyle = 2;    
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.maxStack = 30;         
            item.consumable = true; 
			item.value = Item.sellPrice(0, 0, 2, 0);
            item.width = 20;
            item.height = 28;
            item.rare = 1;
            item.buffType = mod.BuffType("ShamanicEmpowerment"); 
            item.buffTime = 60 * 180;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Shamanic Empowerment Potion");
      Tooltip.SetDefault("Increases the effectiveness of all your shamanic bonds");
    }
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Bottles);	
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ItemID.Moonglow, 1);	
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
