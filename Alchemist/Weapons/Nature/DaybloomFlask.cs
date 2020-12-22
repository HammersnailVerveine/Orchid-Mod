using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class DaybloomFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 55;
			this.colorR = 255;
			this.colorG = 198;
			this.colorB = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybloom Extract");
		    Tooltip.SetDefault("Deals double damage if another element is used in the reaction"
							+  "\nHitting a target coated in alchemic nature deals bonus damage"
							+  "\nReleases lingering nature spores"
							+  "\nOnly one set of spores can exist at once"
							+  "\n20% increased damage during the day");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
			if (Main.dayTime) mult *= 1.2f;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Daybloom, 3);
			recipe.AddIngredient(ItemID.Mushroom, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
