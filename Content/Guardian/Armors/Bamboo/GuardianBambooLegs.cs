using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.Bamboo
{
	[AutoloadEquip(EquipType.Legs)]
	public class GuardianBambooLegs : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianRecharge -= 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BambooBlock, 30);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
