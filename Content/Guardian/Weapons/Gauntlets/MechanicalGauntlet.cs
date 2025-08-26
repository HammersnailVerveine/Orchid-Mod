﻿using System;
using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Projectiles.Gauntlets;
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

		bool hover;

		public override void HoldItemFrame(Player player)
		{
			player.noFallDmg = true;
			player.fallStart = (int)(player.position.Y / 16);
			bool fastFalling = player.controlDown;
			bool slowFalling = player.controlUp;
			if (player.gravDir == -1) (fastFalling, slowFalling) = (slowFalling, fastFalling);
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			hover = !fastFalling && guardian.GuardianGauntletCharge > 0 && player.velocity.Y * player.gravDir > player.maxFallSpeed / 6;
			if (hover)
			{
				player.velocity.Y = Math.Min(player.velocity.Y, player.maxFallSpeed / (slowFalling ? 10 : 3) * player.gravDir);
			}
		}

		public override void ExtraAIGauntlet(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			if (projectile.ModProjectile is GuardianGauntletAnchor anchor && !anchor.OffHandGauntlet)
			{
				//checking for Ding determines whether it is a natural full charge or an instant one, since instant charges do not ding
				if (player.GetModPlayer<OrchidGuardian>().GuardianCounterTime > 0 ||  anchor.Ding)
				{
					Dust dust = Dust.NewDustPerfect(projectile.Center + new Vector2(5, -3 * player.direction).RotatedBy(projectile.rotation), DustID.TheDestroyer, Vector2.Zero);
					dust.noGravity = true;
				}
				if (hover)
				{
					Dust.NewDustPerfect(projectile.Center + new Vector2(1, 7).RotatedBy(projectile.rotation), DustID.Torch, new Vector2(-player.direction, player.gravDir * 2f + player.velocity.Y) + Main.rand.NextVector2Circular(1f, 0.5f), Scale: player.velocity.Y < 2f * player.gravDir ? 2f : 1.25f).noGravity = true;
				}
			}
		}

		public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool charged, ref int damage)
		{
			if (guardian.GuardianCounterTime > 0 || (projectile.ModProjectile is GuardianGauntletAnchor anchor && anchor.Ding))
			{
				guardian.GuardianCounterTime = 0;
				SoundEngine.PlaySound(SoundID.Item14, player.Center);
				Vector2 playerDashVelocity = Vector2.UnitY.RotatedBy((Main.MouseWorld - player.Center).ToRotation() - MathHelper.PiOver2) * 15f;
				guardian.modPlayer.ForcedVelocityVector = playerDashVelocity;
				guardian.modPlayer.ForcedVelocityTimer = 20;
				guardian.modPlayer.PlayerImmunity = 20;
				guardian.modPlayer.ForcedVelocityUpkeep = 0.3f;

				int projectileType = ModContent.ProjectileType<MechanicalGauntletProjectile>();
				Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, guardian.modPlayer.ForcedVelocityVector, projectileType, guardian.GetGuardianDamage(Item.damage) * 2, Item.knockBack, player.whoAmI);
				newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);

				for (int i = 0; i < 20; i++)
				{
					Dust dust = Dust.NewDustDirect(player.position - new Vector2(4, -4), player.width, player.height, DustID.Torch, -playerDashVelocity.X, -playerDashVelocity.Y * 2f);
					dust.scale = Main.rand.NextFloat(0.5f, 1.2f);
					dust.velocity += Main.rand.NextVector2Circular(2f, 2f);
					if (Main.rand.NextBool())
					{
						dust.noGravity = true;
						dust.scale += 1 + Main.rand.NextFloat(2f);
					}
					dust.scale *= 0.5f + i / 20f;
					dust.velocity *= 2f - i / 20f;
				}
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 15);
			recipe.AddIngredient(ItemID.SoulofFlight, 8);
			recipe.Register();
		}
	}
}
