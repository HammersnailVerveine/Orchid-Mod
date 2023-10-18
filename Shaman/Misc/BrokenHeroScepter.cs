using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class BrokenHeroScepter : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Yellow;
		}


		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Broken Hero Scepter");

			Item.ResearchUnlockCount = 1;
		}

	}
}
