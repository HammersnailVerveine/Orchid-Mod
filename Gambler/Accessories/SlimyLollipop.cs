using Terraria;

namespace OrchidMod.Gambler.Accessories
{
	public class SlimyLollipop : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 1;
			item.accessory = true;
			item.crit = 4;
			item.damage = 15;
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