using Terraria;

namespace OrchidMod.Alchemist.Accessories
{
	public class PreservedCorruption : OrchidModAlchemistEquipable
	{
		public override string Texture => OrchidAssets.AlchemistItemsPath + Name;

		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = 1;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Preserved Corruption");
			Tooltip.SetDefault("Maximum potency increased by 2");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistPotencyMax += 2;
		}
	}
}