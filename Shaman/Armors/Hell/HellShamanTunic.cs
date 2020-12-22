using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Hell
{
	[AutoloadEquip(EquipType.Body)]
    public class HellShamanTunic : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 34;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.rare = 3;
            item.defense = 8;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Depths Weaver Tunic");
		  Tooltip.SetDefault("9% increased shamanic critical strike chance");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 9;
        }
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 15);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.Bone, 15);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
