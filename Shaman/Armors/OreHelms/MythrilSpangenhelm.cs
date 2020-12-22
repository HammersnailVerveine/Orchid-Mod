using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
    public class MythrilSpangenhelm : OrchidModShamanEquipable
    {
        

        public override void SafeSetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 4;
            item.defense = 9;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Mythril Spangenhelm");
		  Tooltip.SetDefault("15% increased shamanic damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.15f;
        }
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == 379 && legs.type == 380;
        }
		
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Your shamanic bonds will last 4 seconds longer";
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.armorEffectDrawShadow = true;
			modPlayer.shamanBuffTimer += 4;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = drawAltHair = false;
        }
		
		public override bool DrawHead()
        {
            return true;
        }
		
		public static void ArmorSetShadows(Player player, ref bool longTrail, ref bool smallPulse, ref bool largePulse, ref bool shortTrail)
        {
            shortTrail = true;
            smallPulse = true;
        }
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
