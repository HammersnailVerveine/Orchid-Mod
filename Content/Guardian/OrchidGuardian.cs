using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Buffs;
using OrchidMod.Content.Guardian.Buffs.Debuffs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace OrchidMod
{
	public class OrchidGuardian : ModPlayer
	{
		public OrchidPlayer modPlayer;

		// Can be edited by gear

		public float GuardianRecharge = 1f; // Natural guardian slam/block regeneration multiplier
		public int GuardianBlockMax = 3; // Max block charges
		public int GuardianSlamMax = 3; // Max slam charges
		public int GuardianBonusRune = 0; // Bonus projectiles spawned by runes
		public float GuardianRuneTimer = 1f; // Rune duration multiplier
		public float GuardianSlamDistance = 1f; // Slam Distance multiplier
		public float GuardianBlockDuration = 1f; // Block Duration multiplier

		// Set effects, accessories, misc

		public bool GuardianMeteorite = false;
		public bool GuardianSpikeGoblin = false;
		public bool GuardianSpikeDungeon = false;
		public bool GuardianSpikeMechanical = false;
		public bool GuardianSpikeTemple = false;
		public bool GuardianBamboo = false;
		public bool GuardianGit = false;

		// Dynamic gameplay and UI fields

		public int GuardianBlock = 0;
		public int GuardianSlam = 0;
		public int GuardianBlockRecharge = 0;
		public int GuardianSlamRecharge = 0;
		public int GuardianDisplayUI = 0; // Guardian UI is displayed if > 0
		public float GuardianThrowCharge = 0f; // Player Warhammer Throw Charge, max is 180f
		public float GuardianGauntletCharge = 0f; // Player Gauntlet Punch Charge, max is 180f
		public bool GuardianGauntletParry = false; // Player is currently parrying with a gauntlet
		public bool GuardianGauntletParry2 = false; // Player is currently parrying with a gauntlet (1 frame buffer)
		public int SlamCostUI = 0; // Displays an outline around slams in the UI if > 0
		public List<BlockedEnemy> GuardianBlockedEnemies = new List<BlockedEnemy>();
		public List<Projectile> RuneProjectiles = new List<Projectile>();

		public static int GuardianRechargeTime = 600;

		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			drawInfo.cHandOff = -1;
			drawInfo.cHandOn = -1;
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
		}

		public override void PostUpdate()
		{
		}

		public override void OnRespawn()
		{
			GuardianBlock = GuardianBlockMax;
			GuardianSlam = 1;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			GuardianThrowCharge = 0;
		}

		public override void PostUpdateEquips()
		{
			if (GuardianSpikeTemple && Player.HasBuff(ModContent.BuffType<GuardianSpikeBuff>())) Player.GetCritChance<GuardianDamageClass>() += 15;
		}

		public override void ResetEffects()
		{
			if (GuardianRecharge <= 0f) GuardianRecharge = 0.005f; // Failsafe in case of excessive recharge

			if (GuardianBlock == GuardianBlockMax)
			{
				GuardianBlockRecharge = (int)(GuardianRechargeTime * GuardianRecharge);
			}

			if (GuardianBlockRecharge == 0)
			{
				GuardianBlock++;
				GuardianBlockRecharge = (int)(GuardianRechargeTime * GuardianRecharge);
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

			GuardianBlockRecharge--;
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

			if (GuardianGauntletParry2) GuardianGauntletParry2 = false;
			else GuardianGauntletParry = false;

			SlamCostUI = 0;

			if (GuardianBlock > GuardianBlockMax) GuardianBlock = GuardianBlockMax;
			if (GuardianSlam > GuardianSlamMax) GuardianSlam = GuardianSlamMax;

			// Resetting equipment variables

			GuardianRecharge = 1f;
			GuardianBlockMax = 3;
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
		}

		public void OnBlock(NPC npc, Projectile projectile, Projectile shieldAnchor, bool firstBlock)
		{
			if (npc != null) // a npc has been blocked
			{
				// ...
			}

			if (projectile != null) // a projectile has been blocked
			{
				// ...
			}

			if (GuardianGit) Player.AddBuff(ModContent.BuffType<GuardianGitBuff>(), 600);
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (!Player.HasBuff<BambooCooldown>() && GuardianBamboo && GuardianBlock < GuardianBlockMax)
			{
				Player.AddBuff(ModContent.BuffType<BambooCooldown>(), 600);
				SoundEngine.PlaySound(SoundID.Item37, Player.Center);
				AddBlock(1);
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

		public override bool FreeDodge(Player.HurtInfo info)
		{
			if (GuardianGauntletParry)
			{
				SoundEngine.PlaySound(SoundID.Item37, Player.Center);
				GuardianGauntletParry = false;
				GuardianGauntletParry2 = false;
				Player.immuneTime = 40;
				Player.immune = true;
				AddSlam(1);

				int projectileType = ModContent.ProjectileType<GuardianGauntletAnchor>();
				if (Player.ownedProjectileCounts[projectileType] > 0)
				{
					var proj = Main.projectile.First(i => i.active && i.owner == Player.whoAmI && i.type == projectileType);
					if (proj != null && proj.ModProjectile is GuardianGauntletAnchor anchor)
					{
						if (anchor.GauntletItem.ModItem is OrchidModGuardianGauntlet gauntlet) {
							gauntlet.OnParry(Player, this, info);
						}
					}
				}
				return true;
			}

			return false;
		}

		public void AddSlam(int nb)
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

		public void AddBlock(int nb)
		{
			if (GuardianBlock + nb > GuardianBlockMax) nb = GuardianBlockMax - GuardianBlock;
			if (nb > 0)
			{
				Rectangle rect = Player.Hitbox;
				rect.Y -= 64;
				CombatText.NewText(rect, Color.LightSkyBlue, "+" + nb + " block" + (nb > 1 ? "s" : ""), false, true);
				GuardianBlock += nb;
			}
		}

		public int ThrowLevel()
		{
			if (GuardianThrowCharge < 45f) return 0;
			if (GuardianThrowCharge < 90f) return 1;
			if (GuardianThrowCharge < 135f) return 2;
			if (GuardianThrowCharge < 180f) return 3;
			return 4;
		}
	}
}
