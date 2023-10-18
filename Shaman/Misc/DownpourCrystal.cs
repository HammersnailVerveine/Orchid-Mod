using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class DownpourCrystal : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
		}


		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Downpour Crystal");
		}

	}
}
