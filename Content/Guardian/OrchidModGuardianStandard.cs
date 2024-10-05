﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianStandard : OrchidModGuardianItem
	{
		public int SlamStacks; // Stam Stacks given by the item
		public int GuardStacks; // Block Stacks given by the item
		public int FlagOffset; // Number of diagonal pixels from the top-right of the sprite to the base of the flag
		public float AuraRange; // Flag effect range in tiles
		public bool AffectNearbyPlayers; // Flag has an effect on nearby players
		public bool AffectNearbyNPCs; // Flag has an effect on nearby npcs
		public int StandardDuration; // Effect duration in ticks

		public virtual string ShaftTexture => Texture + "_Shaft";
		public virtual string FlagUpTexture => Texture + "_FlagUp";
		public virtual string FlagQuarterTexture => Texture + "_FlagQuarter";
		public virtual string FlagTwoQuarterTexture => Texture + "_FlagTwoQuarter";
		public virtual string FlagEndTexture => Texture + "_FlagEnd";
		public virtual void NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced) { } // isLocalPlayer is true when this is ran on the client being affected. Do not change stats on the affectedPlayer it won't work, use standardStats
		public virtual void NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced) { } // isLocalPlayer is true when this is ran by the guardian with the flag active
		public virtual void OnCharge(Player player, OrchidGuardian guardian) { }
		public virtual void EffectSimple(Player player, OrchidGuardian guardian) { }
		public virtual void EffectUpgrade(Player player, OrchidGuardian guardian) { }
		public virtual void ExtraAIStandard(Projectile projectile) { }
		public virtual void PostDrawStandard(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreDrawStandard(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }
		public virtual Color GetColor() => Color.White;
		public virtual bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (AffectNearbyPlayers && isPlayer && !PlayerisOwner) || (AffectNearbyNPCs && isNPC && isOwner); // Whether or not the aura should be drawn. This should cover most cases.

		public virtual void SafeHoldItem(Player player) { }

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTime = 30;
			Item.knockBack = 0f;
			Item.damage = 1;
			SlamStacks = 0;
			GuardStacks = 0;
			FlagOffset = 0;
			AuraRange = 0;
			StandardDuration = 1800; // 30 sec
			AffectNearbyPlayers = false;
			AffectNearbyNPCs = false;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();

			AuraRange *= 16f;
			Item.useAnimation = Item.useTime;
		}

		// public override bool WeaponPrefix() => true;

		/*
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool WeaponPrefix() => true;
		*/

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && !player.cursed)
			{
				var projectileType = ModContent.ProjectileType<GuardianStandardAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0)
				{
					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianStandardAnchor anchor && guardian.GuardianStandardCharge == 0f)
					{
						proj.ai[0] = 1f;
						anchor.NeedNetUpdate = true;
						guardian.GuardianStandardCharge++;
						SoundEngine.PlaySound(SoundID.Item7, player.Center);
					}
				}
			}
			return false;
		}

		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianStandardAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] == 0)
			{
				var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

				var proj = Main.projectile[index];
				if (proj.ModProjectile is not GuardianStandardAnchor standard)
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
				if (proj != null && proj.ModProjectile is GuardianStandardAnchor standard)
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

			if (GuardStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldStacks", "Grants " + this.GuardStacks + " guard charge" + (this.GuardStacks > 1 ? "s" : ""))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

			if (SlamStacks > 0)
			{
				tooltips.Insert(index + 1, new TooltipLine(Mod, "ShieldSlams", "Grants " + this.SlamStacks + " slam charge" + (this.SlamStacks > 1 ? "s" : ""))
				{
					OverrideColor = new Color(175, 255, 175)
				});
			}

			int tooltipSeconds = Math.DivRem((int)(StandardDuration * Main.LocalPlayer.GetModPlayer<OrchidGuardian>().GuardianRuneTimer), 60, out int tooltipTicks);
			index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "RuneDuration", tooltipSeconds + " seconds duration"));

			tooltips.RemoveAll(x => x.Name == "Damage" && x.Mod == "Terraria");
			tooltips.RemoveAll(x => x.Name == "Knockback" && x.Mod == "Terraria");
			tooltips.RemoveAll(x => x.Name == "CritChance" && x.Mod == "Terraria");
		}
	}

	public class GuardianStandardStats()
	{
		public int lifeMax = 0;
		public int lifeRegen = 0;
		public int defense;
		public float moveSpeed = 0f;
		public float guardianDamage = 0f;
		public float allDamage = 0f;

		public void ApplyStats(Player player)
		{
			player.statLifeMax2 += lifeMax;
			player.statDefense += defense;
			player.moveSpeed += moveSpeed;
			player.GetDamage<GuardianDamageClass>() += guardianDamage;
			player.GetDamage(DamageClass.Generic) += allDamage;

			lifeMax = 0;
			defense = 0;
			moveSpeed = 0f;
			guardianDamage = 0f;
			allDamage = 0f;
		}

		public void ApplyLifeRegen(Player player)
		{
			player.lifeRegen += lifeRegen;

			lifeRegen = 0;
		}
	}
}
