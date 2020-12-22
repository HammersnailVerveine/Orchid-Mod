using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
    public class SunPriestBelt : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 0;
            item.accessory = true;
			item.vanity = true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Satchel"
						   + "\nTestimony of a past mistake");
		}
    }
}