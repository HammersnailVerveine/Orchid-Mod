using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class BrokenHeroScepter : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = 8;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Hero Scepter");
		}

	}
}
