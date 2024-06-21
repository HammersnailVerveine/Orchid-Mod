using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class GuardianEmblem : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage<GuardianDamageClass>() += 0.15f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.ReplaceResult(935);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
			recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}