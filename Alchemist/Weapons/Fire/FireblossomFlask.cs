using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class FireblossomFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 20;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 255;
			this.colorG = 84;
			this.colorB = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireblossom Extract");
		    Tooltip.SetDefault("Hitting a target coated in alchemic fire deals bonus damage"
							+  "\nReleases fire spores, the less other extracts used, the more"
							+  "\nOnly one set of spores can exist at once"
							+  "\n20% increased damage in hell");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
			if (player.ZoneUnderworldHeight) mult *= 1.2f;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Fireblossom, 3);
			recipe.AddIngredient(ItemID.Hellstone, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
