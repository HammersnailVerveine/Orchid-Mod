using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Accessories
{
	public class PileOfChips : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 1, 50);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pile of Chips");
			/* Tooltip.SetDefault("50% increased gambling chip weapon cycle speed"
							+  "\n15% increased chip weapon damage"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			player.GetDamage<GamblerChipDamageClass>() += 0.15f;
			modPlayer.gamblerChipSpinBonus += 0.5f;
		}
	}
}