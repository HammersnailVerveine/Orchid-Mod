using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class PoisonSigil : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Poison Sigil");
			// Tooltip.SetDefault("Your shamanic fire bonds allows you to poison your foes on hit");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanPoison = true;
		}
	}
}
