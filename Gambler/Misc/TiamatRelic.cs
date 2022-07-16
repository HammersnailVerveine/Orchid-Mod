using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Misc
{
	public class TiamatRelic : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 12;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 15, 0);
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Tyche Relic");
		}
	}
}
