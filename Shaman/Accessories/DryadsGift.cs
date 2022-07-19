using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Accessories
{
	public class DryadsGift : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dryad's Gift");
			Tooltip.SetDefault("Getting a new shamanic bond will increase the duration of your lowest duration one");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanDryad = true;
		}
	}
}