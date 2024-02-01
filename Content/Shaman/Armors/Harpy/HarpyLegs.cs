using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.Harpy
{
	[AutoloadEquip(EquipType.Legs)]
	public class HarpyLegs : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 20, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.moveSpeed += 0.1f;
			player.GetDamage<ShamanDamageClass>() += 0.06f;
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
