using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Thorium.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
    public class TitanSpangenhelm : OrchidModShamanEquipable
    {
        

        public override void SafeSetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 2, 28, 0);
            item.rare = 6;
            item.defense = 14;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Titan Spangenhelm");
		  Tooltip.SetDefault("Your shamanic bonds will last 4 seconds longer"
							+"\nIncreases the effectiveness of your shamanic fire and water bonds");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 4;
			modPlayer.shamanFireBonus += 1;
			modPlayer.shamanWaterBonus += 1;
        }
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				return body.type == thoriumMod.ItemType("TitanBreastplate") && legs.type == thoriumMod.ItemType("TitanGreaves");
			} else {
				return false;
			}
        }
		
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Damage done increased by 18%!";
			player.allDamage += 0.18f;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = drawAltHair = false;
        }
		
		public override bool DrawHead()
        {
            return true;
        }
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(thoriumMod.TileType("SoulForge"));		
				recipe.AddIngredient(null, "TitanBar", 12);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
