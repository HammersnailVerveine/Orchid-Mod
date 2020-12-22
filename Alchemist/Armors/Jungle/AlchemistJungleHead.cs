using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Armors.Jungle
{
	[AutoloadEquip(EquipType.Head)]
    public class AlchemistJungleHead : OrchidModAlchemistEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 26;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.rare = 3;
            item.defense = 6;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Blooming Hood");
		  Tooltip.SetDefault("20% increased potency regeneration"
							+  "\nMaximum number of simultaneous alchemical elements increased by 1");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 12;
			modPlayer.alchemistNbElementsMax += 1;
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<Alchemist.Armors.Jungle.AlchemistJungleChest>() && legs.type == ItemType<Alchemist.Armors.Jungle.AlchemistJungleLegs>();
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.setBonus = "Trigerring enough catalytic reactions creates a catalytic flower bud";
			modPlayer.alchemistFlowerSet = true;
        }
		
        public override bool DrawHead()
        {
            return true;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = false;
			drawAltHair = false;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(331, 8); // Jungle Spores
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
