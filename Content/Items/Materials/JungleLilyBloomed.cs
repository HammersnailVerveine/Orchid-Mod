using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using OrchidMod.Assets;

namespace OrchidMod.Content.Items.Materials
{
	public class JungleLilyBloomed : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bloomed Jungle Lily");
			// Tooltip.SetDefault("Gathered from a chemically bloomed jungle lily");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 5, 0);

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<JungleLilyTile>();
		}
	}
}