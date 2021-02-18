using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class WaterleafFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 22;
			this.colorG = 121;
			this.colorB = 82;
			this.secondaryDamage = 12;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Waterleaf Extract");
		    Tooltip.SetDefault("Releases water spores, the less other extracts used, the more"
							+  "\nOnly one set of spores can exist at once"
							+  "\n20% increased damage during rain");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
			if (Main.raining) mult *= 1.2f;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Waterleaf, 3);
			recipe.AddIngredient(ItemID.Cactus, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
