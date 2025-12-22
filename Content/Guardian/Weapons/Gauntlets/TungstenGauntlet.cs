using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class TungstenGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.knockBack = 3f;
			Item.damage = 60;
			Item.value = Item.sellPrice(0, 0, 12, 25);
			Item.rare = ItemRarityID.White;
			Item.useTime = 30;
			StrikeVelocity = 15f;
			ParryDuration = 60;
			PunchSpeed = 0.9f;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(202, 233, 207);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.TungstenBar, 8);
			recipe.Register();
		}
	}
}
