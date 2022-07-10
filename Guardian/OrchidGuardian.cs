using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist;
using OrchidMod.Dancer;
using OrchidMod.Gambler;
using OrchidMod.Shaman;
using OrchidMod.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Common;

namespace OrchidMod
{
	public class OrchidGuardian : ModPlayer
	{
		public OrchidPlayer modPlayer;

		public float guardianRecharge = 1f;
		public int guardianBlock = 0;
		public int guardianSlam = 0;
		public int guardianBlockMax = 3;
		public int guardianSlamMax = 3;
		public int guardianBlockRecharge = 0;
		public int guardianSlamRecharge = 0;
		public int guardianDisplayUI = 0;
		public int guardianThrowCharge = 0;
		public bool guardianThrowDecreasing;
		public bool holdingHammer;
		public List<BlockedEnemy> guardianBlockedEnemies = new List<BlockedEnemy>();

		public static int guardianRechargeTime = 600;

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
		}


		public override void ResetEffects()
		{
			if (guardianBlock == guardianBlockMax)
			{
				guardianBlockRecharge = (int)(guardianRechargeTime * guardianRecharge);
			}

			if (guardianBlockRecharge == 0)
			{
				guardianBlock++;
				guardianBlockRecharge = (int)(guardianRechargeTime * guardianRecharge);
			}

			if (guardianSlam > 0)
			{
				if (guardianDisplayUI < -300 && guardianSlam > 1)
				{
					guardianSlam = 1;
				}
				guardianSlamRecharge = (int)(guardianRechargeTime * guardianRecharge);
			}

			if (guardianSlamRecharge == 0)
			{
				guardianSlam++;
				guardianSlamRecharge = (int)(guardianRechargeTime * guardianRecharge);
			}

			if (holdingHammer)
			{
				guardianThrowCharge += guardianThrowDecreasing ? -4 : 1;

				if (guardianThrowCharge > 210)
					guardianThrowDecreasing = true;

				if (guardianThrowCharge <= 0)
				{
					guardianThrowDecreasing = false;
					guardianThrowCharge = 0;
				}
			} else guardianThrowCharge = 0;

			guardianRecharge = 1f;

			guardianBlockRecharge--;
			guardianSlamRecharge--;
			guardianDisplayUI--;

			for (int i = guardianBlockedEnemies.Count - 1; i >= 0; i--)
			{
				BlockedEnemy blockedEnemy = guardianBlockedEnemies[i];
				blockedEnemy.time--;
				if (blockedEnemy.time < 0)
				{
					guardianBlockedEnemies.Remove(blockedEnemy);
				}
			}

			holdingHammer = false;
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			foreach (BlockedEnemy blockedEnemy in guardianBlockedEnemies)
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
			this.guardianSlam += nb;
			if (guardianSlam > guardianSlamMax) guardianSlam = guardianSlamMax;
		}

		public void AddBlock(int nb)
		{
			this.guardianBlock += nb;
			if (guardianBlock > guardianBlockMax) guardianBlock = guardianBlockMax;
		}

		public int ThrowLevel()
		{
			if (guardianThrowCharge < 45) return 0;
			if (guardianThrowCharge < 90) return 1;
			if (guardianThrowCharge < 135) return 2;
			if (guardianThrowCharge < 180) return 3;
			return 4;
		}
	}
}
