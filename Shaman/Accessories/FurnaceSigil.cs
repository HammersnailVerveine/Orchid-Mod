using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Accessories
{
	public class FurnaceSigil : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Furnace Sigil");
			Tooltip.SetDefault("Your shamanic fire bonds allows you to ignite your foes on hit");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanFire = true;
		}
	}
}
