using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
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
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 1, 6, 0);
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("DepthsBatonProj").Type;
			this.empowermentType = 5;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Depths Baton");
			Tooltip.SetDefault("Shoots bolts of dark energy"
							  + "\nHitting at maximum range deals increased damage"
							  + "\nHaving 3 or more active shamanic bonds will allow the weapon to shoot a straight beam");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			if (BuffsCount > 2)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("DepthBatonProjAlt").Type, damage - 10, knockBack, player.whoAmI);
			}
			return true;
		}


		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "Blum", 1);
			recipe.AddIngredient(null, "PerishingSoul", 1);
			recipe.AddIngredient(null, "SporeCaller", 1);
			recipe.AddIngredient(null, "VileSpout", 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
			recipe.AddRecipe();

			recipe = CreateRecipe();
			recipe.AddIngredient(null, "Blum", 1);
			recipe.AddIngredient(null, "PerishingSoul", 1);
			recipe.AddIngredient(null, "SporeCaller", 1);
			recipe.AddIngredient(null, "SpineScepter", 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
