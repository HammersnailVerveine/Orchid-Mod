using Terraria;

namespace OrchidMod.Shaman.Accessories
{
	public class DryadsGift : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 1;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dryad's Gift");
			Tooltip.SetDefault("Getting a new shamanic bond will increase the duration of your lowest duration one");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDryad = true;
		}
	}
}