using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.Bamboo
{
	[AutoloadEquip(EquipType.Body)]
	public class GuardianBambooChest : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.White;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
			modPlayer.GuardianRecharge -= 0.15f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BambooBlock, 30);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
}
