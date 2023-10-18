using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class DeepForestCharm : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.damage = 15;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Deep Forest Charm");
			// Tooltip.SetDefault("Your shamanic earth bonds will summon sharp leaves around you");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanForest = true;
		}
	}
}