using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Accessories
{
    public class WeightedCorruption : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 1;
            item.accessory = true;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weighted Corruption");
			Tooltip.SetDefault("Increases alchemic main projectile velocity by 25%"
							+  "\nMaximum potency increased by 2");
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistVelocity += 0.25f;
			modPlayer.alchemistPotencyMax += 2;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<Alchemist.Accessories.WeightedBottles>(), 1);
			recipe.AddIngredient(ItemType<Alchemist.Accessories.PreservedCorruption>(), 1);
            recipe.AddTile(114);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}