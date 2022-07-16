using Terraria.ID;

namespace OrchidMod.Alchemist.Misc.Scrolls
{
	public class ScrollTier2 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			this.hintLevel = 2;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Recipe Scroll");
			Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
