using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Misc
{
	public class HorizonLance : OrchidModGuardianParryItem
	{
		public float AuraRange; // Flag effect range in tiles
		public int StandardDuration; // Effect duration in ticks
		public int ParryDuration;
		public virtual string LanceTexture => Texture;
		public virtual string LanceTextureGlow => Texture + "_Glow";

		public virtual void SafeHoldItem(Player player) { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.UseSound = SoundID.DD2_GhastlyGlaiveImpactGhost;
			Item.useAnimation = Item.useTime;


			Item.width = 46;
			Item.height = 46;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.useTime = 20;
			Item.knockBack = 8f;
			Item.damage = 618;
			AuraRange = 480; // 30 * 16
			StandardDuration = 1800; // 30 sec
			ParryDuration = 90;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;
		}

		public override void OnParry(Player player, OrchidGuardian guardian, Player.HurtInfo info) 
		{
			guardian.modPlayer.TryHeal(20);
			SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, player.Center);
			info.DamageSource.TryGetCausingEntity(out Entity entity);

			Vector2 offset = Vector2.UnitY * 70f;

			if (entity != null)
			{
				if (entity is NPC npc)
				{
					offset = offset.RotatedBy((npc.Center - player.Center).ToRotation() - MathHelper.PiOver2);
				}

				if (entity is Projectile projectile)
				{
					offset = offset.RotatedBy((projectile.Center - player.Center).ToRotation() - MathHelper.PiOver2);
				}
			}
			else 
			{
				offset = offset.RotatedByRandom(MathHelper.Pi);
			}

			var projectileType = ModContent.ProjectileType<GuardianHorizonLanceProjAlt>();
			int damage = guardian.GetGuardianDamage(Item.damage);
			Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), player.Center + offset, offset * 0.0001f, projectileType, damage, Item.knockBack, player.whoAmI, offset.ToRotation());
			newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
		}

		public override bool WeaponPrefix() => true;

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && !player.cursed)
			{
				var projectileType = ModContent.ProjectileType<GuardianHorizonLanceAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0)
				{

					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianHorizonLanceAnchor anchor && proj.ai[0] >= 0f)
					{
						bool shouldBlock = Main.mouseRight && Main.mouseRightRelease;
						bool shouldCharge = Main.mouseLeft && Main.mouseLeftRelease;

						if (ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs)
						{
							shouldBlock = Main.mouseLeft && Main.mouseLeftRelease;
							shouldCharge = Main.mouseRight && Main.mouseRightRelease;
						}

						if (shouldBlock && guardian.UseGuard(1, true) && proj.ai[0] <= 1f)
						{
							player.immuneTime = 0;
							player.immune = false;
							guardian.modPlayer.PlayerImmunity = 0;
							guardian.UseGuard(1);
							proj.ai[0] = ParryDuration + 1f;
							anchor.NeedNetUpdate = true;
							SoundEngine.PlaySound(SoundID.Item37, player.Center);
						}

						if (shouldCharge && guardian.GuardianStandardCharge == 0f)
						{
							proj.ai[0] = 1f;
							anchor.NeedNetUpdate = true;
							guardian.GuardianStandardCharge++;
							SoundEngine.PlaySound(SoundID.Item7, player.Center);
						}
					}
				}
			}
			return false;
		}

		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianHorizonLanceAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] != 1)
			{
				foreach (Projectile projectile in Main.projectile)
				{
					if (projectile.active && projectile.owner == player.whoAmI && projectile.type == projectileType)
					{
						projectile.Kill();
					}
				}

				var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

				var proj = Main.projectile[index];
				if (proj.ModProjectile is not GuardianHorizonLanceAnchor standard)
				{
					proj.Kill();
				}
				else
				{
					standard.OnChangeSelectedItem(player);
				}
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianHorizonLanceAnchor standard)
				{
					if (standard.SelectedItem != player.selectedItem)
					{
						standard.OnChangeSelectedItem(player);
					}
				}
			}
			SafeHoldItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));

			int tooltipSeconds = Math.DivRem((int)(ParryDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration()), 60, out int tooltipTicks);
			tooltips.Insert(index + 1, new TooltipLine(Mod, "ParryDuration", tooltipSeconds + "." + (int)(tooltipTicks * (100 / 60f)) + " parry duration"));

			tooltipSeconds = Math.DivRem((int)(StandardDuration * Main.LocalPlayer.GetModPlayer<OrchidGuardian>().GuardianRuneTimer), 60, out tooltipTicks);
			index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 2, new TooltipLine(Mod, "RuneDuration", tooltipSeconds + " seconds duration"));

			tooltips.Insert(index + 3, new TooltipLine(Mod, "ShieldStacks", "Grants 3 guard charges")
			{
				OverrideColor = new Color(175, 255, 175)
			});

			string click = ModContent.GetInstance<OrchidClientConfig>().SwapGauntletImputs ? "Left" : "Right";
			tooltips.Insert(index + 4, new TooltipLine(Mod, "ClickInfo", click + " click to parry")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
