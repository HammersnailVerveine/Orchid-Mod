using OrchidMod.Content.Guardian.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Accessories
{
	public class HarnessYouxia : OrchidModShapeshifterItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
			Item.damage = 35;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GuardianEmblem>();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
			shapeshifter.ShapeshifterHarness = Item.damage;
		}
	}
}