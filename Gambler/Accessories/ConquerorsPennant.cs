using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Accessories
{
	public class ConquerorsPennant : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Conqueror's Pennant");
			// Tooltip.SetDefault("You enrage when drawing a gambler 'boss' card");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.gamblerPennant = true;
		}
	}
}