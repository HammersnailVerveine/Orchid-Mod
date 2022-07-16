using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class HarpyTalon : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 3, 50);
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Talon");
		}
	}
}
