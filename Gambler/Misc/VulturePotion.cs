using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Misc
{
    public class VulturePotion : OrchidModItem
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
            item.height = 30;
            item.rare = 1;
            item.buffType = BuffType<Gambler.Buffs.VulturePotionBuff>(); 
            item.buffTime = 60 * 180;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Scavenger Potion");
      Tooltip.SetDefault("20% increased gambler chip generation");
    }
		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Bottles);	
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddIngredient(ItemID.Cactus, 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : ItemType<VultureTalon>(), 2);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
