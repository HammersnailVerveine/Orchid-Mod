using Terraria;

namespace OrchidMod.Guardian.Accessories
{
	public class GuardianTest : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = -11;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guardian Test Accessory");
			Tooltip.SetDefault("Does nothing"
							+ "\n[c/FF0000:Test Item]");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.GetDamage<GuardianDamageClass>() += 0.5f;
		}
	}
}