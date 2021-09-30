using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.Content.Items.Materials
{
	public class JungleLilyBloomed : OrchidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomed Jungle Lily");
			Tooltip.SetDefault("Gathered from a chemically bloomed jungle lily");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 99;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 0, 5, 0);
		}
	}
}