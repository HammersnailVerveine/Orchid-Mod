using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Accessories
{
	public class BurningClovers : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Clovers");
			Tooltip.SetDefault("Gambler dice will slow down when rolling 5 or more"
							 + "\nGambler dice cannot roll a 1 or a 2");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			if (modPlayer.gamblerDieValueCurrent > 4) modPlayer.gamblerDieAnimationPause += 20;
			modPlayer.gamblerImp = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<ImpDiceCup>(), 1);
			recipe.AddIngredient(ItemType<BundleOfClovers>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}