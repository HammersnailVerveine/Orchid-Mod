using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class MoonglowFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 15;
			this.colorR = 10;
			this.colorG = 176;
			this.colorB = 230;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonglow Extract");	
		    Tooltip.SetDefault("Hitting a target coated in alchemic nature deals bonus damage"
							+  "\nReleases nature spores, the less other extracts used, the more"
							+  "\nOnly one set of spores can exist at once"
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
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Moonglow, 3);
			recipe.AddIngredient(ItemID.JungleSpores, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
