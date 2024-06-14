using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class TitaniumShield : OrchidModGuardianShield
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 2, 85, 0);
			Item.width = 34;
			Item.height = 44;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item73;
			Item.knockBack = 12f;
			Item.damage = 134;
			Item.rare = ItemRarityID.LightRed;
			Item.useAnimation = 25;
			Item.useTime = 25;
			distance = 55f;
			bashDistance = 240f;
			blockDuration = 240;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.TitaniumBar, 12);
			recipe.Register();
		}
	}
}
