using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class LihzahrdSilk : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 18;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 12, 50);
			item.rare = 8;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Silk");
			Tooltip.SetDefault("It feels warm in your hand");
		}
	}
}
