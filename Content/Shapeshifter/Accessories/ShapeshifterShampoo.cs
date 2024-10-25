using OrchidMod.Content.Shapeshifter;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shapeshifter.Accessories
{
	public class ShapeshifterShampoo : OrchidModShapeshifterItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.vanity = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
			shapeshifter.ShapeshifterShampoo = true;
		}
	}
}