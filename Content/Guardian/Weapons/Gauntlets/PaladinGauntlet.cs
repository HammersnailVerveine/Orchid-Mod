using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Projectiles.Gauntlets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class PaladinGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 40;
			Item.knockBack = 6f;
			Item.damage = 392;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 30;
			strikeVelocity = 25f;
			parryDuration = 120;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(238, 218, 200);
		}

		public override void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info)
		{
			player.AddBuff(ModContent.BuffType<GuardianPaladinGauntletBuff>(), 600);
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged)
		{
			if (charged)
			{
				int projectileType = ModContent.ProjectileType<PaladinGauntletProjectile>();
				for (int i = 0; i < 10 + Main.rand.Next(4); i++)
				{
					float speed = strikeVelocity * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetSlamDistance() * Main.rand.NextFloat(0.55f, 0.8f);
					Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2).RotatedByRandom(MathHelper.ToRadians(40));
					int damage = (int)player.GetDamage<GuardianDamageClass>().ApplyTo(Item.damage * 0.35f);
					Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity * speed, projectileType, damage, Item.knockBack, player.whoAmI, 1f);
					newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
					newProjectile.rotation = newProjectile.velocity.ToRotation();
					newProjectile.velocity += player.velocity * 1.5f;
					newProjectile.netUpdate = true;
				}

				SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, player.Center);
				SoundEngine.PlaySound(SoundID.Item110, player.Center);
				return false;
			}
			else if (player.HasBuff<GuardianPaladinGauntletBuff>())
			{
				int projectileType = ModContent.ProjectileType<PaladinGauntletProjectile>();
				for (int i = 0; i < 2 + Main.rand.Next(2); i ++)
				{
					float speed = strikeVelocity * Item.GetGlobalItem<Prefixes.GuardianPrefixItem>().GetSlamDistance() * Main.rand.NextFloat(0.5f, 0.65f);
					Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2).RotatedByRandom(MathHelper.ToRadians(5));
					int damage = (int)player.GetDamage<GuardianDamageClass>().ApplyTo(Item.damage * 0.35f);
					Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity * speed, projectileType, damage, Item.knockBack, player.whoAmI);
					newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
					newProjectile.rotation = newProjectile.velocity.ToRotation();
					newProjectile.velocity += player.velocity * 1.5f;
					newProjectile.netUpdate = true;
				}

				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, player.Center);
				SoundEngine.PlaySound(SoundID.Item110, player.Center);
				return false;
			}
			return true;
		}
	}
}
