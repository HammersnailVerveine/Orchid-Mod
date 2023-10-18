using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Misc.Scrolls
{
	public class ScrollTier2 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			this.hintLevel = 2;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alchemist Recipe Scroll");
			// Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
