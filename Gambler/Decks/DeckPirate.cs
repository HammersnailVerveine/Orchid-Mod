using Terraria;

namespace OrchidMod.Gambler.Decks
{
	public class DeckPirate : GamblerDeck
	{
		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Gambler Deck");
			Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+1 parrot affinity'");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}
	}
}
