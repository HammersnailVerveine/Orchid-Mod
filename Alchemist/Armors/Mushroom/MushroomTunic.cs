using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Body)]
    public class MushroomTunic : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 30;
            item.height = 22;
            item.value = Item.sellPrice(0, 0, 5, 0);
            item.rare = 1;
            item.defense = 3;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phosphorescent Tunic");
			Tooltip.SetDefault("5% increased potency regeneration"
							+  "\nMaximum potency increased by 2");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 3;
			modPlayer.alchemistPotencyMax += 2;
        }
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(null, "MushroomThread", 1);
			recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
