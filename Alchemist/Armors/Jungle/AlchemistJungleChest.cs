using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Alchemist.Armors.Jungle
{
	[AutoloadEquip(EquipType.Body)]
    public class AlchemistJungleChest : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 30;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 90, 0);
            item.rare = 3;
            item.defense = 7;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lily Tunic");
			Tooltip.SetDefault("Maximum potency increased by 3"
							+  "\nIncreases alchemic main projectile velocity by 20%");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistPotencyMax += 3;
			modPlayer.alchemistVelocity += 0.2f;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "JungleLilyItemBloomed", 1);
			recipe.AddIngredient(210, 1); // Vine
			recipe.AddIngredient(331, 5); // Jungle Spores
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
