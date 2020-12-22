using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
    public class MeteorToolbelt : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 28;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 1;
            item.accessory = true;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Toolbelt");
			Tooltip.SetDefault("Using 3 or more elements in a single attack increases your movement speed");
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistMeteor = true;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);		
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}