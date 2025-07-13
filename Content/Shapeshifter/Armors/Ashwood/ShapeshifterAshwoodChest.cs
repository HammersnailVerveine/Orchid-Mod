using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Armors.Ashwood
{
	[AutoloadEquip(EquipType.Body)]
	public class ShapeshifterAshwoodChest : OrchidModShapeshifterItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage<ShapeshifterDamageClass>() += 0.7f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AshWood, 30);
			recipe.AddIngredient(ItemID.HellstoneBar, 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
