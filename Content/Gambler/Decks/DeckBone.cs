using Terraria;

namespace OrchidMod.Content.Gambler.Decks
{
	public class DeckBone : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bone Gambler Deck");
			/* Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+206 bones'"); */
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.height = 36;
		}
	}
}
