using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Alchemist;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public class OrchidModAlchemistHelper
	{
		public static void alchemistPostUpdateEquips(Player player, OrchidModPlayer modPlayer, Mod mod) {
			int regenComparator = modPlayer.alchemistPotencyWait <= 120 ? modPlayer.alchemistPotencyWait <= 0 ? (int)(modPlayer.alchemistRegenPotency / 3) : (int)(modPlayer.alchemistRegenPotency / 2) : modPlayer.alchemistRegenPotency;
			if (modPlayer.alchemistPotency < modPlayer.alchemistPotencyMax && ((modPlayer.alchemistRegenPotency > 0) ? modPlayer.timer120 % regenComparator == 0 : true)) {
				modPlayer.alchemistPotencyDisplayTimer = 180;
				modPlayer.alchemistPotency ++;
				modPlayer.alchemistFlowerTimer = modPlayer.alchemistFlowerSet ? 600 : 0;
			} else {
				if (modPlayer.alchemistPotency > modPlayer.alchemistPotencyMax) {
					modPlayer.alchemistPotency = modPlayer.alchemistPotencyMax;
				}
				modPlayer.alchemistPotencyDisplayTimer --;
			}
			
			modPlayer.alchemistResetTimer = (modPlayer.alchemistPotencyDisplayTimer > 0) ? 300 : modPlayer.alchemistResetTimer - 1;
			modPlayer.alchemistResetTimer = (modPlayer.alchemistResetTimer == -1) ? 0 : modPlayer.alchemistResetTimer;
			modPlayer.alchemistPotencyWait -= modPlayer.alchemistPotencyWait > 0 ? 1 : 0;
			
			if (modPlayer.alchemistResetTimer == 1) {
				modPlayer.alchemistFlaskDamage = 0;
				modPlayer.alchemistNbElements = 0;
				OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
				OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
				OrchidModAlchemistHelper.clearAlchemistDusts(player, modPlayer, mod);
				OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
			}
			
			if (modPlayer.alchemistColorRDisplay != modPlayer.alchemistColorR) {
				bool superior = modPlayer.alchemistColorRDisplay > modPlayer.alchemistColorR;
				int abs = Math.Abs(modPlayer.alchemistColorRDisplay - modPlayer.alchemistColorR);
				int val = (int)(abs / 10);
				if (abs > val) {
					modPlayer.alchemistColorRDisplay += superior ? -val : val;
				}
				modPlayer.alchemistColorRDisplay += superior ? -1 : 1;
			}
			
			if (modPlayer.alchemistColorGDisplay != modPlayer.alchemistColorG) {
				bool superior = modPlayer.alchemistColorGDisplay > modPlayer.alchemistColorG;
				int abs = Math.Abs(modPlayer.alchemistColorGDisplay - modPlayer.alchemistColorG);
				int val = (int)(abs / 10);
				if (abs > val) {
					modPlayer.alchemistColorGDisplay += superior ? -val : val;
				}
				modPlayer.alchemistColorGDisplay += superior ? -1 : 1;
			}
			
			if (modPlayer.alchemistColorBDisplay != modPlayer.alchemistColorB) {
				bool superior = modPlayer.alchemistColorBDisplay > modPlayer.alchemistColorB;
				int abs = Math.Abs(modPlayer.alchemistColorBDisplay - modPlayer.alchemistColorB);
				int val = (int)(abs / 10);
				if (abs > val) {
					modPlayer.alchemistColorBDisplay += superior ? -val : val;
				}
				modPlayer.alchemistColorBDisplay += superior ? -1 : 1;
			}
		}

		public static void postUpdateAlchemist(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.alchemistFlowerTimer -= modPlayer.alchemistFlowerTimer > 0 ? 1 : 0;
			modPlayer.alchemistFlower = modPlayer.alchemistFlowerTimer == 0 ? 0 : modPlayer.alchemistFlower;
			
			if (modPlayer.alchemistShootProjectile) {	
				float shootSpeed = 10f * modPlayer.alchemistVelocity;
				int projType = ProjectileType<Alchemist.Projectiles.AlchemistProj>();				
				Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
				Vector2 heading = target - player.Center;
				heading.Normalize();
				heading *= shootSpeed;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, heading.X, heading.Y, projType, 1, 1f, player.whoAmI);
				modPlayer.alchemistShootProjectile = false;
			}
		}
		
		public static void ResetEffectsAlchemist(Player player, OrchidModPlayer modPlayer, Mod mod) {
			if (OrchidModAlchemistHelper.getNbAlchemistFlasks(player, modPlayer, mod) == 0) {
				OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
				OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
				OrchidModAlchemistHelper.clearAlchemistDusts(player, modPlayer, mod);
				OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
			}
			
			modPlayer.alchemistPotencyMax = 8;
			modPlayer.alchemistRegenPotency = 60;
			modPlayer.alchemistNbElementsMax = 2;
			modPlayer.alchemistCrit = 0;
			modPlayer.alchemistDamage = 1.0f;
			modPlayer.alchemistVelocity = 1.0f;
			modPlayer.alchemistSelectUIDisplay = modPlayer.alchemistSelectUIItem ? modPlayer.alchemistSelectUIDisplay : false;
			modPlayer.alchemistSelectUIItem = false;
			
			modPlayer.alchemistMeteor = false;
			modPlayer.alchemistFlowerSet = false;
			modPlayer.alchemistMushroomSpores = false;
			modPlayer.alchemistReactiveVials = false;
		}
		
		public static void onRespawnAlchemist(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.alchemistPotency = modPlayer.alchemistPotencyMax;
			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			modPlayer.alchemistPotencyDisplayTimer = 0;
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistDusts(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);
		}
		
		public static void clearAlchemistColors(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.alchemistColorR = 50;
			modPlayer.alchemistColorG = 100;
			modPlayer.alchemistColorB = 255;
		}
		
		public static void clearAlchemistFlasks(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.alchemistFlasks = new int[6];
			
			for (int i = 0 ; i < 6 ; i ++) {
				modPlayer.alchemistFlasks[i] = 0;
			}
		}
		
		public static void clearAlchemistElements(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.alchemistElements = new bool[6];
			
			for (int i = 0 ; i < 6 ; i ++) {
				modPlayer.alchemistElements[i] = false;
			}
		}
		
		public static void clearAlchemistDusts(Player player, OrchidModPlayer modPlayer, Mod mod) {
			modPlayer.alchemistDusts = new int[6];
			
			for (int i = 0 ; i < 6 ; i ++) {
				modPlayer.alchemistDusts[i] = -1;
			}
		}
		
		public static int getNbAlchemistFlasks(Player player, OrchidModPlayer modPlayer, Mod mod) {
			int val = 0;
			for (int i = 0; i < 6; i ++) {
				val += modPlayer.alchemistFlasks[i] != 0 ? 1 : 0;
			}
			return val;
		}
		
		public static bool containsAlchemistFlask(int flaskType, Player player, OrchidModPlayer modPlayer, Mod mod) {
			for (int i = 0; i < 6; i ++) {
				if (modPlayer.alchemistFlasks[i] == flaskType) {
					return true;
				}
			}
			return false;
		}
	}
}