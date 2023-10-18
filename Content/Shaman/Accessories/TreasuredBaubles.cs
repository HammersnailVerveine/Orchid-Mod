using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class TreasuredBaubles : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 35, 20);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Treasured Baubles");
			// Tooltip.SetDefault("After completing an orb weapon cycle, you will be given a bonus orb on your next hit");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		}
	}
}
