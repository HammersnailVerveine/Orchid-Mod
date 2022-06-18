using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Accessories
{
	public class VenomSigil : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 35, 0);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom Sigil");
			Tooltip.SetDefault("Your shamanic fire bonds allows you to envenom your foes on hit");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanVenom = true;
		}
	}
}
