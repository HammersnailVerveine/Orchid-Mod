using Terraria.ID;

namespace OrchidMod.Alchemist.Misc.Scrolls
{
	public class ScrollTier5 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			Item.rare = ItemRarityID.Pink;
			this.hintLevel = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Recipe Scroll");
			Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
