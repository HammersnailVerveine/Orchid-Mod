using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
    public class EmeraldIdol : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 1;
            item.accessory = true;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Idol");
			Tooltip.SetDefault("Having an active air bond increases movement speed by 10%");
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanEmerald = true;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Emerald, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}