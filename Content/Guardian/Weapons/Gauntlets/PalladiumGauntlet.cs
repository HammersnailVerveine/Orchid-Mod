using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class PalladiumGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.knockBack = 5f;
			Item.damage = 243;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 15;
			strikeVelocity = 22.5f;
			parryDuration = 85;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(247, 183, 51);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.PalladiumBar, 10);
			recipe.Register();
		}
	}
}
