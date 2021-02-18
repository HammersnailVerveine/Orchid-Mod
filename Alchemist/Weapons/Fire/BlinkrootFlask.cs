using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class BlinkrootFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 9;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 57;
			this.colorR = 191;
			this.colorG = 82;
			this.colorB = 0;
			this.secondaryDamage = 16;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blinkroot Extract");
		    Tooltip.SetDefault("Releases fire spores, the less other extracts used, the more"
							+  "\nOnly one set of spores can exist at once"
							+  "\n20% increased damage in caverns");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
			if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight) mult *= 1.2f;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Blinkroot, 3);
			recipe.AddIngredient(ItemID.Cobweb, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
