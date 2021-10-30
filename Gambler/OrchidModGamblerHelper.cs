using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler
{
	public class OrchidModGamblerHelper
	{
		public static void gamblerPostUpdateEquips(Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				//Since this is a variable that is synced through SendClientChanges, it has to only be assigned on the client aswell
				modPlayer.gamblerHasCardInDeck = modPlayer.gamblerCardsItem[0].type != 0;
			}

			if (modPlayer.gamblerRedrawsMax > 0)
			{
				modPlayer.gamblerRedrawCooldown = modPlayer.gamblerRedraws >= modPlayer.gamblerRedrawsMax ? modPlayer.gamblerRedrawCooldownMax : modPlayer.gamblerRedrawCooldown;
				modPlayer.gamblerRedrawCooldown = modPlayer.gamblerRedrawCooldown > modPlayer.gamblerRedrawCooldownMax ? modPlayer.gamblerRedrawCooldownMax : modPlayer.gamblerRedrawCooldown;
				modPlayer.gamblerRedrawCooldown = modPlayer.gamblerRedrawCooldown <= 0 ? modPlayer.gamblerRedrawCooldownMax : modPlayer.gamblerRedrawCooldown - 1;
				if (modPlayer.gamblerRedrawCooldown <= 0 && modPlayer.gamblerRedraws < modPlayer.gamblerRedrawsMax)
				{
					modPlayer.gamblerRedraws++;
				}
			}
			else
			{
				modPlayer.gamblerRedrawCooldown = -1;
			}

			modPlayer.gamblerRedrawCooldownUse -= modPlayer.gamblerRedrawCooldownUse > 0 ? 1 : 0;
			modPlayer.gamblerShuffleCooldown -= modPlayer.gamblerShuffleCooldown > 0 ? 1 : 0;
			modPlayer.gamblerUIDisplayTimer = modPlayer.gamblerShuffleCooldown <= 0 && modPlayer.gamblerDiceDuration <= 0 ? modPlayer.gamblerUIDisplayTimer > 0 ? modPlayer.gamblerUIDisplayTimer - 1 : modPlayer.gamblerUIDisplayTimer : 300;
			if (modPlayer.gamblerChips > 0 && modPlayer.gamblerUIDisplayTimer <= 0 && modPlayer.timer120 % 60 == 0)
			{
				modPlayer.gamblerChips--;
				modPlayer.gamblerUIDisplayTimer = modPlayer.gamblerChips == 0 ? 60 : 0;
			}

			if (modPlayer.gamblerUIDisplayTimer == 0 && modPlayer.gamblerChips == 0 && modPlayer.gamblerCardCurrent.type == 0)
			{
				OrchidModGamblerHelper.clearGamblerCardCurrent(player, modPlayer);
				modPlayer.gamblerRedraws = 0;
				OrchidModGamblerHelper.clearGamblerCardsNext(player, modPlayer);
			}

			if (modPlayer.gamblerRedraws > modPlayer.gamblerRedrawsMax) modPlayer.gamblerRedraws = modPlayer.gamblerRedrawsMax;
			if (modPlayer.gamblerChips > modPlayer.gamblerChipsMax) modPlayer.gamblerChips = modPlayer.gamblerChipsMax;

			if (modPlayer.gamblerDiceDuration <= 0)
			{
				modPlayer.gamblerDiceID = -1;
				modPlayer.gamblerDiceValue = 0;
			}
			else
			{
				modPlayer.gamblerDiceDuration--;
				switch (modPlayer.gamblerDiceID)
				{
					case 0:
						modPlayer.gamblerDamage += (0.03f * modPlayer.gamblerDiceValue);
						break;
					case 1:
						modPlayer.gamblerChipsConsume += 4 * modPlayer.gamblerDiceValue;
						break;
					default:
						break;
				}
			}
		}

		public static void postUpdateGambler(Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			switch (modPlayer.gamblerDiceID)
			{
				case 0:
					player.AddBuff(mod.BuffType("GamblerDice"), 1);
					break;
				case 1:
					player.AddBuff(mod.BuffType("GemstoneDice"), 1);
					break;
				case 2:
					player.AddBuff(mod.BuffType("HoneyDice"), 1);
					break;
				default:
					break;
			}

			if (modPlayer.gamblerTimerHoney < 30)
			{
				modPlayer.gamblerTimerHoney++;
			}
		}

		public static void ModifyHitNPCWithProjGambler(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			if (modPlayer.gamblerDiceID == 2 && modPlayer.gamblerTimerHoney == 30)
			{
				player.HealEffect(modPlayer.gamblerDiceValue, true);
				player.statLife += modPlayer.gamblerDiceValue;
				modPlayer.gamblerTimerHoney = 0;
			}
		}

		public static void ResetEffectsGambler(Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			modPlayer.gamblerChipSpin += modPlayer.gamblerPauseChipRotation > 0 ? 0f : 1.5f + (modPlayer.gamblerChipSpinBonus * 1.5f);
			modPlayer.gamblerChipSpin = modPlayer.gamblerChipSpin > 360f ? modPlayer.gamblerChipSpin - 360f : modPlayer.gamblerChipSpin;
			modPlayer.gamblerPauseChipRotation -= modPlayer.gamblerPauseChipRotation > 0 ? 1 : 0;
			modPlayer.gamblerDamage = 1.0f;
			modPlayer.gamblerDamageChip = 0f;
			modPlayer.gamblerChipChance = 1.0f;
			modPlayer.gamblerCrit = 0;
			modPlayer.gamblerChipSpinBonus = 0f;
			modPlayer.gamblerChipsMax = 5;
			modPlayer.gamblerChipsConsume = 0;
			modPlayer.gamblerSeeCards = 0;
			modPlayer.gamblerRedrawsMax = 0;
			modPlayer.gamblerRedrawCooldownMax = 1800;
			modPlayer.gamblerShuffleCooldownMax = 900;
			modPlayer.GamblerDeckInHand = false;
			modPlayer.gamblerUIFightDisplay = false;
			modPlayer.gamblerUIChipSpinDisplay = false;

			modPlayer.gamblerDungeon = false;
			modPlayer.gamblerLuckySprout = false;
			modPlayer.gamblerPennant = false;
			modPlayer.gamblerElementalLens = false;
			modPlayer.gamblerVulture = false;
			modPlayer.gamblerSlimyLollipop = false;
		}

		public static void onRespawnGambler(Player player, OrchidModPlayer modPlayer)
		{
			modPlayer.gamblerShuffleCooldown = 0;
			modPlayer.gamblerChips = 0;
			modPlayer.gamblerRedraws = 0;
			modPlayer.gamblerUIDisplayTimer = 0;
			OrchidModGamblerHelper.clearGamblerCardCurrent(player, modPlayer);
			OrchidModGamblerHelper.clearGamblerCardsNext(player, modPlayer);
		}

		public static void clearGamblerCardsNext(Player player, OrchidModPlayer modPlayer)
		{
			modPlayer.gamblerCardNext = new Item[3];
			for (int i = 0; i < 3; i++)
			{
				modPlayer.gamblerCardNext[i] = new Item();
				modPlayer.gamblerCardNext[i].SetDefaults(0, true);
			}
		}

		public static void clearGamblerCardCurrent(Player player, OrchidModPlayer modPlayer)
		{
			modPlayer.gamblerCardCurrent = new Item();
			modPlayer.gamblerCardCurrent.SetDefaults(0, true);
		}

		public static void clearGamblerCards(Player player, OrchidModPlayer modPlayer)
		{
			modPlayer.gamblerCardsItem = new Item[20];
			for (int i = 0; i < 20; i++)
			{
				modPlayer.gamblerCardsItem[i] = new Item();
				modPlayer.gamblerCardsItem[i].SetDefaults(0, true);
			}

			modPlayer.gamblerCardCurrent = new Item();
			modPlayer.gamblerCardCurrent.SetDefaults(0, true);
		}

		public static void removeGamblerCard(Item card, Player player, OrchidModPlayer modPlayer)
		{
			if (OrchidModGamblerHelper.containsGamblerCard(card, player, modPlayer))
			{
				bool found = false;
				for (int i = 0; i < 20; i++)
				{
					if (modPlayer.gamblerCardsItem[i].type == card.type)
					{
						found = true;
					}
					if (found)
					{
						modPlayer.gamblerCardsItem[i] = new Item();
						modPlayer.gamblerCardsItem[i].SetDefaults(i == 19 ? 0 : modPlayer.gamblerCardsItem[i + 1].type, true);
					}
				}
			}
		}

		public static int getNbGamblerCards(Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerCardsItem.Count() != 20)
			{
				OrchidModGamblerHelper.clearGamblerCards(player, modPlayer);
			}
			int val = 0;
			for (int i = 0; i < 20; i++)
			{
				if (modPlayer.gamblerCardsItem[i].type != 0)
				{
					val++;
				}
				else
				{
					return val;
				}
			}
			return val;
		}

		public static bool containsGamblerCard(Item card, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 20; i++)
			{
				if (modPlayer.gamblerCardsItem[i].type == card.type)
				{
					return true;
				}
			}
			return false;
		}

		public static void rollGamblerDice(int diceID, int diceDuration, Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			modPlayer.gamblerDiceID = diceID;
			modPlayer.gamblerDiceValue = Main.rand.Next(6) + 1;
			modPlayer.gamblerDiceDuration = 60 * diceDuration;
		}

		public static void addGamblerChip(int chance, Player player, OrchidModPlayer modPlayer)
		{
			chance = (int)(chance * modPlayer.gamblerChipChance);
			if (Main.rand.Next(100) < chance)
			{
				modPlayer.gamblerChips += modPlayer.gamblerChips < modPlayer.gamblerChipsMax ? 1 : 0;
			}
			modPlayer.gamblerUIDisplayTimer = 300;
		}

		public static void removeGamblerChip(int chance, int number, Player player, OrchidModPlayer modPlayer, Mod mod)
		{
			for (int i = 0; i < number; i++)
			{
				if (Main.rand.Next(100) < (chance - modPlayer.gamblerChipsConsume))
				{
					modPlayer.gamblerChips--;
				}
			}
			modPlayer.gamblerUIDisplayTimer = 300;
		}

		public static void drawDummyCard(Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerCardDummy.type == 0)
			{
				if (modPlayer.gamblerCardsItem[0].type != 0)
				{
					modPlayer.gamblerCardDummy = new Item();
					modPlayer.gamblerCardDummy.SetDefaults(modPlayer.gamblerCardsItem[0].type, true);
				}
				return;
			}

			for (int i = 0; i < 20; i++)
			{
				if (i == 19)
				{
					modPlayer.gamblerCardDummy = new Item();
					modPlayer.gamblerCardDummy.SetDefaults(modPlayer.gamblerCardsItem[0].type, true);
				}

				if (modPlayer.gamblerCardsItem[i].type == modPlayer.gamblerCardDummy.type)
				{
					if (modPlayer.gamblerCardsItem[i + 1].type != 0)
					{
						modPlayer.gamblerCardDummy = new Item();
						modPlayer.gamblerCardDummy.SetDefaults(modPlayer.gamblerCardsItem[i + 1].type, true);
					}
					else
					{
						modPlayer.gamblerCardDummy = new Item();
						modPlayer.gamblerCardDummy.SetDefaults(modPlayer.gamblerCardsItem[0].type, true);
					}
					break;
				}
			}
		}

		public static void drawGamblerCard(Player player, OrchidModPlayer modPlayer)
		{
			modPlayer.gamblerJustSwitched = true;

			if (modPlayer.gamblerCardNext.Count() != 3)
			{
				OrchidModGamblerHelper.clearGamblerCardsNext(player, modPlayer);
			}

			for (int i = 0; i < 3; i++)
			{
				if (modPlayer.gamblerCardNext[i].type == 0)
				{
					int rand = Main.rand.Next(OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer));
					modPlayer.gamblerCardNext[i] = new Item();
					modPlayer.gamblerCardNext[i].SetDefaults(modPlayer.gamblerCardsItem[rand].type, true);
				}
			}

			modPlayer.gamblerCardCurrent = new Item();
			modPlayer.gamblerCardCurrent.SetDefaults(modPlayer.gamblerCardNext[0].type, true);

			for (int i = 0; i < 2; i++)
			{
				modPlayer.gamblerCardNext[i] = new Item();
				modPlayer.gamblerCardNext[i].SetDefaults(modPlayer.gamblerCardNext[i + 1].type, true);
			}

			modPlayer.gamblerCardNext[2] = new Item();
			modPlayer.gamblerCardNext[2].SetDefaults(0, true);

			for (int i = 0; i < 3; i++)
			{
				if (modPlayer.gamblerCardNext[i].type == 0)
				{
					int rand = Main.rand.Next(OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer));
					modPlayer.gamblerCardNext[i] = new Item();
					modPlayer.gamblerCardNext[i].SetDefaults(modPlayer.gamblerCardsItem[rand].type, true);
				}
			}

			if (OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) > 3)
			{
				for (int i = 2; i > -1; i--)
				{
					for (int j = 2; j > -1; j--)
					{
						int currentType = modPlayer.gamblerCardNext[i].type;
						if ((currentType == modPlayer.gamblerCardNext[j].type || currentType == modPlayer.gamblerCardCurrent.type) && i != j)
						{
							int k = 0;
							while (k < 5 && (currentType == modPlayer.gamblerCardNext[j].type || currentType == modPlayer.gamblerCardCurrent.type))
							{
								k++;
								int rand = Main.rand.Next(OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer));
								modPlayer.gamblerCardNext[i] = new Item();
								modPlayer.gamblerCardNext[i].SetDefaults(modPlayer.gamblerCardsItem[rand].type, true);
								currentType = modPlayer.gamblerCardNext[i].type;
							}
						}
					}
				}
			}

			modPlayer.gamblerShuffleCooldown = modPlayer.gamblerShuffleCooldownMax;

			if (modPlayer.gamblerDungeon)
			{
				int rand = Main.rand.Next(3);
				for (int i = 0; i < rand; i++)
				{
					OrchidModGamblerHelper.addGamblerChip(100, player, modPlayer);
				}
			}

			if (modPlayer.gamblerPennant)
			{
				OrchidModGlobalItem orchidItem = modPlayer.gamblerCardCurrent.GetGlobalItem<OrchidModGlobalItem>();
				if (orchidItem.gamblerCardSets.Contains("Boss"))
				{
					player.AddBuff(BuffType<Gambler.Buffs.ConquerorsPennantBuff>(), 60 * 10);
				}
			}

			if (modPlayer.gamblerVulture)
			{
				int rand = Main.rand.Next(3) + 1;
				int projType = ProjectileType<Gambler.Projectiles.Equipment.VultureCharmProj>();
				for (int i = 0; i < rand; i++)
				{
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					Vector2 target = Main.MouseWorld;
					Vector2 heading = target - player.Center;
					heading.Normalize();
					heading *= new Vector2(0f, 10f).Length();
					Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(20));
					vel = vel * scale;
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, 12, 0f, player.whoAmI);
					if (i == 0)
					{
						OrchidModProjectile.spawnDustCircle(player.Center, 31, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
					}
				}
			}
			
			modPlayer.gamblerSeedCount = 0;
		}

		public static bool hasGamblerDeck(Player player)
		{
			for (int i = 0; i < Main.maxInventory; i++)
			{
				Item item = player.inventory[i];
				if (item.type != ItemID.None)
				{
					OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
					if (orchidItem.gamblerDeck) return true;
				}
			}
			return false;
		}

		public static int DummyProjectile(int proj, bool dummy)
		{
			if (dummy)
			{
				OrchidModGlobalProjectile modProjectile = Main.projectile[proj].GetGlobalProjectile<OrchidModGlobalProjectile>();
				modProjectile.gamblerDummyProj = true;
			}
			return proj;
		}

		public static int checkSetCardsInDeck(OrchidModPlayer modPlayer, string setName)
		{
			int nbCards = 0;
			for (int i = 0; i < 20; i++)
			{
				OrchidModGlobalItem orchidItem = modPlayer.gamblerCardsItem[i].GetGlobalItem<OrchidModGlobalItem>();
				nbCards += orchidItem.gamblerCardSets.Contains(setName) ? 1 : 0;
			}
			return nbCards;
		}

		public static void ShootBonusProjectiles(Player player, Vector2 position, bool dummy)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (modPlayer.gamblerSlimyLollipop)
			{
				OrchidModGlobalItem orchidItem = modPlayer.gamblerCardCurrent.GetGlobalItem<OrchidModGlobalItem>();
				if (orchidItem.gamblerCardSets.Contains("Slime") && Main.rand.Next(180) == 0)
				{
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					int rand = Main.rand.Next(3) + 1;
					int projType = ProjectileType<Gambler.Projectiles.SlimeRainCardProj2>();
					for (int i = 0; i < rand; i++)
					{
						Vector2 target = Main.MouseWorld;
						Vector2 heading = target - player.position;
						heading.Normalize();
						heading *= new Vector2(0f, 5f).Length();
						Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(30));
						vel = vel * scale;
						int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, 15, 0f, player.whoAmI), dummy);
						Main.projectile[newProjectile].ai[1] = 1f;
						Main.projectile[newProjectile].netUpdate = true;
					}
				}
			}

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile projectile = Main.projectile[l];
				if (projectile.active && projectile.owner == player.whoAmI)
				{
					OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
					if (modProjectile.gamblerDummyProj == dummy && modProjectile.gamblerBonusTrigger)
					{
						modProjectile.gamblerBonusProjectilesDelegate(player, modPlayer, projectile, modProjectile, dummy);
					}
				}
			}
		}
	}
}