using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace OrchidMod.Content.General.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class NeoAncientHallowedPlateMail : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = ArmorIDs.Back.HallowedCape;
			ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = ArmorIDs.Back.HallowedCape;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 24;
			Item.rare = ItemRarityID.Pink;
		}
	}
}
