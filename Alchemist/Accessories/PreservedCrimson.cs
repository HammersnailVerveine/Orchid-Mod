using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Accessories
{
	public class PreservedCrimson : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Preserved Crimson");
			Tooltip.SetDefault("Maximum potency increased by 2");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayerAlchemist modPlayer = player.GetModPlayer<OrchidModPlayerAlchemist>();
			modPlayer.alchemistPotencyMax += 2;
		}
	}
}