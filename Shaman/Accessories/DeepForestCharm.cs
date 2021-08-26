using Terraria;

namespace OrchidMod.Shaman.Accessories
{
	public class DeepForestCharm : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.crit = 4;
			item.accessory = true;
			item.damage = 15;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deep Forest Charm");
			Tooltip.SetDefault("Your shamanic earth bonds will summon sharp leaves around you");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanForest = true;
		}
	}
}