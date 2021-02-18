using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class GlowingAttractiteFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 35, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 15;
			this.colorR = 5;
			this.colorG = 149;
			this.colorB = 235;
			this.secondaryDamage = 22;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Attractite Flask");
		    Tooltip.SetDefault("Hit target will attract most nearby alchemical lingering projectiles"
							+  "\nThe attractivity buff will jump to the nearest target on miss"
							+  "\nReleases lingering spores"
							+  "\n20% increased damage during the night");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
			if (!Main.dayTime) mult *= 1.2f;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "MoonglowFlask", 1);
			recipe.AddIngredient(null, "AttractiteFlask", 1);
			recipe.AddIngredient(null, "AlchemicStabilizer", 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
