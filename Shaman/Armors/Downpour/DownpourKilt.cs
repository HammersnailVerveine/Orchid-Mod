using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Downpour
{
	[AutoloadEquip(EquipType.Legs)]
    public class DownpourKilt : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 14;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.defense = 11;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Downpour Kilt");
		  Tooltip.SetDefault("15% increased shamanic damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.15f;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 16);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 16);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
