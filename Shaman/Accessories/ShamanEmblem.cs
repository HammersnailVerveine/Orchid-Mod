using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class ShamanEmblem : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = 4;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shaman Emblem");
			Tooltip.SetDefault("15% increased shamanic damage");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.15f;
		}
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(548, 5);
			recipe.AddIngredient(549, 5);
			recipe.AddIngredient(547, 5);
			recipe.AddTile(114);
			recipe.SetResult(935);
			recipe.AddRecipe();
		}
	}
}
