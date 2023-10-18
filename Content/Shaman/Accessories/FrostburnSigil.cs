using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class FrostburnSigil : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Frostburn Sigil");
			// Tooltip.SetDefault("Your shamanic fire bonds allows you to frostburn your foes on hit");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanIce = true;
		}
	}
}
