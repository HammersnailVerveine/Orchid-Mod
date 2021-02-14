using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Accessories
{
    public class ReactiveVials : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 26;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = 1;
            item.accessory = true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reactive Vials");
			Tooltip.SetDefault("Your first chemical mixture after using a hidden reaction deals 10% more damage");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistReactiveVials = true;
        }
    }
}