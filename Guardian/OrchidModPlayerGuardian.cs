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
	public class OrchidModPlayerGuardian : ModPlayer
	{
		public OrchidModPlayer modPlayer;

		public float guardianDamage = 1.0f;
		public int guardianCrit = 0;
		public float guardianRecharge = 1f;
		public int guardianBlock = 0;
		public int guardianSlam = 0;
		public int guardianBlockMax = 3;
		public int guardianSlamMax = 3;
		public int guardianBlockRecharge = 0;
		public int guardianSlamRecharge = 0;
		public int guardianDisplayUI = 0;
		public List<BlockedEnemy> guardianBlockedEnemies = new List<BlockedEnemy>();

		public static int guardianRechargeTime = 600;

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidModPlayer>();
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

			guardianDamage = 1.0f;
			guardianCrit = 0;
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
	}
}
