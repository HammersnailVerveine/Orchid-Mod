using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Decks
{
	public class GamblerAttack : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambler Deck");
		    Tooltip.SetDefault("Allows the use of gambler abilities");
		}
	}
}
