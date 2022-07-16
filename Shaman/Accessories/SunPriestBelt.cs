using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class SunPriestBelt : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.White;
			Item.accessory = true;
			Item.vanity = true;
		}
		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Satchel"
						   + "\nTestimony of a past mistake");
		}
	}
}