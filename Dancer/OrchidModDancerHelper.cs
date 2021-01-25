using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Dancer;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Dancer
{
	public class OrchidModDancerHelper
	{
		public static void dancerPostUpdateEquips(Player player, OrchidModPlayer modPlayer, Mod mod) {	
			if (modPlayer.dancerVelocity.X != 0f || modPlayer.dancerVelocity.Y != 0f) {
				player.velocity *= 0f;
				
				int Height = !player.onTrack ? player.height : player.height - 20;
				modPlayer.dancerVelocity = Collision.TileCollision(player.position, modPlayer.dancerVelocity, player.width, Height, true, false, (int) player.gravDir);
				
				player.position.X += modPlayer.dancerVelocity.X;
				player.position.Y += modPlayer.dancerVelocity.Y;
				// player.position = Vector2.op_Addition(player.position, currentVelocity);
			}
		}
		
		public static bool PreHurtDancer(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
		ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, OrchidModPlayer modPlayer, Mod mod) {
			if (modPlayer.dancerInvincibility > 0) {
				return false;
			}
			return true;
		}
		
		public static void ModifyHitByNPCDancer(NPC npc, ref int damage, ref bool crit, Player player, OrchidModPlayer modPlayer, Mod mod) {
			if (modPlayer.dancerWeaponType != OrchidModDancerItemType.NULL) {
				if (!(npc.boss || npc.type == NPCID.TargetDummy) && npc.knockBackResist > 0f) {
					npc.velocity.X = player.velocity.X * modPlayer.dancerWeaponKnockback * npc.knockBackResist / 5;
					npc.velocity.Y = player.velocity.Y * modPlayer.dancerWeaponKnockback * npc.knockBackResist / 5;
				}
				npc.StrikeNPCNoInteraction(modPlayer.dancerWeaponDamage, 0f, 0);
				
				switch (modPlayer.dancerWeaponType) {
					case OrchidModDancerItemType.IMPACT:
						player.velocity = modPlayer.dancerVelocity * -0.5f;
						OrchidModDancerHelper.clearDancerEffects(player, modPlayer, mod);
						break;
					case OrchidModDancerItemType.PHASE:
						break;
					case OrchidModDancerItemType.MOMENTUM:
						player.velocity = modPlayer.dancerVelocity * -1f;
						break;
					default:
						break;
				}
				
				damage = 0;
				modPlayer.dancerInvincibility = 10;
			}
		}
			
		public static void ResetEffectsDancer(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.dancerDamage = 1.0f;
			modPlayer.dancerCrit = 0;
			modPlayer.dancerPoiseMax = 20;
			modPlayer.dancerPoiseConsume = 0;
			modPlayer.dancerInvincibility -= modPlayer.dancerInvincibility > 0 ? 1 : 0;
				
			if (modPlayer.dancerDashTimer > 0) {
				modPlayer.dancerDashTimer --;
				if (modPlayer.dancerDashTimer == 0) {
					player.velocity *= 0f;
					OrchidModDancerHelper.clearDancerEffects(player, modPlayer, mod);
				}
			}
		}
		
		public static void clearDancerEffects(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.dancerDashTimer = 0;
			modPlayer.dancerWeaponDamage = 0;
			modPlayer.dancerWeaponKnockback = 0f;
			modPlayer.dancerWeaponType = OrchidModDancerItemType.NULL;
			modPlayer.dancerVelocity = new Vector2(0f, 0f);
		}
		
		public static void removeDancerPoise(int chance, int number, Player player, OrchidModPlayer modPlayer, Mod mod) {
			for (int i = 0 ; i < number ; i ++) {
				if (Main.rand.Next(100) < (chance - modPlayer.dancerPoiseConsume)) {
					modPlayer.dancerPoise --;
				}
			}
		}
	}
}