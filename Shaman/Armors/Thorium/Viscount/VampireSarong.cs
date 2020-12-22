using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Armors.Thorium.Viscount
{
	[AutoloadEquip(EquipType.Legs)]
    public class VampireSarong : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 12, 50);
            item.rare = 2;
            item.defense = 6;
        }

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Vampire Sarong");
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
			recipe.AddIngredient(null, "ViscountMaterial", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
