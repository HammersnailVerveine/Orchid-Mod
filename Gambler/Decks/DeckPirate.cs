using Terraria;

namespace OrchidMod.Gambler.Decks
{
	public class DeckPirate : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Gambler Deck");
			Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+1 parrot affinity'");
		}

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 1, 0, 0);
		}
	}
}
