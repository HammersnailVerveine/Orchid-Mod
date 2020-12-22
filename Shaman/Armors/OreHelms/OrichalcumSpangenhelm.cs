using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
    public class OrichalcumSpangenhelm : OrchidModShamanEquipable
    {
        
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 4;
            item.defense = 10;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Orichalcum Spangenhelm");
		  Tooltip.SetDefault("Your shamanic bonds will last 3 seconds longer"
							+"\n18% increased shamanic critical strike chance");
		}

       public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            modPlayer.shamanBuffTimer += 3;
            modPlayer.shamanCrit += 18;
        }
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == 1213 && legs.type == 1214;
        }
		
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Flower petals will fall on your target for extra damage";
			player.armorEffectDrawShadow = true;
			player.onHitPetal = true;
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
        }
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
