using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class WoodenPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 0, 30);
			Item.knockBack = 5f;
			Item.damage = 12;
			Item.useTime = 35;
			this.distance = 30f;
			this.bashDistance = 70f;
			this.blockDuration = 60;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wooden Pavise");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.Register();
		}
	}
}
