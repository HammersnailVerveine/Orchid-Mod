using OrchidMod.Content.Guardian.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.Empress
{
	[AutoloadEquip(EquipType.Body)]
	public class GuardianEmpressChest : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 5, 10, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 25;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.GetDamage<GuardianDamageClass>() += 0.13f;
			modPlayer.GuardianRecharge -= 0.15f;
			modPlayer.GuardianSlamMax += 2;
			player.aggro += 250;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GuardianEmpressMaterial>(12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
