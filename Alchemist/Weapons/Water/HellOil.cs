using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class HellOil : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 3, 20, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 184;
			this.colorR = 117;
			this.colorG = 48;
			this.colorB = 48;
			this.secondaryDamage = 60;
			this.secondaryScaling = 20f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfire Oil");
		    Tooltip.SetDefault("Coats the target in alchemical water and oil"
							+  "\nUsing a fire element in the same attack will drastically increase its damage"
							+  "\nThis will also spread alchemical fire to all nearby oil coated enemies"
							+  "\nHas a chance to release a catalytic oil bubble, coating nearby enemies on reaction");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);	
			recipe.AddIngredient(null, "GoblinArmyFlask", 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "AlchemicStabilizer", 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
