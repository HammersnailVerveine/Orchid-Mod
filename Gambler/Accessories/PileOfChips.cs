using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Accessories
{
	public class PileOfChips : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 1, 50);
			item.rare = 1;
			item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pile of Chips");
			Tooltip.SetDefault("50% increased gambling chip weapon cycle speed"
							+  "\n15% increased chip weapon damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerDamageChip += 0.15f;
			modPlayer.gamblerChipSpinBonus += 0.5f;
		}
	}
}