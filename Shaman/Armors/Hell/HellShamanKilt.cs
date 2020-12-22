using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Hell
{
	[AutoloadEquip(EquipType.Legs)]
    public class HellShamanKilt : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 22;
            item.height = 14;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.rare = 3;
            item.defense = 7;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Depths Weaver Robes");
		  Tooltip.SetDefault("8% increased shamanic damage");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.08f;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
