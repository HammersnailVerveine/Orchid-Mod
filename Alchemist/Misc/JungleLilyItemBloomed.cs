using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Misc
{
	public class JungleLilyItemBloomed : OrchidModItem
	{

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.createTile = TileType<Tiles.Ambient.JungleLilyTile>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomed Jungle Lily");
			Tooltip.SetDefault("Gathered from a chemically bloomed jungle lily");
		}
	}
}