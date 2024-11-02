using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.General.Prefixes;
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
		public virtual void SlamHitFirst(Player player, Projectile shield, NPC npc) { } // Called up to once upon hitting an enemy when slamming
		public virtual void SlamHit(Player player, Projectile shield, NPC npc) { } // Called upon hitting an enemy when slamming
		public virtual void Slam(Player player, Projectile shield) { } // Called upon slamming
		public virtual void Push(Player player, Projectile shield, NPC npc) { } // Called upon pushing an enemy while blocking
		public virtual void Protect(Player player, Projectile shield) { } // Called up to once when the first projectile is blocked or enemy pushed
		public virtual void Block(Player player, Projectile shield, Projectile projectile) {  // Called when any projectile is blocked
			projectile.Kill();
		}
		public virtual void BlockStart(Player player, Projectile shield) { }  // Called when the player starts blocking (as they press the left click)

		public float distance = 100f;
		public float slamDistance = 100f;
		public int blockDuration = 60;
		/// <summary>Causes the shield's held sprite to flip when facing right.</summary>
		public bool shouldFlip = false;

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

					bool shouldSlam = player.altFunctionUse != 2;
					if (ModContent.GetInstance<OrchidClientConfig>().SwapPaviseImputs) shouldSlam = !shouldSlam;

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
								shield.NeedNetUpdate = true;
							}
						} else { // Block
							if (proj.ai[1] + proj.ai[0] == 0f && guardian.UseGuard(1, true)) 
							{
								shield.shieldEffectReady = true;
								guardian.UseGuard();
								proj.ai[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration());
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
									proj.ai[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration());
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
					proj.localAI[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration()); // Used for UI display
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
						proj.localAI[0] = (int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration()); // Used for UI display
						shield.OnChangeSelectedItem(player);
					}
				}
			}
			this.SafeHoldItem(player);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}

			int tooltipSeconds = Math.DivRem((int)(blockDuration * Item.GetGlobalItem<GuardianPrefixItem>().GetBlockDuration()), 60, out int tooltipTicks);

			int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
			tooltips.Insert(index + 1, new TooltipLine(Mod, "BlockDuration", tooltipSeconds + "." + (int)(tooltipTicks * (100 / 60f)) + " block duration"));

			string click = ModContent.GetInstance<OrchidClientConfig>().SwapPaviseImputs ? "Left" : "Right";
			tooltips.Insert(index + 2, new TooltipLine(Mod, "ClickInfo", click + " click to block")
			{
				OverrideColor = new Color(175, 255, 175)
			});
		}
	}
}
