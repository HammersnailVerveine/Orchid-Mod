using Terraria;

namespace OrchidMod.Gambler.Accessories
{
	public class ConquerorsPennant : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 1;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conqueror's Pennant");
			Tooltip.SetDefault("You enrage when drawing a gambler 'boss' card");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerPennant = true;
		}
	}
}