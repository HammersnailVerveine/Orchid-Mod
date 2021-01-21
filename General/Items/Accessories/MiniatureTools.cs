using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Accessories
{
    public class MiniatureTools : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 1;
            item.accessory = true;
        }
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Salvaged Toolbox");
		  Tooltip.SetDefault("Provides a small amount of light"
							+"\nAllows you to drastically reduce trap damage, on a cooldown");
		}
		
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.generalTools = true;
			Lighting.AddLight((int)(player.Center.X / 16), (int)(player.Center.Y / 16), 0.2f, 0.10f, 0f);
        }
    }
}
