using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
    public class WaxyVial : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 35, 0);
            item.rare = 2;
            item.accessory = true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Waxy Incense");
			Tooltip.SetDefault("Your shamanic earth bonds will cover you in honey"
							 + "\nYou have a chance to release harmful bees when under the effect of shamanic earth bonds");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanHoney = true;
        }
    }
}
