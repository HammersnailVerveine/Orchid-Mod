using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Corruption
{
	[AutoloadEquip(EquipType.Legs)]
    public class DarkShamanLegs : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 14;
            item.value = Item.sellPrice(0, 0, 45, 0);
            item.rare = 2;
            item.defense = 4;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Dark Shaman Kilt");
		  Tooltip.SetDefault("6% increased shamanic damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.06f;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 20);
			recipe.AddIngredient(ItemID.ShadowScale, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
