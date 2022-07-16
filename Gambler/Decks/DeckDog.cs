using Terraria;

namespace OrchidMod.Gambler.Decks
{
	public class DeckDog : GamblerDeck
	{
		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Dog Gambler Deck");
			Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+1 bork'");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}
	}
}
