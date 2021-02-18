using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class SunflowerFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 6;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 3, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 3;
			this.colorR = 245;
			this.colorG = 197;
			this.colorB = 1;
			this.secondaryDamage = 8;
			this.secondaryScaling = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forest Samples");
		    Tooltip.SetDefault("Using a water element increases damage and spawns a damaging sunflower"
							+  "\n'Handcrafted jars are unfit for precise alchemy'");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.Mushroom, 3);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
