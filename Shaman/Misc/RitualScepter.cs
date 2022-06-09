using Terraria;

namespace OrchidMod.Shaman.Misc
{
	public class RitualScepter : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = 1;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ritual Scepter");
			Tooltip.SetDefault("Can be upgraded into various shamanic weapons");
		}

	}
}
