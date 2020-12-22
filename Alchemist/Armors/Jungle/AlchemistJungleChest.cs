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
			DisplayName.SetDefault("Blooming Tunic");
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
			recipe.AddIngredient(331, 16); // Jungle Spores
			recipe.AddIngredient(209, 10); // Stinger
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
