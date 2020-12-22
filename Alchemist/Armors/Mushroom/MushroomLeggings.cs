using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Legs)]
    public class MushroomLeggings : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 4, 0);
            item.rare = 1;
            item.defense = 3;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Phosphorescent Leggings");
		  Tooltip.SetDefault("5% increased potency regeneration");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 3;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(null, "MushroomThread", 1);
			recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
