using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class MechanicalGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 40;
			Item.knockBack = 7.5f;
			Item.damage = 250;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 30;
			strikeVelocity = 25f;
			parryDuration = 75;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(223, 51, 51);
		}

		public override void HoldItemFrame(Player player)
		{
			player.noFallDmg = true;
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged)
		{
			if (charged)
			{
				strikeVelocity = 80f;
				Vector2 playerDashVelocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2) * strikeVelocity * 0.4f;
				guardian.modPlayer.ForcedVelocityVector = playerDashVelocity;
				guardian.modPlayer.ForcedVelocityTimer = 20;
				guardian.modPlayer.PlayerImmunity = 20;
				guardian.modPlayer.ForcedVelocityUpkeep = 0.1f;
			}
			else strikeVelocity = 25f;
			return true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.Register();
		}
	}
}
