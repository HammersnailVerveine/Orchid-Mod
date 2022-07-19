using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Accessories
{
	public class BundleOfClovers : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bundle of Clovers");
			Tooltip.SetDefault("Gambler dice will slow down when rolling 4 or more"
							 + "\n'4 leaves, all of them!'");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			if (modPlayer.gamblerDieValueCurrent > 3) modPlayer.gamblerDieAnimationPause += 20;
		}
	}
}