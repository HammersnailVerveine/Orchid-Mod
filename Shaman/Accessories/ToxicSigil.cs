using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class ToxicSigil : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Sigil");
			Tooltip.SetDefault("Your shamanic fire bonds allows you to poison and envenom your foes on hit");

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanVenom = true;
			modPlayer.shamanPoison = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<VenomSigil>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PoisonSigil>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
