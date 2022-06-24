using Terraria;

namespace OrchidMod.Alchemist.Accessories
{
	public class AlchemistTest : OrchidModAlchemistEquipable
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
			DisplayName.SetDefault("Alchemist Test Accessory");
			Tooltip.SetDefault("Maximum number of simultaneous alchemical elements increased by 4"
							+ "\nMaximum potency increased by 20"
							+ "\n50% increased potency regeneration"
							+ "\n[c/FF0000:Test Item]");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistPotencyMax += 20;
			modPlayer.alchemistNbElementsMax += 4;
			modPlayer.alchemistRegenPotency -= 30;
		}
	}
}