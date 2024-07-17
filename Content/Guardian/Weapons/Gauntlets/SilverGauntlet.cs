using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class SilverGauntlet : OrchidModGuardianGauntlet
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 3, 40);
			Item.width = 30;
			Item.height = 32;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 5f;
			Item.damage = 20;
			Item.rare = ItemRarityID.White;
			Item.useTime = 30;
			slamDistance = 45f;
			blockDuration = 80;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.SilverBar, 8);
			recipe.Register();
		}
	}
}
