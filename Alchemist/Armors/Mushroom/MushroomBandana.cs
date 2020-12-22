using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Alchemist.Armors.Mushroom
{
	[AutoloadEquip(EquipType.Head)]
    public class MushroomBandana : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = 1;
            item.defense = 2;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Phosphorescent Bandana");
		  Tooltip.SetDefault("5% increased potency regeneration");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 3;
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MushroomTunic") && legs.type == mod.ItemType("MushroomLeggings");
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.setBonus = "Maximum number of simultaneous alchemical elements increased by 1";
			modPlayer.alchemistNbElementsMax += 1;
        }
		
        public override bool DrawHead()
        {
            return true;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
			drawAltHair = false;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 4);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(null, "MushroomThread", 1);
			recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
