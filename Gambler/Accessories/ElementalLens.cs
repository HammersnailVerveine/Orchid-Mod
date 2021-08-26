using Terraria;

namespace OrchidMod.Gambler.Accessories
{
	public class ElementalLens : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 1;
			item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Lens");
			Tooltip.SetDefault("Gambler 'elemental' cards attacks will affect enemy with related debuffs"
							+ "\nCards already inflicting debuffs will have them last longer");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerElementalLens = true;
		}
	}
}