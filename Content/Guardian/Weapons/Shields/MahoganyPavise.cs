using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class MahoganyPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.knockBack = 6f;
			Item.damage = 46;
			Item.useTime = 28;
			distance = 30f;
			slamDistance = 35f;
			blockDuration = 60;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.RichMahogany, 10);
			recipe.Register();
		}
	}
}
