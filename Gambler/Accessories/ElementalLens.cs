using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Accessories
{
	public class ElementalLens : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
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