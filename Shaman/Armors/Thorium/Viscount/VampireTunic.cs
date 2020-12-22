using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Thorium.Viscount
{
	[AutoloadEquip(EquipType.Body)]
    public class VampireTunic : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
            item.defense = 7;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Vampire Tunic");
		  Tooltip.SetDefault("8% increased shamanic critical chance");
		}

        public override void UpdateEquip(Player player)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 8;
        }
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ViscountMaterial", 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
