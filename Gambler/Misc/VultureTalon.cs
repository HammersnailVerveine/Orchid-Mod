using Terraria;

namespace OrchidMod.Gambler.Misc
{
	public class VultureTalon : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 50);
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture Talon");
		}
	}
}
