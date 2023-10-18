using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	public class EmptyFlask : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 4, 0);
			Item.rare = ItemRarityID.White;
		}


		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Empty Flask");
			/* Tooltip.SetDefault("Sold by the mineshaft chemist"
							+ "\nUsed to make various alchemist weapons"); */
		}

	}
}
