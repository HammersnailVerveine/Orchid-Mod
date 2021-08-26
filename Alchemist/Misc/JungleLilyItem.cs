using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Misc
{
	public class JungleLilyItem : OrchidModItem
	{

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.createTile = TileType<Tiles.Ambient.JungleLilyTile>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Bud");
			Tooltip.SetDefault("Maybe the chemist could help you making it bloom?");
		}
	}
}