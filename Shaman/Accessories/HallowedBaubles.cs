using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class HallowedBaubles : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 12, 75);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hallowed Baubles");
			/* Tooltip.SetDefault("After completing an orb weapon cycle, you will be given a bonus orb on your next hit"
							  + "\nYou will also recover some health based on the number of orbs in the cycle"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TreasuredBaubles", 1);
			recipe.AddIngredient(ItemID.PixieDust, 10);
			recipe.AddIngredient(ItemID.UnicornHorn, 2);
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
