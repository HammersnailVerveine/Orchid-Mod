using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class DepthsBaton : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 57;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 1, 6, 0);
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			//Item.shoot = ModContent.ProjectileType<DepthsBatonProj>();
			this.Element = ShamanElement.SPIRIT;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Depths Baton");
			/* Tooltip.SetDefault("Shoots bolts of dark energy"
							  + "\nHitting at maximum range deals increased damage"
							  + "\nHaving 3 or more active shamanic bonds will allow the weapon to shoot a straight beam"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			/*
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int BuffsCount = modPlayer.GetNbShamanicBonds();
			if (BuffsCount > 2)
			{
				int typeAlt = ModContent.ProjectileType<DepthBatonProjAlt>();
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			}
			*/
			return true;
		}


		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Blum>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PerishingSoul>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SporeCaller>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VileSpout>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Blum>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PerishingSoul>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SporeCaller>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SpineScepter>(), 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
