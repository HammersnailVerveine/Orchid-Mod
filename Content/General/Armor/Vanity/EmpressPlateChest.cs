using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Armor.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class EmpressPlateChest : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
