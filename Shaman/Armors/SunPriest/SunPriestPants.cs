using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.SunPriest
{
	[AutoloadEquip(EquipType.Legs)]
    public class SunPriestPants : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 14;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = 8;
            item.defense = 15;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sun Priest Pants");
		  Tooltip.SetDefault("6% increased shamanic damage"
						   + "\n5% increased movement speed");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            player.moveSpeed += 0.05f;
			modPlayer.shamanDamage += 0.06f;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "LihzahrdSilk", 4);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
