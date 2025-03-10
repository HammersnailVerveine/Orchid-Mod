using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class OrchidModGuardianShield : OrchidModGuardianItem
	{
		public virtual string ShieldTexture => Texture + "_Shield";
		public virtual void ExtraAIShield(Projectile projectile) { }
		public virtual void PostDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }

		public virtual void SafeHoldItem(Player player) { }
		/// <summary>Called once per slam, when the slam first hits an enemy.</summary>
		public virtual void SlamHitFirst(Player player, Projectile shield, NPC npc) { }
		/// <summary>Called when this shield's slam hits an enemy.</summary>
		public virtual void SlamHit(Player player, Projectile shield, NPC npc) { }
		/// <summary>Called on the first frame of a slam.</summary>
		public virtual void Slam(Player player, Projectile shield) { }
		/// <summary>Called when an enemy collides with the shield during a block. Will be called once per frame per enemy colliding with it.</summary>
		public virtual void Push(Player player, Projectile shield, NPC npc) { }
		/// <summary>Called once per block when the first enemy or projectile is blocked. This is called after <c>Push</c> or <c>Block</c>, but before <c>Block</c> destroys the projectile.</summary>
		public virtual void Protect(Player player, Projectile shield) { }
		/// <summary>Called when a projectile collides with the shield during a block. Return <c>true</c> to destroy the projectile. Defaults to <c>true</c>.</summary>
		/// <returns>Whether to destroy the projectile.</returns>
		public virtual bool Block(Player player, Projectile shield, Projectile projectile) { return true; }
		/// <summary>Called on the first frame of a block.</summary>
		public virtual void BlockStart(Player player, Projectile shield) { }

		public float distance = 100f;
		public float slamDistance = 100f;
		public int blockDuration = 60;
		/// <summary>Causes the shield's held sprite to flip when facing right.</summary>
		public bool shouldFlip = false;
		public bool slamAutoReuse = true;

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
			Item.knockBack = 6f;

			OrchidGlobalItemPerEntity orchidItem = Item.GetGlobalItem<OrchidGlobalItemPerEntity>();
			orchidItem.guardianWeapon = true;

			SafeSetDefaults();
			Item.useAnimation = Item.useTime;
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
				var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
				if (player.ownedProjectileCounts[projectileType] > 0) {
					var guardian = player.GetModPlayer<OrchidGuardian>();
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);

					bool shouldBlock = Main.mouseRight && Main.mouseRightRelease;
					bool shouldSlam = Main.mouseLeft && (Main.mouseLeftRelease || slamAutoReuse);
					if (ModContent.GetInstance<OrchidClientConfig>().SwapPaviseImputs)
					{
						shouldBlock = Main.mouseLeft && Main.mouseLeftRelease;
						shouldSlam = Main.mouseRight && (Main.mouseRightRelease || slamAutoReuse);
					}

					if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
					{
						if (shouldSlam) { // Slam
							if (proj.ai[1] == 0f && guardian.UseSlam(1, true)) 
							{
								SoundEngine.PlaySound(Item.UseSound, player.Center);
								guardian.UseSlam();
								shield.shieldEffectReady = true;
								proj.ai[1] = 60f;
								if (proj.ai[0] > 0f) 
								{
									shield.spawnDusts();
									proj.ai[0] = 0f;
									resetBlockedEnemiesDuration(guardian);
								}
								proj.ResetLocalNPCHitImmunity();
								shield.NeedNetUpdate = true;
							}
						} else if (shouldBlock) { // Block
							if (proj.ai[1] + proj.ai[0] == 0f && guardian.UseGuard(1, true)) 
							{
								shield.shieldEffectReady = true;
								guardian.UseGuard();
								proj.ai[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianBlockDuration);
								shield.NeedNetUpdate = true;
								BlockStart(player, proj);
								SoundEngine.PlaySound(SoundID.Item1, player.Center);
							}
							else if (proj.ai[0] > 0f && Main.mouseLeftRelease) // Remove block stance if click again
							{
								if (ModContent.GetInstance<OrchidClientConfig>().BlockCancelChain && guardian.UseGuard(1, true))
								{
									// Taken from the shield anchor code
									Vector2 aimedLocation = Main.MouseWorld - player.Center.Floor();
									aimedLocation.Normalize();
									proj.velocity = aimedLocation * float.Epsilon;
									aimedLocation *= -distance;
									proj.rotation = aimedLocation.ToRotation();
									proj.direction = proj.spriteDirection;
									aimedLocation = player.Center.Floor() - aimedLocation - new Vector2(proj.width / 2f, proj.height / 2f);
									proj.position = aimedLocation;
									shield.aimedLocation = aimedLocation;
									proj.ai[2] = proj.rotation; // networked rotation

									shield.shieldEffectReady = true;
									guardian.UseGuard();
									proj.ai[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianBlockDuration);
									shield.NeedNetUpdate = true;
									BlockStart(player, proj);
									SoundEngine.PlaySound(SoundID.Item1, player.Center);
								}
								else
								{
									shield.shieldEffectReady = true;
									shield.spawnDusts();
									proj.ai[0] = 0f;
									shield.NeedNetUpdate = true;
								}
							} 
						}
					}
				}
			}
			return false;
		}

		public GuardianShieldAnchor GetAnchor(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
				{
					return shield;
				}
			}
			return null;
		}

		public void resetBlockedEnemiesDuration(OrchidGuardian modPlayer) {
			for (int i = modPlayer.GuardianBlockedEnemies.Count - 1; i >= 0; i--)
			{
				BlockedEnemy blockedEnemy = modPlayer.GuardianBlockedEnemies[i];
				blockedEnemy.time = blockedEnemy.time < 60 ? blockedEnemy.time : 60;
			}
		}
		
		public sealed override void HoldItem(Player player)
		{
			var projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			var guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianDisplayUI = 300;

			if (player.ownedProjectileCounts[projectileType] == 0)
			{
				var index = Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, projectileType, 0, 0f, player.whoAmI);

				var proj = Main.projectile[index];
				if (proj.ModProjectile is not GuardianShieldAnchor shield)
				{
					proj.Kill();
				}
				else
				{
					proj.damage = guardian.GetGuardianDamage(Item.damage);
					proj.CritChance = guardian.GetGuardianCrit(Item.crit);
					proj.knockBack = Item.knockBack;
					proj.localAI[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianBlockDuration); // Used for UI display
					shield.OnChangeSelectedItem(player);
				}
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj != null && proj.ModProjectile is GuardianShieldAnchor shield)
				{
					if (shield.SelectedItem != player.selectedItem)
					{
						proj.localAI[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianBlockDuration); // Used for UI display
						shield.OnChangeSelectedItem(player);
					}
				}
			}
			this.SafeHoldItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var guardian = Main.LocalPlayer.GetModPlayer<OrchidGuardian>();
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "BlockDuration", Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.BlockDuration", OrchidUtils.FramesToSeconds((int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration() * guardian.GuardianBlockDuration)))));

			string click = ModContent.GetInstance<OrchidClientConfig>().SwapPaviseImputs ? Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.LeftClick") : Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.RightClick");
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ClickInfo", Language.GetTextValue("Mods.OrchidMod.UI.GuardianItem.Block", click))
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
