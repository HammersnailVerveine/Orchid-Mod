using Terraria;

namespace OrchidMod.Content.Guardian.Accessories
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

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			if (modPlayer.GuardianBlock < 1) modPlayer.GuardianBlock = 1;
			if (modPlayer.GuardianSlam < 1) modPlayer.GuardianSlam = 1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Guardian Test Accessory");
			/* Tooltip.SetDefault("Does nothing"
							+ "\n[c/FF0000:Test Item]"); */
		}
	}
}