using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.GemTiaras
{
	[AutoloadEquip(EquipType.Head)]
    public class TopazTiara : OrchidModShamanEquipable
    {

        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 1;
            item.defense = 2;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Topaz Circlet");
		  Tooltip.SetDefault("Increases the effectiveness of your shamanic earth bonds"
							+"\nYour shamanic bonds will last 3 seconds longer"
							+"\n6% increased shamanic damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.06f;
			modPlayer.shamanEarthBonus += 1;
			modPlayer.shamanBuffTimer += 3;
        }
		
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
			drawAltHair = false;
        }
		
		public override bool DrawHead()
        {
            return true;
        }
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Topaz, 1);
			recipe.AddIngredient(null, "EmptyTiara", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
