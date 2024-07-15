using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.Meteorite
{
	[AutoloadEquip(EquipType.Legs)]
	public class GuardianMeteoriteLegs : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 70, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianRecharge -= 0.1f;
			player.moveSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
