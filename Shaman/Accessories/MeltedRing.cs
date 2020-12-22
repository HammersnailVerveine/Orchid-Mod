using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
    public class MeltedRing : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 55, 0);
            item.rare = 3;
			item.crit = 4;
            item.accessory = true;
			item.damage = 30;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Ring");
			Tooltip.SetDefault("Allows the wearer to release damaging droplets of lava while under the effect of shamanic air bond");	
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDripping = true;
        }
    }
}
