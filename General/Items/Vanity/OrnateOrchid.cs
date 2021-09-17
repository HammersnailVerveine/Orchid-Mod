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
			item.width = 26;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.vanity = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornate Orchid");
			Tooltip.SetDefault("'Great for impersonating Orchid Devs!'");
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
			drawAltHair = false;
		}

		public override bool DrawHead() => true;
	}
}
