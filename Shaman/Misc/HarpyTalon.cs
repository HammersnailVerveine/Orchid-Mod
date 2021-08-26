using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class HarpyTalon : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 3, 50);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Talon");
		}
	}
}
