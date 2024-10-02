using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class CobaltGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.knockBack = 5f;
			Item.damage = 187;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 20;
			strikeVelocity = 22.5f;
			parryDuration = 75;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(113, 217, 251);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.Register();
		}
	}
}
