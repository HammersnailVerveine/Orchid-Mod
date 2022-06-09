using Terraria;

namespace OrchidMod.Gambler.Accessories
{
	public class SlimyLollipop : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = 1;
			Item.accessory = true;
			Item.crit = 4;
			Item.damage = 15;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimy Lollipop");
			Tooltip.SetDefault("Periodically releases friendly slimes when a gambler 'slime' card is active");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerSlimyLollipop = true;
		}
	}
}