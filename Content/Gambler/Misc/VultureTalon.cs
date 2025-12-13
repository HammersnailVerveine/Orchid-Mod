using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler.Misc
{
	public class VultureTalon : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 50);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vulture Talon");
			Item.ResearchUnlockCount = 25;
		}
	}
}
