using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.GemTiaras
{
	[AutoloadEquip(EquipType.Head)]
    public class SapphireTiara : OrchidModShamanEquipable
    {

        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = 1;
            item.defense = 2;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sapphire Circlet");
		  Tooltip.SetDefault("Having an active water bond increases shamanic critical strike chance by 10%"
							+"\nYour shamanic bonds will last 3 seconds longer");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanSapphire = true;
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
			recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddIngredient(null, "EmptyTiara", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
