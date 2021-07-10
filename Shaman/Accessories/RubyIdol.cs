using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
    public class RubyIdol : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 1;
            item.accessory = true;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Idol");
			Tooltip.SetDefault("Having an active fire bond increases life regeneration");
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanRuby = true;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ruby, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
