using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
    public class HarpyAnklet : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 11, 50);
            item.rare = 1;
			item.crit = 4;
            item.accessory = true;
			item.damage = 12;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Anklet");
			Tooltip.SetDefault("Releases a burst of feathers around you when using a double jump"
					 	     + "\nAllows you to double jump, if you cannot already"
					 	     + "\nDamage increased under the effect of cloud burst potion"
							 + "\nThese effects will only occur if you have an active shamanic air bond");
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanHarpyAnklet = true;
        }
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).shamanDamage;
			if (Main.LocalPlayer.FindBuffIndex(mod.BuffType("HarpyAgility")) > -1)
				add += 1.1f;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);	
			recipe.AddIngredient(null, "HarpyTalon", 1);
			recipe.AddIngredient(ItemID.Feather, 4);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
