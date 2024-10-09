using OrchidMod.Content.Guardian.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class GuardianBuffStation : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<GuardianBuffStationTile>());
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 2, 0, 0);
		}
	}
}
