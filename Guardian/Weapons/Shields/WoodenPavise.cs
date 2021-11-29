using Terraria;
using Terraria.ID;

namespace OrchidMod.Guardian.Weapons.Shields
{
	public class WoodenPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.width = 28;
			item.height = 32;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.knockBack = 3f;
			item.damage = 10;
			item.rare = 0;
			item.useAnimation = 30;
			item.useTime = 30;
			this.distance = 7.5f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Pavise");
			Tooltip.SetDefault("owo");
		}
		
		public override void Block(Player player, Projectile shield, Projectile projectile) {
			player.HealEffect(10, true);
			player.statLife += 10;
			projectile.Kill();
		}

		// public override void AddRecipes()
		// {
		// ModRecipe recipe = new ModRecipe(mod);
		// recipe.AddTile(TileID.WorkBenches);		
		// recipe.AddIngredient(ItemID.Wood, 8);
		// recipe.SetResult(this);
		// recipe.AddRecipe();
		// }
	}
}
