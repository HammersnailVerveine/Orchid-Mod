using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Buffs;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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

		// Dynamic gameplay and UI fields

		public int GuardianBlock = 0;
		public int GuardianSlam = 0;
		public int GuardianBlockRecharge = 0;
		public int GuardianSlamRecharge = 0;
		public int GuardianDisplayUI = 0;
		public int GuardianThrowCharge = 0;
		public bool GuardianThrowDecreasing;
		public bool HoldingHammer;
		public bool GuardianHammerDing;
		public int SlamCostUI = 0;
		public List<BlockedEnemy> GuardianBlockedEnemies = new List<BlockedEnemy>();
		public List<Projectile> RuneProjectiles = new List<Projectile>();

		public static int GuardianRechargeTime = 600;

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
		}

		public override void PostUpdate()
		{
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

			if (HoldingHammer)
			{
				GuardianThrowCharge += GuardianThrowDecreasing ? -4 : 1;

				if (GuardianThrowCharge >= 180 && !GuardianHammerDing) {
					GuardianHammerDing = true;
					SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
				}

				if (GuardianThrowCharge > 210)
					GuardianThrowDecreasing = true;

				if (GuardianThrowCharge <= 0)
				{
					GuardianThrowDecreasing = false;
					GuardianThrowCharge = 0;
				}
			} else GuardianThrowCharge = 0;

			if (GuardianThrowCharge < 180) GuardianHammerDing = false;

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

			HoldingHammer = false;
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
			if (GuardianThrowCharge < 45) return 0;
			if (GuardianThrowCharge < 90) return 1;
			if (GuardianThrowCharge < 135) return 2;
			if (GuardianThrowCharge < 180) return 3;
			return 4;
		}
	}
}
