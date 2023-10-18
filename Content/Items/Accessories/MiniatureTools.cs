using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Accessories
{
	public class MiniatureTools : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Salvaged Toolbox");
			/* Tooltip.SetDefault("Provides a small amount of light"
							  + "\nAllows you to drastically reduce trap damage, on a cooldown"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			modPlayer.generalTools = true;
			Lighting.AddLight((int)(player.Center.X / 16), (int)(player.Center.Y / 16), 0.2f, 0.10f, 0f);
		}
	}
}
