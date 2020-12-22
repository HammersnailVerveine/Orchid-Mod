using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Crimson
{
	[AutoloadEquip(EquipType.Head)]
    public class BloodShamanTiara : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 2;
            item.defense = 4;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Blood Shaman Tiara");
		  Tooltip.SetDefault("5% increased shamanic damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.05f;
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BloodShamanChest") && legs.type == mod.ItemType("BloodShamanLegs");
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.setBonus = "Shamanic earth bonds are more effective and cause attacks to steal life"
							+ "\n             Your shamanic bonds will last 3 seconds longer";
			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanCrimtane = true;
			modPlayer.shamanEarthBonus += 1;
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
			recipe.AddIngredient(ItemID.CrimtaneBar, 15);
			recipe.AddIngredient(ItemID.TissueSample, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
