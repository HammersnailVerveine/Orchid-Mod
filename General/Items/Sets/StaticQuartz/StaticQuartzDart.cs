using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartzDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Damage doubles after hitting a tile");
		}

		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 0, 0, 1);
			Item.rare = ItemRarityID.White;
			Item.shoot = ProjectileType<General.Items.Sets.StaticQuartz.Projectiles.StaticQuartzDartProj>();
			Item.shootSpeed = 1f;
			Item.ammo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(100);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 1);
			recipe.Register();
		}
	}
}
