using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class FossilChip : OrchidModGamblerChipItem
	{
		
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.knockBack = 6f;
			item.damage = 30;
			item.crit = 4; 
			item.rare = 1;
			item.shootSpeed = 10f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.FossilChipProj>();
			item.autoReuse = true;
			this.chipCost = 1;	
			this.consumeChance = 100;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fossil Chip");
		    Tooltip.SetDefault("Throws gambling chips at your foes");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);		
			recipe.AddIngredient(ItemID.Amber, 8);
			recipe.AddIngredient(3380, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
