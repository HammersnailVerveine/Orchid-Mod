using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Buffs.Debuffs;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using OrchidMod.Content.Guardian.Projectiles.Standards;
using OrchidMod.Content.Guardian.Weapons.Misc;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidGuardian : ModPlayer
	{
		public OrchidPlayer modPlayer;

		// Can be edited by gear

		public float GuardianRecharge = 1f; // Natural guardian slam/guard regeneration multiplier
		public int GuardianGuardMax = 3; // Max guard charges
		public int GuardianSlamMax = 3; // Max slam charges
		public int GuardianBonusRune = 0; // Bonus projectiles spawned by runes
		public float GuardianRuneTimer = 1f; // Rune duration multiplier
		public float GuardianStandardTimer = 1f; // Standard duration multiplier
		public float GuardianSlamDistance = 1f; // Slam Distance multiplier
		public float GuardianBlockDuration = 1f; // Block Duration multiplier

		// Set effects, accessories, misc

		public bool GuardianMeteorite = false; // Armor Sets
		public bool GuardianBamboo = false;
		public bool GuardianGit = false;
		public bool GuardianHorizon = false;
		public bool GuardianSpikeGoblin = false; // Accessories
		public bool GuardianSpikeDungeon = false;
		public bool GuardianSpikeMechanical = false;
		public bool GuardianSpikeTemple = false;
		public bool GuardianWormTooth = false;
		public bool GuardianMonsterFang = false;
		public bool GuardianStandardDesert = false; // Standards
		public bool GuardianHoneyPotion = false; // Misc

		// Dynamic gameplay and UI fields

		public GuardianStandardStats GuardianStandardStats = new GuardianStandardStats(); // Used to receive stats from standards
		public int GuardianGuard = 0; // Current Guard stacks
		public int GuardianSlam = 0; // Current Slam Stacks
		public int GuardianGuardRecharge = 0; // Current timer for guard stack recharge
		public int GuardianSlamRecharge = 0; // Current timer for slam stack recharge
		public int GuardianDisplayUI = 0; // Guardian UI is displayed if > 0
		public float GuardianHammerCharge = 0f; // Player Warhammer Throw Charge, max is 180f
		public float GuardianGauntletCharge = 0f; // Player Gauntlet Punch Charge, max is 180f
		public float GuardianStandardCharge = 0f; // Player Standard Charge, max is 180f
		public float GuardianRuneCharge = 0f; // Player Rune Charge, max is 180f
		public bool GuardianGauntletParry = false; // Player is currently parrying with a gauntlet
		public bool GuardianGauntletParry2 = false; // Player is currently parrying with a gauntlet (1 frame buffer)
		public bool GuardianStandardBuffer = false; // used to delay the deactivation of various standards effects by 1 frame
		public int SlamCostUI = 0; // Displays an outline around slams in the UI if > 0
		public List<BlockedEnemy> GuardianBlockedEnemies = new List<BlockedEnemy>();
		public List<Projectile> RuneProjectiles = new List<Projectile>();
		public Projectile GuardianCurrentStandardAnchor;

		public static int GuardianRechargeTime = 600;

		public int GetGuardianDamage(float damage) => (int)(Player.GetDamage<GuardianDamageClass>().ApplyTo(damage) + Player.GetDamage(DamageClass.Generic).ApplyTo(damage) - damage);

		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			Item item = Player.HeldItem;
			if (item.ModItem is OrchidModGuardianGauntlet gauntlet)
			{
				drawInfo.compBackArmFrame = Rectangle.Empty; // Makes the back arm disappear when holding a gauntlet
				if (gauntlet.hasArm)
				{ // Makes the front arm disappear if the gauntlet has its own arm texture.
					drawInfo.compFrontArmFrame = new Rectangle(1, 1, 1 ,1); // An empty rectangle crashes the game
				}

				if (gauntlet.hasShoulder)
				{
					drawInfo.hideCompositeShoulders = true; // Makes the shoulders disappear if the gauntlet has its own shoulder texture
				}
			}

			if (item.ModItem is OrchidModGuardianItem && Player.compositeFrontArm.enabled)
			{
				drawInfo.compShoulderOverFrontArm = true; // Why is this not on by default
				drawInfo.hideCompositeShoulders = false;
			}
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
		}

		public override void OnRespawn()
		{
			GuardianGuard = GuardianGuardMax;
			GuardianSlam = 1;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			GuardianHammerCharge = 0;
			GuardianGauntletCharge = 0;
			GuardianStandardCharge = 0;
			GuardianRuneCharge = 0;
		}

		public override void PostUpdateMiscEffects()
		{
			if (GuardianSpikeTemple && Player.HasBuff(ModContent.BuffType<GuardianSpikeBuff>()))
			{
				Player.GetCritChance<GuardianDamageClass>() += 15;
			}

			GuardianStandardStats.ApplyStats(Player); // Standards apply their stats here
		}

		public override void UpdateLifeRegen()
		{
			GuardianStandardStats.ApplyLifeRegen(Player); // Standards apply their stats here
		}

		public override void ResetEffects()
		{
			if (GuardianRecharge <= 0.005f) GuardianRecharge = 0.005f; // Failsafe in case of excessive recharge

			if (GuardianGuard == GuardianGuardMax)
			{
				GuardianGuardRecharge = (int)(GuardianRechargeTime * GuardianRecharge);
			}

			if (GuardianGuardRecharge == 0)
			{
				GuardianGuard++;
				GuardianGuardRecharge = (int)(GuardianRechargeTime * GuardianRecharge);
			}

			if (GuardianSlam > 0)
			{
				if (GuardianDisplayUI < -300 && GuardianSlam > 1)
				{
					GuardianSlam = 1;
				}
				GuardianSlamRecharge = (int)(GuardianRechargeTime * GuardianRecharge);
			}

			if (GuardianSlamRecharge == 0)
			{
				GuardianSlam++;
				GuardianSlamRecharge = (int)(GuardianRechargeTime * GuardianRecharge);
			}

			GuardianGuardRecharge--;
			GuardianSlamRecharge--;
			GuardianDisplayUI--;

			for (int i = GuardianBlockedEnemies.Count - 1; i >= 0; i--)
			{
				BlockedEnemy blockedEnemy = GuardianBlockedEnemies[i];
				blockedEnemy.time--;
				if (blockedEnemy.time < 0)
				{
					GuardianBlockedEnemies.Remove(blockedEnemy);
				}
			}

			if (Player.HeldItem.ModItem is not OrchidModGuardianGauntlet) GuardianGauntletCharge = 0f;
			if (Player.HeldItem.ModItem is not OrchidModGuardianHammer) GuardianHammerCharge = 0f;
			if (Player.HeldItem.ModItem is not OrchidModGuardianStandard && Player.HeldItem.ModItem is not HorizonLance) GuardianStandardCharge = 0f;
			if (Player.HeldItem.ModItem is not OrchidModGuardianRune) GuardianRuneCharge = 0f;

			if (GuardianGauntletParry2) GuardianGauntletParry2 = false;
			else GuardianGauntletParry = false;

			SlamCostUI = 0;

			if (GuardianGuard > GuardianGuardMax) GuardianGuard = GuardianGuardMax;
			if (GuardianSlam > GuardianSlamMax) GuardianSlam = GuardianSlamMax;

			GuardianCurrentStandardAnchor = null;
			RuneProjectiles.Clear();

			// Resetting standards effects

			if (GuardianStandardBuffer)
			{
				GuardianStandardBuffer = false;
			}
			else
			{ // Reset standards here
				GuardianStandardDesert = false;
			}

			// Resetting equipment variables

			GuardianRecharge = 1f;
			GuardianGuardMax = 3;
			GuardianSlamMax = 3;
			GuardianBonusRune = 0;
			GuardianRuneTimer = 1f;
			GuardianSlamDistance = 1f;
			GuardianBlockDuration = 1f;

			GuardianMeteorite = false;
			GuardianSpikeGoblin = false;
			GuardianSpikeDungeon = false;
			GuardianSpikeMechanical = false;
			GuardianSpikeTemple = false;
			GuardianBamboo = false;
			GuardianGit = false;
			GuardianHorizon = false;
			GuardianHoneyPotion = false;
			GuardianWormTooth = false;
			GuardianMonsterFang = false;
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (!Player.HasBuff<BambooCooldown>() && GuardianBamboo && GuardianGuard < GuardianGuardMax)
			{
				Player.AddBuff(ModContent.BuffType<BambooCooldown>(), 600);
				SoundEngine.PlaySound(SoundID.Item37, Player.Center);
				AddGuard(1);
				if (GuardianDisplayUI < 0) GuardianDisplayUI = 0;
			}
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			foreach (BlockedEnemy blockedEnemy in GuardianBlockedEnemies)
			{
				if (blockedEnemy.npc.whoAmI == npc.whoAmI)
				{
					return false;
				}
			}

			return true;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (proj.ModProjectile is OrchidModGuardianProjectile)
			{
				if (GuardianCurrentStandardAnchor != null)
				{
					if (GuardianStandardDesert && GuardianCurrentStandardAnchor.ModProjectile is GuardianStandardAnchor standardAnchor)
					{
						int type = ModContent.ProjectileType<DesertStandardProj>();
						float range = (standardAnchor.BuffItem.ModItem as OrchidModGuardianStandard).AuraRange + target.width * 0.5f;
						if (proj.type != type && target.Center.Distance(Player.Center) < range)
						{
							SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Player.Center);
							float damage = (damageDone * 0.5f);
							if (damage > 20) damage = 20;
							Projectile projectile = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), target.Center, Vector2.UnitY * -10f, type, (int)damage, 1f, Player.whoAmI, target.whoAmI);
							projectile.CritChance = (int)(Player.GetCritChance<GuardianDamageClass>() + Player.GetCritChance<GenericDamageClass>());
						}
					}
				}

				//..
			}
		}

		public override bool FreeDodge(Player.HurtInfo info)
		{
			if (GuardianGauntletParry)
			{
				SoundEngine.PlaySound(SoundID.Item37, Player.Center);
				GuardianGauntletParry = false;
				GuardianGauntletParry2 = false;
				modPlayer.PlayerImmunity = 40;
				Player.immuneTime = 40;
				Player.immune = true;

				if (Player.HeldItem.ModItem is OrchidModGuardianParryItem parryItem)
				{
					parryItem.OnParry(Player, this, info);
					info.DamageSource.TryGetCausingEntity(out Entity entity);

					Projectile proj = null;
					int projectileType = ModContent.ProjectileType<GuardianGauntletAnchor>();
					if (Player.ownedProjectileCounts[projectileType] > 0)
					{
						proj = Main.projectile.First(i => i.active && i.owner == Player.whoAmI && (i.type == projectileType));
					}
					else
					{
						projectileType = ModContent.ProjectileType<GuardianHorizonLanceAnchor>();
						if (Player.ownedProjectileCounts[projectileType] > 0)
						{
							proj = Main.projectile.First(i => i.active && i.owner == Player.whoAmI && i.type == projectileType);
						}
					}

					if (entity != null && proj != null)
					{
						if (entity is NPC npc)
						{
							OnBlockNPC(proj, npc);
							OnBlockNPCFirst(proj, npc);
						}

						if (entity is Projectile projectile)
						{
							OnBlockProjectile(proj, projectile);
							OnBlockProjectileFirst(proj, projectile);
						}
					}
				}
				return true;
			}

			return false;
		}

		// Below are custom Guardian Methods

		public void AddSlam(int nb = 1)
		{
			if (GuardianSlam + nb > GuardianSlamMax) nb = GuardianSlamMax - GuardianSlam;
			if (nb > 0)
			{
				Rectangle rect = Player.Hitbox;
				rect.Y -= 64;
				CombatText.NewText(rect, Color.LightCyan, "+" + nb + " slam" + (nb > 1 ? "s" : ""), false, true);
				GuardianSlam += nb;
			}
		}

		public void AddGuard(int nb = 1)
		{
			if (GuardianGuard + nb > GuardianGuardMax) nb = GuardianGuardMax - GuardianGuard;
			if (nb > 0)
			{
				Rectangle rect = Player.Hitbox;
				rect.Y -= 64;
				CombatText.NewText(rect, Color.LightSkyBlue, "+" + nb + " guard" + (nb > 1 ? "s" : ""), false, true);
				GuardianGuard += nb;
			}
		}

		public bool UseSlam(int nb = 1, bool checkOnly = false)
		{
			if (GuardianHorizon && Player.statLife > Player.statLifeMax2 * 0.5f && Player.statLife > 20)
			{ // Horizon armor set consumes health instead of guardian charges
				if (!checkOnly)
				{
					Player.statLife -= 20;
					CombatText.NewText(Player.Hitbox, CombatText.DamagedFriendly, 20, false, true);
					SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, Player.Center);
				}
				return true;
			}

			if (GuardianSlam >= nb)
			{
				if (!checkOnly)
				{
					GuardianSlam -= nb;
				}
				return true;
			}
			return false;
		}

		public bool UseGuard(int nb = 1, bool checkOnly = false)
		{
			if (GuardianHorizon && Player.statLife > Player.statLifeMax2 * 0.5f && Player.statLife > 20)
			{ // Horizon armor set consumes health instead of guardian charges
				if (!checkOnly)
				{
					Player.statLife -= 20;
					CombatText.NewText(Player.Hitbox, CombatText.DamagedFriendly, 20, false, true);
					SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, Player.Center);
				}
				return true;
			}

			if (GuardianGuard >= nb)
			{
				if (!checkOnly)
				{
					GuardianGuard -= nb;
				}
				return true;
			}
			return false;
		}

		public void StandardNearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{ // Called after affectedPlayer has been affected by a standard held by guardian. isLocalPlayer is true when this is ran by the client holding the standard. Do not change stats on the affectedPlayer it won't work, use standardStats
			if (GuardianWormTooth)
			{
				standardStats.allDamage += 0.05f;
			}

			if (GuardianMonsterFang)
			{
				standardStats.lifeRegen += 2;
			}
		}

		public void StandardNearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
		{ // isLocalPlayer is true when this is ran by the client holding the standard - Should return true if the npc was affected
			// ..
		}

		public void OnBlockAnyFirst(Projectile anchor, ref int toAdd)
		{ // Called by both FirstBlockEffect methods to do universal on-first-block effect
			if (GuardianMeteorite && Main.rand.NextBool(2))
			{
				toAdd++;
			}

			if (GuardianHoneyPotion)
			{ // Heal the player if they have the honey potion effect
				modPlayer.TryHeal((int)(Player.statLifeMax2 * 0.01f));
			}
		}

		public void OnBlockNPCFirst(Projectile anchor, NPC target)
		{ // Called anytime the player blocks/parries their first NPC
			int toAdd = 1;
			OnBlockAnyFirst(anchor, ref toAdd);

			if (anchor.ModProjectile is GuardianShieldAnchor shieldAnchor)
			{
				if (GuardianSpikeGoblin)
				{
					float damage = Player.statDefense;
					if (GuardianSpikeTemple) damage *= 3f;
					else if (GuardianSpikeMechanical) damage *= 2.5f;
					else if (GuardianSpikeDungeon) damage *= 1.5f;

					damage = GetGuardianDamage(damage);
					bool crit = Main.rand.NextFloat(100) < anchor.CritChance;
					Player.ApplyDamageToNPC(target, (int)damage, 0f, Player.direction, crit, ModContent.GetInstance<GuardianDamageClass>());
				}
			}

			AddSlam(toAdd);
		}

		public void OnBlockProjectileFirst(Projectile anchor, Projectile blockedProjectile)
		{ // Called anytime the player blocks/parries their first projectile
			int toAdd = 1;
			OnBlockAnyFirst(anchor, ref toAdd);
			AddSlam(toAdd);
		}

		public void OnBlockAny(Projectile anchor)
		{ // Called by both BlockEffect methods to do universal block effects
			if (GuardianGit)
			{
				Player.AddBuff(ModContent.BuffType<GuardianGitBuff>(), 600);
			}
		}

		public void OnBlockNPC(Projectile anchor, NPC target)
		{ // Called anytime the player blocks/parries a NPC
			OnBlockAny(anchor);
		}

		public void OnBlockProjectile(Projectile anchor, Projectile blockedProjectile)
		{ // Called anytime the player blocks/parries a projectile
			OnBlockAny(anchor);

			if (anchor.ModProjectile is GuardianShieldAnchor shieldAnchor)
			{
				if (GuardianSpikeDungeon)
				{ // For some god forsaken reason, any projectile spawned here is destroyed on frame 1
					int type = ModContent.ProjectileType<WaterSpikeProjAlt>();
					Vector2 dir = Vector2.Normalize(anchor.Center - Player.Center) * 10f;
					int damage = GetGuardianDamage(30); // Duplicate changes in the Dungeon Spike item
					Projectile projectile = Projectile.NewProjectileDirect(anchor.GetSource_FromAI(), anchor.Center, dir, type, damage, 1f, Player.whoAmI);
					projectile.CritChance = (int)(Player.GetCritChance<GuardianDamageClass>() + Player.GetCritChance<GenericDamageClass>());
				}

				if (GuardianSpikeTemple || GuardianSpikeMechanical)
				{
					Player.AddBuff(ModContent.BuffType<GuardianSpikeBuff>(), 600);
				}
			}
		}

		public int ThrowLevel()
		{
			if (GuardianHammerCharge < 45f) return 0;
			if (GuardianHammerCharge < 90f) return 1;
			if (GuardianHammerCharge < 135f) return 2;
			if (GuardianHammerCharge < 180f) return 3;
			return 4;
		}
	}
}
