using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Armors.Harpy
{
	[AutoloadEquip(EquipType.Body)]
	public class ShapeshifterHarpyChest : OrchidModShapeshifterItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 25, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 8);
			recipe.AddIngredient(ItemID.ShadowScale, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 8);
			recipe.AddIngredient(ItemID.TissueSample, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
