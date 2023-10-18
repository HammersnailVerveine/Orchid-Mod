using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	[ClassTag(ClassTags.Without)]
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
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sun Priest Satchel");
		}
	}
}