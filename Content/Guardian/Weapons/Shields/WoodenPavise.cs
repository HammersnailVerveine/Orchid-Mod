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
			Item.damage = 43;
			Item.useTime = 30;
			distance = 30f;
			slamDistance = 30f;
			blockDuration = 60;
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
