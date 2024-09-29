using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class DemoniteShield : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 52, 50);
			Item.width = 28;
			Item.height = 38;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 8f;
			Item.damage = 106;
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 40;
			distance = 45f;
			slamDistance = 85f;
			blockDuration = 160;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.Register();
		}
	}
}
