using Terraria;

namespace OrchidMod.Gambler.Decks
{
	public class DeckEnchanted : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Gambler Deck");
			Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+15 enchantment'");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.height = 36;
		}
	}
}
