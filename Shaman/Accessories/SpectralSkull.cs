using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
    public class SpectralSkull : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = Item.sellPrice(0, 5, 30, 0);
            item.rare = 8;
            item.accessory = true;
			item.damage = 80;
			item.crit = 4;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Spectral Skull");
		  Tooltip.SetDefault("While under the effect of a shamanic water bond, your shamanic critical strikes will release a lost soul to chase down your enemies");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanSkull = true;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3261, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
