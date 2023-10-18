using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Gambler.Accessories
{
	public class ImpDiceCup : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 2, 30, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Imp Dice Cup");
			// Tooltip.SetDefault("Gambler dice cannot roll a 1 or a 2");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.gamblerImp = true;
		}
	}
}