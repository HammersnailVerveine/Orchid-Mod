namespace OrchidMod.Alchemist.Misc.Scrolls
{
	public class ScrollTier2 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			item.rare = 2;
			this.hintLevel = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Recipe Scroll");
			Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
