using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.SunPriest
{
	[AutoloadEquip(EquipType.Head)]
    public class SunPriestHood : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 7, 50, 0);
            item.rare = 8;
            item.defense = 20;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sun Priest Hood");
		  Tooltip.SetDefault("10% increased shamanic damage"
							+ "\nYour shamanic bonds will last 5 seconds longer");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.1f;
			modPlayer.shamanBuffTimer += 5;
        }
		
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SunPriestRobe") && legs.type == mod.ItemType("SunPriestPants");
        }
		
        public override void UpdateArmorSet(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.setBonus = "Releases homing bursts of light while under the effect of a shamanic fire bond";
			modPlayer.shamanSmite = true;
        }
		
        public override bool DrawHead()
        {
            return false;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = false;
			drawAltHair = false;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LihzahrdSilk", 3);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
