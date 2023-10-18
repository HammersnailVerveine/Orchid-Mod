using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler.Misc
{
	public class TiamatRelic : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 12;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 15, 0);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tyche Relic");
		}
	}
}
