using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	[ClassTag(ClassTags.Without)]
	public class SunPriestTorch : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.White;
			Item.accessory = true;
			Item.vanity = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Torch");
		}
	}
}