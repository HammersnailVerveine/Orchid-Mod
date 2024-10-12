using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Armor.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class EmpressPlateHead : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}
	}
}
