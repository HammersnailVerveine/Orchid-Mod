using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.General.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class OrnateOrchid : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.vanity = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornate Orchid");
			Tooltip.SetDefault("'Great for impersonating Orchid Devs!'");
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)/* tModPorter Note: Removed. In SetStaticDefaults, use ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true if you had drawHair set to true, and ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true if you had drawAltHair set to true */
		{
			drawHair = true;
			drawAltHair = false;
		}

		public override bool DrawHead()/* tModPorter Note: Removed. In SetStaticDefaults, use ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false if you returned false */ => true;
	}
}
