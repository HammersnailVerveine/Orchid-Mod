using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class IronPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 3, 40);
			Item.width = 28;
			Item.height = 38;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 7f;
			Item.damage = 27;
			Item.rare = ItemRarityID.White;
			Item.useAnimation = 30;
			Item.useTime = 30;
			distance = 35f;
			bashDistance = 90f;
			blockDuration = 80;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Iron Pavise");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.IronBar, 8);
			recipe.AddIngredient(ItemID.Wood, 6);
			recipe.Register();
		}
	}
}
