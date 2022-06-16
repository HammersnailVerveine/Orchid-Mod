using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class LihzahrdSilk : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 18;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 12, 50);
			Item.rare = ItemRarityID.Yellow;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Silk");
			Tooltip.SetDefault("It feels warm in your hand");
		}
	}
}
