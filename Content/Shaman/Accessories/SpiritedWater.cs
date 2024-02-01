using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shaman.Accessories
{
	public class SpiritedWater : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shaman, ShamanElement element, Projectile catalyst)
		{
			if (element == ShamanElement.WATER)
			{
				shaman.modPlayer.TryHeal(30);
			}
		}
	}
}
