using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Decks
{
	public class DeckPirate : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Gambler Deck");
		    Tooltip.SetDefault("Allows the use of gambler abilities");
		}
		
		public override void SafeSetDefaults() {
			item.value = Item.sellPrice(0, 1, 0, 0);
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
