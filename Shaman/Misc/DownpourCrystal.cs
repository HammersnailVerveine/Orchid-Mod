using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class DownpourCrystal : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 26;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 5;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Downpour Crystal");
		}

	}
}
