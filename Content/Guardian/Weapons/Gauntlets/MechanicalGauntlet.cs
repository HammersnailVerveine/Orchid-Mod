using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class MechanicalGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 40;
			Item.knockBack = 4f;
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

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage)
		{
			if (player.HasBuff<GuardianMechanicalGauntletBuff>())
			{
				player.ClearBuff(ModContent.BuffType<GuardianMechanicalGauntletBuff>());
				SoundEngine.PlaySound(SoundID.Item14, player.Center);
				strikeVelocity = 45f;
				damage *= 2;
				Vector2 playerDashVelocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2) * strikeVelocity * 0.3f;
				guardian.modPlayer.ForcedVelocityVector = playerDashVelocity;
				guardian.modPlayer.ForcedVelocityTimer = 15;
				guardian.modPlayer.PlayerImmunity = 15;
				guardian.modPlayer.ForcedVelocityUpkeep = 0.3f;
			}
			else strikeVelocity = 25f;
			return true;
		}

		public override void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info)
		{
			player.AddBuff(ModContent.BuffType<GuardianMechanicalGauntletBuff>(), 60);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.Register();
		}
	}
}
