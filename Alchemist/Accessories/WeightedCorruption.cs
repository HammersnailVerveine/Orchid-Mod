using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace OrchidMod.Alchemist.Accessories
{
	public class WeightedCorruption : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Weighted Corruption");
			Tooltip.SetDefault("Increases alchemic main projectile velocity by 25%"
							+ "\nMaximum potency increased by 2");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayerAlchemist modPlayer = player.GetModPlayer<OrchidModPlayerAlchemist>();
			modPlayer.alchemistVelocity += 0.25f;
			modPlayer.alchemistPotencyMax += 2;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<Alchemist.Accessories.WeightedBottles>(), 1);
			recipe.AddIngredient(ItemType<Alchemist.Accessories.PreservedCorruption>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}