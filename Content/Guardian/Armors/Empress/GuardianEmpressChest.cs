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
			Item.height = 24;
			Item.value = Item.sellPrice(0, 5, 10, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 36;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianRecharge -= 0.15f;
			player.GetDamage<GuardianDamageClass>() += 0.13f;
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
