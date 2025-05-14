using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Armors.Harpy
{
	[AutoloadEquip(EquipType.Legs)]
	public class ShapeshifterHarpyLegs : OrchidModShapeshifterItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 20, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();
			player.moveSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 6);
			recipe.AddIngredient(ItemID.ShadowScale, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 6);
			recipe.AddIngredient(ItemID.TissueSample, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
