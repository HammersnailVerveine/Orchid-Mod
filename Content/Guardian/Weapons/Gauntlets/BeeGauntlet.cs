using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.General.Prefixes;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class BeeGauntlet : OrchidModGuardianGauntlet
	{
		public int FlightAnimation = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.knockBack = 3f;
			Item.damage = 80;
			Item.value = Item.sellPrice(0, 0, 70, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 20;
			StrikeVelocity = 20f;
			ParryDuration = 30;
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool fullyManuallyCharged, ref bool charged, ref int damage)
		{
			if (charged)
			{
				int beesInMyMouth = 3 + Main.rand.Next(3); // 3-5 (4 avg for 100% of Item.damage dealt on slam)

				if (player.strongBees && Main.rand.NextBool(2)) // 50% chance of 1 more bee with Hive Pack
				{
					beesInMyMouth ++;
				}

				for (int i = 0; i < beesInMyMouth; i++)
				{
					float speed = StrikeVelocity * Item.GetGlobalItem<GuardianPrefixItem>().GetSlamDistance() * Main.rand.NextFloat(0.3f, 0.5f);
					Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2).RotatedByRandom(MathHelper.ToRadians(20)) * speed + player.velocity * 0.75f;
					int beeDamage = guardian.GetGuardianDamage(Item.damage * 0.25f); // bees deal 25% of weapon damage so the 4 bees spawned on average deals normal slam damage

					Projectile newProjectile = null;
					if (player.strongBees && Main.rand.NextBool(2))
					{
						newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, ProjectileID.GiantBee, (int)(beeDamage * 1.15f), 0f, player.whoAmI);
					}
					else
					{
						newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, ProjectileID.Bee, beeDamage, 0f, player.whoAmI);
					}

					newProjectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
					newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
					newProjectile.rotation = newProjectile.velocity.ToRotation();
					newProjectile.netUpdate = true;
				}

				for (int i = 0; i < Main.rand.Next(5, 8); i++)
				{
					float speed = StrikeVelocity * Item.GetGlobalItem<GuardianPrefixItem>().GetSlamDistance() * Main.rand.NextFloat(0.1f, 0.3f);
					Vector2 velocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2).RotatedByRandom(MathHelper.ToRadians(20)) * speed + player.velocity * 0.75f;
					Dust dust = Dust.NewDustDirect(projectile.Center, 4, 4, DustID.Honey2);
					dust.noGravity = true;
					dust.velocity = velocity;
				}

				guardian.GuardianGuardRecharging += 0.5f; // because it doesn't shoot a proper slam projectile

				SoundStyle soundStyle = SoundID.Item97;
				soundStyle.Pitch *= Main.rand.NextFloat(1.4f, 1.8f);
				SoundEngine.PlaySound(soundStyle, player.Center);
				return false;
			}
			return true;
		}

		public override void OnParryGauntlet(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
		{
			int beesInMyPockets = 1 + Main.rand.Next(3); // identical to Honey Comb

			if (player.strongBees && Main.rand.NextBool(3))
			{
				beesInMyPockets ++;
			}

			for (int i = 0; i < beesInMyPockets; i++)
			{
				int beeDamage = guardian.GetGuardianDamage(Item.damage * 0.25f);

				Projectile newProjectile = null;
				if (player.strongBees && Main.rand.NextBool(2))
				{
					newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), player.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.GiantBee, (int)(beeDamage * 1.15f), 0f, player.whoAmI);
				}
				else
				{
					newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), player.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.Bee, beeDamage, 0f, player.whoAmI);
				}

				newProjectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
				newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
				newProjectile.rotation = newProjectile.velocity.ToRotation();
				newProjectile.netUpdate = true;
			}

			SoundStyle soundStyle = SoundID.Item97;
			soundStyle.Pitch *= Main.rand.NextFloat(0.4f, 0.8f);
			SoundEngine.PlaySound(soundStyle, player.Center);
		}

		public override void ExtraAIGauntlet(Player player, OrchidGuardian guardian, Projectile anchor, bool offHandGauntlet)
		{
			if (!offHandGauntlet)
			{
				if (guardian.GuardianItemCharge > 0)
				{
					FlightAnimation = (FlightAnimation + 1) % 6;

					player.noFallDmg = true;
					player.fallStart = (int)(player.position.Y / 16);
					bool fastFalling = player.controlDown;
					if (player.gravDir == -1) fastFalling = player.controlUp;

					if (!fastFalling && player.velocity.Y * player.gravDir > player.maxFallSpeed / 6)
					{
						player.velocity.Y = Math.Min(player.velocity.Y, player.maxFallSpeed / 3 * player.gravDir);
					}
				}
				else
				{
					FlightAnimation = 0;
				}
			}
		}

		public override Texture2D GetGauntletTexture(Player player, Projectile anchor, bool OffHandGauntlet, out Rectangle? drawRectangle)
		{
			Texture2D texture = ModContent.Request<Texture2D>(GauntletTexture).Value;
			Rectangle rectangle = texture.Bounds;
			rectangle.Height = rectangle.Height / 2;

			if (FlightAnimation > 2)
			{ // bzzz
				rectangle.Y = rectangle.Height;
			}

			drawRectangle = rectangle;
			return texture;
		}

		public override Color GetColor(bool offHand)
		{
			if (offHand) return new Color(139, 109, 191);
			return new Color(254, 194, 20);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.BeeWax, 14);
			recipe.Register();
		}
	}
}
