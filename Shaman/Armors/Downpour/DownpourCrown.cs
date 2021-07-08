using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Downpour
{
	[AutoloadEquip(EquipType.Head)]
    public class DownpourCrown : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 8;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Downpour Crown");
		  Tooltip.SetDefault("Your shamanic bonds will last 4 seconds longer");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 4;
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("DownpourTunic") && legs.type == mod.ItemType("DownpourKilt");
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.setBonus = " Dealing damage with 3 or more active bonds has a chance to electrocute the enemy"; // + bonds affects alchemic stats
			modPlayer.shamanDownpour = true;
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
			recipe.AddIngredient(ItemID.AdamantiteBar, 10);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 10);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
