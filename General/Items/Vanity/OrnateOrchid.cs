using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.General.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class OrnateOrchid : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.vanity = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ornate Orchid");
			// Tooltip.SetDefault("'Great for impersonating Orchid Devs!'");
		}
	}
}
