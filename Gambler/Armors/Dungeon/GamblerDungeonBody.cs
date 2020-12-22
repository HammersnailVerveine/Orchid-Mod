using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Gambler.Armors.Dungeon
{
	[AutoloadEquip(EquipType.Body)]
    public class GamblerDungeonBody : OrchidModGamblerEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 34;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 2;
            item.defense = 7;
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiamat Chestplate");
			Tooltip.SetDefault("10% increased gambling critical strike chance"
							+  "\nMaximum redraws increased by 1");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerCrit += 10;
			modPlayer.gamblerRedrawsMax += 1;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
