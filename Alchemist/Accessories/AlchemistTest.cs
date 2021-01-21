using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Accessories
{
    public class AlchemistTest : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = Item.sellPrice(0, 0, 30, 0);
            item.rare = -11;
            item.accessory = true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Test Accessory");
			Tooltip.SetDefault("Maximum number of simultaneous alchemical elements increased by 4"
							+  "\nMaximum potency increased by 20"
							+  "\n50% increased potency regeneration"
							+  "\n[c/FF0000:Test Item]");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistPotencyMax += 20;
			modPlayer.alchemistNbElementsMax += 4;
			modPlayer.alchemistRegenPotency -= 30;
        }
    }
}