using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class ThoriumDeadwoodGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.knockBack = 3f;
			Item.damage = 27;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.White;
			Item.useTime = 20;
			StrikeVelocity = 13f;
			ParryDuration = 40;
			PunchSpeed *= 1.15f;
		}

		public override Color GetColor(bool offHand)
		{
			if (offHand) return new Color(156, 172, 174);
			return new Color(150, 45, 48);
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.WorkBenches);
				recipe.AddIngredient(thoriumMod, "Deadwood", 18);
				recipe.Register();
			}
		}
	}
}
