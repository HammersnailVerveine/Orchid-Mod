using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shapeshifter.Misc
{
	public class ShapeshifterBlankEffigy : OrchidModShapeshifterItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
		}
	}
}
