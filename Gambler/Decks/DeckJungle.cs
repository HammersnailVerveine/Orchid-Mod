using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Decks
{
	public class DeckJungle : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Gambler Deck");
		    Tooltip.SetDefault("Allows the use of gambler abilities"
							+  "\n'+ 15 leafiness '");
		}
		
		public override void SafeSetDefaults() {
			item.value = Item.sellPrice(0, 0, 20, 0);
		}
	}
}
