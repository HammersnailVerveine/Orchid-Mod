using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
    public class FragilePresent : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = Item.sellPrice(0, 4, 0, 0);
            item.rare = 8;
            item.accessory = true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragile Present");
			Tooltip.SetDefault("8% increased shamanic damage"
							+  "\nIncreases the effectiveness of all your shamanic bonds"
							+  "\nHowever, taking direct damage will nullify theses effects, and reduce their effectiveness for 15 seconds");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!(Main.LocalPlayer.FindBuffIndex(mod.BuffType("BrokenPower")) > -1))
			{
				modPlayer.shamanFireBonus ++;
				modPlayer.shamanWaterBonus ++;		
				modPlayer.shamanAirBonus ++;	
				modPlayer.shamanEarthBonus ++;	
				modPlayer.shamanSpiritBonus ++;	
				modPlayer.shamanDamage	+= 0.08f;
			}
			modPlayer.shamanSunBelt = true;
        }
    }
}