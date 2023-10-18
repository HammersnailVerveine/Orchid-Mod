using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Misc
{
	public class HarpyTalon : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 3, 50);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Harpy Talon");
		}
	}
}
