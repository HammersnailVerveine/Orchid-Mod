using System;
using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian.Projectiles.Gauntlets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class ThoriumGraniteGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 38;
			Item.knockBack = 10f;
			Item.damage = 100;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 20;
			strikeVelocity = 12f;
			parryDuration = 60;
			hasArm = true;
			hasShoulder = true;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(0, 192, 255);
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage)
		{
			if (charged && guardian.UseGuard(1, true))
			{
				bool deflect = false;
				bool instantExplode = true;
				Vector2 strikeVelocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.MountedCenter).ToRotation() - MathHelper.PiOver2) * 8;
				Vector2 strikeEndPosition = projectile.Center + strikeVelocity * 10;
				int punchDamage = guardian.GetGuardianDamage(Item.damage);
				int highestDeflectedDamage = 0;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile deflectProj = Main.projectile[i];
					if(deflectProj.active && deflectProj.hostile && deflectProj.damage > 0 && Collision.CheckAABBvLineCollision(deflectProj.position + deflectProj.velocity - new Vector2(16), new Vector2(deflectProj.width + 32, deflectProj.height + 32), projectile.Center, strikeEndPosition))
					{
						if (!deflect && guardian.UseGuard())
						{
							deflect = true;
							guardian.OnBlockProjectileFirst(projectile, deflectProj);
						}
						if (deflect)
						{
							instantExplode = false;
							guardian.OnBlockProjectile(projectile, deflectProj);
							if (deflectProj.damage > highestDeflectedDamage) highestDeflectedDamage = deflectProj.damage;
							deflectProj.Kill();
						}
					}
				}
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC deflectEnemy = Main.npc[i];
					if (deflectEnemy.active && !deflectEnemy.friendly && Collision.CheckAABBvLineCollision(deflectEnemy.position + deflectEnemy.velocity - new Vector2(16), new Vector2(deflectEnemy.width + 32, deflectEnemy.height + 32), projectile.Center, strikeEndPosition) && (deflectEnemy.lifeMax < 2 || (deflectEnemy.velocity - player.velocity).Length() > 6f))
					{
						if (!deflect && guardian.UseGuard())
						{
							deflect = true;
							guardian.OnBlockNPCFirst(projectile, deflectEnemy);
						}
						if (deflect)
						{
							guardian.OnBlockNPC(projectile, deflectEnemy);
							if (deflectEnemy.damage > highestDeflectedDamage) highestDeflectedDamage = deflectEnemy.damage;
							if (!deflectEnemy.dontTakeDamage)
							{
								NPC.HitInfo info = deflectEnemy.CalculateHitInfo(punchDamage, strikeVelocity.X > 1 ? 1 : -1, false, 1f, ModContent.GetInstance<GuardianDamageClass>());
								if (info.Damage >= deflectEnemy.life) instantExplode = false; 
								deflectEnemy.StrikeNPC(info);
							}
						}
					}
				}
				if (deflect)
				{
					SoundEngine.PlaySound(SoundID.Item37.WithPitchOffset(0.5f), player.Center);
					//SoundEngine.PlaySound(SoundID.Item106.WithPitchOffset(0.2f), player.Center);
					//SoundEngine.PlaySound(SoundID.Item72, player.Center);
					player.GetModPlayer<OrchidPlayer>().PlayerImmunity = player.immuneTime = InvincibilityDuration;
					player.immune = true;
					Projectile counterProj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), projectile.Center + strikeVelocity * 4, Vector2.Zero, ModContent.ProjectileType<ThoriumGraniteGauntletProjectile>(), Math.Clamp(highestDeflectedDamage, punchDamage, 1000), Item.knockBack, projectile.owner, instantExplode ? 1 : 0);
					counterProj.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					return false;
				}
			}
			return true;
		}
	}
}
