using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Guardian
{
	public class OrchidModGuardianHelper
	{	
		public static int guardianRechargeTime = 600;
	
		public static bool CanBeHitByNPCGuardian(NPC npc, ref int cooldownSlot, Player player, OrchidModPlayer modPlayer, Mod mod) {
			foreach (BlockedEnemy blockedEnemy in modPlayer.guardianBlockedEnemies)
			{
				if (blockedEnemy.npc.whoAmI == npc.whoAmI)
				{
					return false;
				}
			}
			return true;
		}


		public static void ResetEffectsGuardian(Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			if (modPlayer.guardianBlock == modPlayer.guardianBlockMax) {
				modPlayer.guardianBlockRecharge = (int)(guardianRechargeTime * modPlayer.guardianRecharge);
			}
			
			if (modPlayer.guardianBlockRecharge == 0) {
				modPlayer.guardianBlock ++;
				modPlayer.guardianBlockRecharge = (int)(guardianRechargeTime * modPlayer.guardianRecharge);
			}
			
			if (modPlayer.guardianSlam > 0) {
				if (modPlayer.guardianDisplayUI < -300 && modPlayer.guardianSlam > 1)
				{
					modPlayer.guardianSlam = 1;
				}
				modPlayer.guardianSlamRecharge = (int)(guardianRechargeTime * modPlayer.guardianRecharge);
			}
			
			if (modPlayer.guardianSlamRecharge == 0) {
				modPlayer.guardianSlam ++;
				modPlayer.guardianSlamRecharge = (int)(guardianRechargeTime * modPlayer.guardianRecharge);
			}
			
			modPlayer.guardianDamage = 1.0f;
			modPlayer.guardianCrit = 0;
			modPlayer.guardianRecharge = 1f;
			
			modPlayer.guardianBlockRecharge --;
			modPlayer.guardianSlamRecharge --;
			modPlayer.guardianDisplayUI --;
			
			for (int i = modPlayer.guardianBlockedEnemies.Count - 1; i >= 0; i--)
			{
				BlockedEnemy blockedEnemy = modPlayer.guardianBlockedEnemies[i];
				blockedEnemy.time --;
				if (blockedEnemy.time < 0)
				{
					modPlayer.guardianBlockedEnemies.Remove(blockedEnemy);
				}
			}
		}
	}
}