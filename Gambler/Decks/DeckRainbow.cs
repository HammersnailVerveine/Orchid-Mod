using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Decks
{
	public class DeckRainbow : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbow Gambler Deck");
			Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+7 colorfulness'");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "GamblerAttack", 1);
			recipe.AddIngredient(1066, 1); // Rainbow Dye
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
