using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartzDart : OrchidModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Damage doubles after hitting a tile");
		}

		public override void SetDefaults() {
			item.damage = 6;
			item.ranged = true;
			item.width = 14;
			item.height = 24;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 0, 0, 1);
			item.rare = 0;
			item.shoot = ProjectileType<General.Items.Sets.StaticQuartz.Projectiles.StaticQuartzDartProj>();
			item.shootSpeed = 1f;
			item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 1);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}
