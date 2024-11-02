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
			Item.damage = 201;
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 30;
			distance = 55f;
			slamDistance = 120f;
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
