using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Accessories
{
	public class GuideTorchesInactive : OrchidModShapeshifterItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override bool CanRightClick() => true;

		public override bool ConsumeItem(Player player) => false;

		public override void RightClick(Player player)
		{
			int prefix = Item.prefix;
			Item.SetDefaults(ModContent.ItemType<GuideTorches>());
			Item.Prefix(prefix);
		}
	}
}