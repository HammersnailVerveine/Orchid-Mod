using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
    public class PrismaticIdol : OrchidModShamanEquipable
    {
        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = Item.sellPrice(0, 5, 50, 0);
            item.rare = 5;
            item.accessory = true;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Idol");
			Tooltip.SetDefault("Your different shamanic bonds increases various stats while active"
							+  "\nYour shamanic earth bonds empowerments will increase your maximum life by 50"
							+  "\nIncreases the duration of your shamanic empowerments by 3 seconds"
							+  "\n10% increased shamanic damage");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanAmber = true;
			modPlayer.shamanAmethyst = true;
			modPlayer.shamanTopaz = true;
			modPlayer.shamanSapphire = true;
			modPlayer.shamanEmerald = true;
			modPlayer.shamanRuby = true;
			modPlayer.shamanDamage += 0.10f;
        }
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AmberIdol", 1);
			recipe.AddIngredient(null, "TopazIdol", 1);
			recipe.AddIngredient(null, "AmethystIdol", 1);
			recipe.AddIngredient(null, "SapphireIdol", 1);
			recipe.AddIngredient(null, "EmeraldIdol", 1);
            recipe.AddIngredient(null, "RubyIdol", 1);
			recipe.AddIngredient(null, "DiamondIdol", 1);
			recipe.AddIngredient(null, "ShamanEmblem", 1);
			recipe.AddIngredient(1225, 10);
			recipe.AddIngredient(547, 5);
			recipe.AddIngredient(548, 5);
			recipe.AddIngredient(549, 5);
            recipe.AddTile(114);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}