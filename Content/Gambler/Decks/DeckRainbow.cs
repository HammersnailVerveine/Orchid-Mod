using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler.Decks
{
	public class DeckRainbow : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rainbow Gambler Deck");
			/* Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+7 colorfulness'"); */
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "GamblerAttack", 1);
			recipe.AddIngredient(1066, 1); // Rainbow Dye
			recipe.Register();
		}
	}
}
