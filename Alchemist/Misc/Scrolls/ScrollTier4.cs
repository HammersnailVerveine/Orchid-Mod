namespace OrchidMod.Alchemist.Misc.Scrolls
{
	public class ScrollTier4 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			item.rare = 4;
			this.hintLevel = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Recipe Scroll");
			Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
