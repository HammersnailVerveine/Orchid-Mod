using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Accessories
{
    public class VultureCharm : OrchidModGamblerEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 1;
            item.accessory = true;
			item.crit = 4;
			item.damage = 12;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture Charm");
			Tooltip.SetDefault("Drawing a card releases a burst of vulture feathers");
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerVulture = true;
        }
		
		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : mod.ItemType("VultureTalon"), 3);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}