using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class Warhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 5f;
			Item.shootSpeed = 8f;
			Item.damage = 54;
			Range = 26;
			GuardStacks = 1;
			ReturnSpeed = 0.8f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.StoneBlock, 25);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 3);
			recipe.Register();
		}
	}
}
