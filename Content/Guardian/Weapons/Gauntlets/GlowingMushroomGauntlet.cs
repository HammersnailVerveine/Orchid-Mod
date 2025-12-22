using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Gauntlets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class GlowingMushroomGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.knockBack = 1.5f;
			Item.damage = 49;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			StrikeVelocity = 15f;
			ParryDuration = 60;
			ChargeSpeedMultiplier = 3f;
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool manuallyFullyCharged, ref bool charged, ref int damage)
		{
			if (manuallyFullyCharged)
			{
				int projType = ModContent.ProjectileType<GlowingMushroomGauntletProjectile>();
				foreach (Projectile proj in Main.projectile)
				{
					if (proj.type == projType && proj.active && proj.owner == player.whoAmI)
					{
						proj.friendly = false;
						proj.ai[0] = 1f;
						proj.netUpdate = true;
					}
				}

				int sporeDamage = guardian.GetGuardianDamage(Item.damage * 0.33f);
				Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.MountedCenter).ToRotation() - MathHelper.PiOver2) * 6f;
				Projectile newprojectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), projectile.Center + velocity, velocity, projType, sporeDamage, 0f, player.whoAmI);
				newprojectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
				return false;
			}

			return true;
		}

		public override Color GetColor(bool offHand)
		{
			if (offHand) return new Color(176, 175, 150);
			return new Color(113, 146, 245);
		}
	}
}
