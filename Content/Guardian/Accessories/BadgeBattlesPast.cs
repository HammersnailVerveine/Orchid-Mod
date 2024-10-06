using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class BadgeBattlesPast : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianSpikeGoblin = true;
		}
	}
}