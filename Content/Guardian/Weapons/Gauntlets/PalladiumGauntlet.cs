using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class PalladiumGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.knockBack = 6.5f;
			Item.damage = 252;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 21;
			StrikeVelocity = 24f;
			ParryDuration = 85;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(247, 183, 51);
		}

		//wanted to make it work for slamming to demonstrate ref charged but necessary variables are reset too early, oh well
		/*public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, ref bool charged, ref int damage)
		{
			if (!charged && (projectile.ModProjectile as GuardianGauntletAnchor).CanInstantSlam() && player.statLife > player.statLifeMax2 * 0.9f && player.statLife > 20)
			{
				player.statLife -= 20;
				CombatText.NewText(player.Hitbox, CombatText.DamagedFriendly, 20, false, true);
				SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, player.Center);
				charged = true;
			}
			return true;
		}*/

		public override bool PreGuard(Player player, OrchidGuardian guardian, Projectile anchor)
		{
			if (!guardian.UseGuard(1, true) && player.statLife > player.statLifeMax2 * 0.9f && player.statLife > 20)
			{
				player.statLife -= 20;
				CombatText.NewText(player.Hitbox, CombatText.DamagedFriendly, 20, false, true);
				SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, player.Center);
				return true;
			}
			return guardian.UseGuard(1);
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
