using Terraria;

namespace OrchidMod.Guardian.Accessories
{
	public class GuardianTest : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = -11;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guardian Test Accessory");
			Tooltip.SetDefault("Does nothing"
							+ "\n[c/FF0000:Test Item]");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
		}
	}
}