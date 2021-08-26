using Terraria;

namespace OrchidMod.Alchemist.Accessories
{
	public class PreservedCrimson : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 15, 0);
			item.rare = 1;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Preserved Crimson");
			Tooltip.SetDefault("Maximum potency increased by 2");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistPotencyMax += 2;
		}
	}
}