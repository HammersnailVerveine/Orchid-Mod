using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Misc.Scrolls
{
	public class ScrollTier6 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			Item.rare = ItemRarityID.Yellow;
			this.hintLevel = 6;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alchemist Recipe Scroll");
			// Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
