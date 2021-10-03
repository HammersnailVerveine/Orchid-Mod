using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Accessories
{
	public class AlchemistEmblem : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 4;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Emblem");
			Tooltip.SetDefault("15% increased chemical damage");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistDamage += 0.15f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
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
