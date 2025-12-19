using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class AshWoodPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.knockBack = 8f;
			Item.damage = 55;
			Item.useTime = 28;
			distance = 30f;
			slamDistance = 36f;
			blockDuration = 90;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.AshWood, 10);
			recipe.Register();
		}
	}
}
