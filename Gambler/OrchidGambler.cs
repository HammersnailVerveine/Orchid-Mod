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
using OrchidMod.Gambler.Buffs.Dice;

namespace OrchidMod
{
	public class OrchidGambler : ModPlayer
	{
		public OrchidPlayer modPlayer;

		public float gamblerChipChance = 1.0f;
		public Item[] gamblerCardsItem = new Item[20];
		public Item[] gamblerCardNext = new Item[3];
		public Item gamblerCardCurrent = new Item();
		public Item gamblerCardDummy = new Item();
		public int gamblerShuffleCooldown = 0;
		public int gamblerShuffleCooldownMax = 900;
		public int gamblerChips = 0;
		public int gamblerChipsMax = 5;
		public int gamblerChipsConsume = 0;
		public int gamblerSeeCards = 0;
		public int gamblerRedraws = 0;
		public int gamblerRedrawsMax = 0;
		public float gamblerChipSpinBonus = 0f;
		public int gamblerRedrawCooldown = 0;
		public int gamblerRedrawCooldownMax = 1800;
		public int gamblerRedrawCooldownUse = 0;
		public int gamblerDiceID = -1;
		public int gamblerDiceValue = 0;
		public int gamblerDiceDuration = 0;
		public int gamblerUIDisplayTimer = 0;
		public bool gamblerUIFightDisplay = false;
		public bool gamblerUIDeckDisplay = true;
		public bool gamblerUIChipSpinDisplay = false;
		public bool gamblerJustSwitched = false;
		public bool GamblerDeckInHand = false;
		public bool GamblerDummyInHand = false;
		public bool gamblerHasCardInDeck = false;

		public float gamblerChipSpin = 0;
		public int gamblerPauseChipRotation = 0;
		public int gamblerTimerHoney = 0;
		public int gamblerSeedCount = 0;
		
		public bool gamblerDungeon = false;
		public bool gamblerLuckySprout = false;
		public bool gamblerPennant = false;
		public bool gamblerElementalLens = false;
		public bool gamblerVulture = false;
		public bool gamblerSlimyLollipop = false;

		public void Reset()
		{
			gamblerShuffleCooldown = 0;
			gamblerChips = 0;
			gamblerRedraws = 0;
			gamblerUIDisplayTimer = 0;
			ClearGamblerCardCurrent();
			ClearGamblerCardsNext();
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();

			ClearGamblerCards();
			Reset();
		}

		public override void SaveData(TagCompound tag)/* Suggestion: Edit tag parameter rather than returning new TagCompound */
		{
			tag.Add("GamblerCardsItem", gamblerCardsItem.Select(ItemIO.Save).ToList());
		}

		public override void LoadData(TagCompound tag)
		{
			gamblerCardsItem = tag.GetList<TagCompound>("GamblerCardsItem").Select(ItemIO.Load).ToArray();
			//If no cards were saved (old character, crash, etc), this can return Item[] of length 0
			//In case of length not being equal to 20, fix the array
			if (gamblerCardsItem.Length != 20)
			{
				Array.Resize(ref gamblerCardsItem, 20);
				for (int i = 0; i < gamblerCardsItem.Length; i++)
				{
					if (gamblerCardsItem[i] == null)
					{
						gamblerCardsItem[i] = new Item();
						gamblerCardsItem[i].SetDefaults(0, true);
					}
				}
			}
		}

		public override void PostUpdateEquips()
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				//Since this is a variable that is synced through SendClientChanges, it has to only be assigned on the client aswell
				gamblerHasCardInDeck = gamblerCardsItem[0].type != 0;
			}

			if (gamblerRedrawsMax > 0)
			{
				gamblerRedrawCooldown = gamblerRedraws >= gamblerRedrawsMax ? gamblerRedrawCooldownMax : gamblerRedrawCooldown;
				gamblerRedrawCooldown = gamblerRedrawCooldown > gamblerRedrawCooldownMax ? gamblerRedrawCooldownMax : gamblerRedrawCooldown;
				gamblerRedrawCooldown = gamblerRedrawCooldown <= 0 ? gamblerRedrawCooldownMax : gamblerRedrawCooldown - 1;
				if (gamblerRedrawCooldown <= 0 && gamblerRedraws < gamblerRedrawsMax)
				{
					gamblerRedraws++;
				}
			}
			else
			{
				gamblerRedrawCooldown = -1;
			}

			gamblerRedrawCooldownUse -= gamblerRedrawCooldownUse > 0 ? 1 : 0;
			gamblerShuffleCooldown -= gamblerShuffleCooldown > 0 ? 1 : 0;
			gamblerUIDisplayTimer = gamblerShuffleCooldown <= 0 && gamblerDiceDuration <= 0 ? gamblerUIDisplayTimer > 0 ? gamblerUIDisplayTimer - 1 : gamblerUIDisplayTimer : 300;
			if (gamblerChips > 0 && gamblerUIDisplayTimer <= 0 && modPlayer.timer120 % 60 == 0)
			{
				gamblerChips--;
				gamblerUIDisplayTimer = gamblerChips == 0 ? 60 : 0;
			}

			if (gamblerUIDisplayTimer == 0 && gamblerChips == 0 && gamblerCardCurrent.type == 0)
			{
				ClearGamblerCardCurrent();
				gamblerRedraws = 0;
				ClearGamblerCardsNext();
			}

			if (gamblerRedraws > gamblerRedrawsMax) gamblerRedraws = gamblerRedrawsMax;
			if (gamblerChips > gamblerChipsMax) gamblerChips = gamblerChipsMax;

			if (gamblerDiceDuration <= 0)
			{
				gamblerDiceID = -1;
				gamblerDiceValue = 0;
			}
			else
			{
				gamblerDiceDuration--;
				switch (gamblerDiceID)
				{
					case 0:
						Player.GetDamage<GamblerDamageClass>() += (0.03f * gamblerDiceValue);
						break;
					case 1:
						gamblerChipsConsume += 4 * gamblerDiceValue;
						break;
					default:
						break;
				}
			}
		}

		public override void PostUpdate()
		{
			switch (gamblerDiceID)
			{
				case 0:
					Player.AddBuff(ModContent.BuffType<GamblerDice>(), 1);
					break;
				case 1:
					Player.AddBuff(ModContent.BuffType<GemstoneDice>(), 1);
					break;
				case 2:
					Player.AddBuff(ModContent.BuffType<HoneyDice>(), 1);
					break;
				default:
					break;
			}

			if (gamblerTimerHoney < 30)
			{
				gamblerTimerHoney++;
			}
		}

		public override void ModifyHitNPCWithProj(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.gamblerProjectile)
			{
				/*
				if (Main.rand.Next(101) <= this.gamblerCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;
				*/

				if (gamblerDiceID == 2 && gamblerTimerHoney == 30)
				{
					Player.HealEffect(gamblerDiceValue, true);
					Player.statLife += gamblerDiceValue;
					gamblerTimerHoney = 0;
				}
			}
		}

		public override void ResetEffects()
		{
			gamblerChipSpin += gamblerPauseChipRotation > 0 ? 0f : 1.5f + (gamblerChipSpinBonus * 1.5f);
			gamblerChipSpin = gamblerChipSpin > 720f ? gamblerChipSpin - 720f : gamblerChipSpin;
			gamblerPauseChipRotation -= gamblerPauseChipRotation > 0 ? 1 : 0;
			gamblerChipChance = 1.0f;
			gamblerChipSpinBonus = 0f;
			gamblerChipsMax = 5;
			gamblerChipsConsume = 0;
			gamblerSeeCards = 0;
			gamblerRedrawsMax = 0;
			gamblerRedrawCooldownMax = 1800;
			gamblerShuffleCooldownMax = 900;
			GamblerDeckInHand = false;
			GamblerDummyInHand = false;
			gamblerUIFightDisplay = false;
			gamblerUIChipSpinDisplay = false;

			gamblerDungeon = false;
			gamblerLuckySprout = false;
			gamblerPennant = false;
			gamblerElementalLens = false;
			gamblerVulture = false;
			gamblerSlimyLollipop = false;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			Reset();
		}

		public override void OnRespawn(Player player)
		{
			Reset();
		}

		public override void clientClone(ModPlayer clientClone)
		{
			OrchidGambler clone = clientClone as OrchidGambler;
			clone.gamblerHasCardInDeck = this.gamblerHasCardInDeck;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)OrchidModMessageType.ORCHIDPLAYERSYNCPLAYERGAMBLER);
			packet.Write((byte)Player.whoAmI);

			packet.Write(gamblerHasCardInDeck);

			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			OrchidGambler clone = clientPlayer as OrchidGambler;
			if (clone.gamblerHasCardInDeck != gamblerHasCardInDeck)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
				packet.Write((byte)Player.whoAmI);
				packet.Write(gamblerHasCardInDeck);
				packet.Send();
			}
		}
		public void ClearGamblerCardsNext()
		{
			gamblerCardNext = new Item[3];
			for (int i = 0; i < 3; i++)
			{
				gamblerCardNext[i] = new Item();
				gamblerCardNext[i].SetDefaults(0, true);
			}
		}

		public void ClearGamblerCardCurrent()
		{
			gamblerCardCurrent = new Item();
			gamblerCardCurrent.SetDefaults(0, true);
		}

		public void ClearGamblerCards()
		{
			gamblerCardsItem = new Item[20];
			for (int i = 0; i < 20; i++)
			{
				gamblerCardsItem[i] = new Item();
				gamblerCardsItem[i].SetDefaults(0, true);
			}

			gamblerCardCurrent = new Item();
			gamblerCardCurrent.SetDefaults(0, true);
		}

		public void RemoveGamblerCard(Item card)
		{
			if (ContainsGamblerCard(card))
			{
				bool found = false;
				for (int i = 0; i < 20; i++)
				{
					if (gamblerCardsItem[i].type == card.type)
					{
						found = true;
					}
					if (found)
					{
						gamblerCardsItem[i] = new Item();
						gamblerCardsItem[i].SetDefaults(i == 19 ? 0 : gamblerCardsItem[i + 1].type, true);
					}
				}
			}
		}

		public int GetNbGamblerCards()
		{
			if (gamblerCardsItem.Count() != 20)
			{
				ClearGamblerCards();
			}
			int val = 0;
			for (int i = 0; i < 20; i++)
			{
				if (gamblerCardsItem[i].type != 0)
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

		public bool ContainsGamblerCard(Item card)
		{
			for (int i = 0; i < 20; i++)
			{
				if (gamblerCardsItem[i].type == card.type)
				{
					return true;
				}
			}
			return false;
		}

		public void RollGamblerDice(int diceID, int diceDuration)
		{
			gamblerDiceID = diceID;
			gamblerDiceValue = Main.rand.Next(6) + 1;
			gamblerDiceDuration = 60 * diceDuration;
		}

		public void AddGamblerChip(int chance)
		{
			chance = (int)(chance * gamblerChipChance);
			if (Main.rand.Next(100) < chance)
			{
				gamblerChips += gamblerChips < gamblerChipsMax ? 1 : 0;
			}
			gamblerUIDisplayTimer = 300;
		}

		public void RemoveGamblerChip(int chance, int number)
		{
			for (int i = 0; i < number; i++)
			{
				if (Main.rand.Next(100) < (chance - gamblerChipsConsume))
				{
					gamblerChips--;
				}
			}
			gamblerUIDisplayTimer = 300;
		}

		public void DrawDummyCard()
		{
			if (gamblerCardDummy.type == 0)
			{
				if (gamblerCardsItem[0].type != 0)
				{
					gamblerCardDummy = new Item();
					gamblerCardDummy.SetDefaults(gamblerCardsItem[0].type, true);
				}
				return;
			}

			for (int i = 0; i < 20; i++)
			{
				if (i == 19)
				{
					gamblerCardDummy = new Item();
					gamblerCardDummy.SetDefaults(gamblerCardsItem[0].type, true);
				}

				if (gamblerCardsItem[i].type == gamblerCardDummy.type)
				{
					if (gamblerCardsItem[i + 1].type != 0)
					{
						gamblerCardDummy = new Item();
						gamblerCardDummy.SetDefaults(gamblerCardsItem[i + 1].type, true);
					}
					else
					{
						gamblerCardDummy = new Item();
						gamblerCardDummy.SetDefaults(gamblerCardsItem[0].type, true);
					}
					break;
				}
			}
		}

		public void DrawGamblerCard()
		{
			gamblerJustSwitched = true;

			if (gamblerCardNext.Count() != 3)
			{
				ClearGamblerCardsNext();
			}

			for (int i = 0; i < 3; i++)
			{
				if (gamblerCardNext[i].type == 0)
				{
					int rand = Main.rand.Next(GetNbGamblerCards());
					gamblerCardNext[i] = new Item();
					gamblerCardNext[i].SetDefaults(gamblerCardsItem[rand].type, true);
				}
			}

			gamblerCardCurrent = new Item();
			gamblerCardCurrent.SetDefaults(gamblerCardNext[0].type, true);

			for (int i = 0; i < 2; i++)
			{
				gamblerCardNext[i] = new Item();
				gamblerCardNext[i].SetDefaults(gamblerCardNext[i + 1].type, true);
			}

			gamblerCardNext[2] = new Item();
			gamblerCardNext[2].SetDefaults(0, true);

			for (int i = 0; i < 3; i++)
			{
				if (gamblerCardNext[i].type == 0)
				{
					int rand = Main.rand.Next(GetNbGamblerCards());
					gamblerCardNext[i] = new Item();
					gamblerCardNext[i].SetDefaults(gamblerCardsItem[rand].type, true);
				}
			}

			if (GetNbGamblerCards() > 3)
			{
				for (int i = 2; i > -1; i--)
				{
					for (int j = 2; j > -1; j--)
					{
						int currentType = gamblerCardNext[i].type;
						if ((currentType == gamblerCardNext[j].type || currentType == gamblerCardCurrent.type) && i != j)
						{
							int k = 0;
							while (k < 5 && (currentType == gamblerCardNext[j].type || currentType == gamblerCardCurrent.type))
							{
								k++;
								int rand = Main.rand.Next(GetNbGamblerCards());
								gamblerCardNext[i] = new Item();
								gamblerCardNext[i].SetDefaults(gamblerCardsItem[rand].type, true);
								currentType = gamblerCardNext[i].type;
							}
						}
					}
				}
			}

			gamblerShuffleCooldown = gamblerShuffleCooldownMax;

			if (gamblerDungeon)
			{
				int rand = Main.rand.Next(3);
				for (int i = 0; i < rand; i++)
				{
					AddGamblerChip(100);
				}
			}

			if (gamblerPennant)
			{
				OrchidModGlobalItem orchidItem = gamblerCardCurrent.GetGlobalItem<OrchidModGlobalItem>();
				if (orchidItem.gamblerCardSets.Contains("Boss"))
				{
					Player.AddBuff(BuffType<Gambler.Buffs.ConquerorsPennantBuff>(), 60 * 10);
				}
			}

			if (gamblerVulture)
			{
				int rand = Main.rand.Next(3) + 1;
				int projType = ProjectileType<Gambler.Projectiles.Equipment.VultureCharmProj>();
				for (int i = 0; i < rand; i++)
				{
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					Vector2 target = Main.MouseWorld;
					Vector2 heading = target - Player.Center;
					heading.Normalize();
					heading *= new Vector2(0f, 10f).Length();
					Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(20));
					vel = vel * scale;
					Projectile.NewProjectile(null, Player.Center.X, Player.Center.Y, vel.X, vel.Y, projType, 12, 0f, Player.whoAmI);
					if (i == 0)
					{
						OrchidModProjectile.spawnDustCircle(Player.Center, 31, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
					}
				}
			}

			gamblerSeedCount = 0;
		}

		public bool HasGamblerDeck()
		{
			for (int i = 0; i < Main.InventorySlotsTotal; i++)
			{
				Item item = Player.inventory[i];
				if (item.type != ItemID.None)
				{
					OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
					if (orchidItem.gamblerDeck) return true;
				}
			}
			return false;
		}

		public int CheckSetCardsInDeck(string setName)
		{
			int nbCards = 0;
			for (int i = 0; i < 20; i++)
			{
				OrchidModGlobalItem orchidItem = gamblerCardsItem[i].GetGlobalItem<OrchidModGlobalItem>();
				nbCards += orchidItem.gamblerCardSets.Contains(setName) ? 1 : 0;
			}
			return nbCards;
		}

		public void ShootBonusProjectiles(bool dummy)
		{
			if (gamblerSlimyLollipop)
			{
				OrchidModGlobalItem orchidItem = gamblerCardCurrent.GetGlobalItem<OrchidModGlobalItem>();
				if (orchidItem.gamblerCardSets.Contains("Slime") && Main.rand.Next(180) == 0)
				{
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					int rand = Main.rand.Next(3) + 1;
					int projType = ProjectileType<Gambler.Projectiles.SlimeRainCardProj2>();
					for (int i = 0; i < rand; i++)
					{
						int newProjectile = OrchidGambler.DummyProjectile(Projectile.NewProjectile(null, Player.Center.X, Player.Center.Y, 0f, 5f, projType, 15, 0f, Player.whoAmI), dummy);
						Main.projectile[newProjectile].ai[1] = 1f;
						Main.projectile[newProjectile].netUpdate = true;
					}
				}
			}

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile projectile = Main.projectile[l];
				if (projectile.active && projectile.owner == Player.whoAmI)
				{
					OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
					if (modProjectile.gamblerDummyProj == dummy && modProjectile.gamblerBonusTrigger)
					{
						modProjectile.gamblerBonusProjectilesDelegate(Player, this, projectile, modProjectile, dummy);
					}
				}
			}
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
	}
}
