using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Accessories
{
	public class WeightedCrimson : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = 1;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weighted Crimson");
			Tooltip.SetDefault("Increases alchemic main projectile velocity by 25%"
							+ "\nMaximum potency increased by 2");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistVelocity += 0.25f;
			modPlayer.alchemistPotencyMax += 2;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(ItemType<Alchemist.Accessories.WeightedBottles>(), 1);
			recipe.AddIngredient(ItemType<Alchemist.Accessories.PreservedCrimson>(), 1);
			recipe.AddTile(114);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}