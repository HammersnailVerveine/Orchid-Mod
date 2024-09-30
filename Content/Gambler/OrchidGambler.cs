using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Common.Global.Items;
using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Gambler;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
	public class OrchidGambler : ModPlayer
	{
		public OrchidPlayer modPlayer;

		public int gamblerDieAnimation = 0;
		public int gamblerDieAnimationPause = 10;
		public int gamblerDieValueCurrent = 1;
		public int gamblerDieValuePrevious = 1;
		public bool gamblerDieDisplay = false;
		public int gamblerDieDuration = 0;
		public OrchidModGamblerDie gamblerDie = null;
		public int gamblerDieValue = 0;

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
		public int gamblerChipCooldownCurrent = 0;
		public float gamblerChipCooldown = 1f;
		public float gamblerChipSpin = 0;
		public int gamblerUIDisplayTimer = 0;
		public bool gamblerUIFightDisplay = false;
		public bool gamblerUIDeckDisplay = true;
		public bool gamblerUIChipSpinDisplay = false;
		public bool gamblerJustSwitched = false;
		public bool GamblerDeckInHand = false;
		public bool GamblerDummyInHand = false;
		public bool gamblerHasCardInDeck = false;

		public int gamblerPauseChipRotation = 0;
		public int gamblerTimerHoney = 0;
		public int gamblerSeedCount = 0;
		
		public bool gamblerDungeon = false;
		public bool gamblerLuckySprout = false;
		public bool gamblerPennant = false;
		public bool gamblerElementalLens = false;
		public bool gamblerVulture = false;
		public bool gamblerImp = false;
		public bool gamblerSlimyLollipop = false;

		public OrchidModGamblerCard GetCurrentGamblerItem()
		{
			if (gamblerCardCurrent.ModItem is OrchidModGamblerCard gamblerItem)
			{
				return gamblerItem;
			}
			return null;
		}

		public void Reset()
		{
			gamblerShuffleCooldown = 0;
			gamblerChips = 0;
			gamblerRedraws = 0;
			gamblerUIDisplayTimer = 0;
			gamblerChipCooldownCurrent = 0;
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

			if (gamblerDieAnimation >= OrchidModGamblerDie.AnimationDuration + gamblerDieAnimationPause)
			{
				gamblerDieAnimation = 0;
				gamblerDieValuePrevious = gamblerDieValueCurrent;
				for (int i = 0; i < 100; i++)
				{
					gamblerDieValueCurrent = gamblerImp ? Main.rand.Next(4) + 3 : Main.rand.Next(6) + 1;
					if (gamblerDieValueCurrent != gamblerDieValuePrevious) break;
				}
			}
			else gamblerDieAnimation++;

			if (gamblerDie != null)
			{
				gamblerDie.UpdateDie(Player, this);
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
			gamblerUIDisplayTimer = gamblerShuffleCooldown <= 0 && gamblerDieDuration <= 0 ? gamblerUIDisplayTimer > 0 ? gamblerUIDisplayTimer - 1 : gamblerUIDisplayTimer : 300;
			if (gamblerChips > 0 && gamblerUIDisplayTimer <= 0 && modPlayer.Timer120 % 60 == 0)
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

			if (gamblerDieDuration <= 0)
			{
				gamblerDie = null;
				gamblerDieValue = 0;
			}
			else
				gamblerDieDuration--;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			OrchidGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidGlobalProjectile>();
			if (modProjectile.gamblerProjectile)
			{
				if (gamblerDie != null)
				{
					gamblerDie.OnHitNPCWithProj(Player, this, target, hit, damageDone);
				}
			}
		}

		public override void ResetEffects()
		{
			gamblerChipCooldown = 1.0f;
			gamblerChipSpinBonus = 0f;
			gamblerChipsMax = 5;
			gamblerChipsConsume = 0;
			gamblerSeeCards = 0;
			gamblerRedrawsMax = 0;
			gamblerRedrawCooldownMax = 1800;
			gamblerShuffleCooldownMax = 900;
			gamblerChipSpin += gamblerPauseChipRotation > 0 ? 0f : 1.5f + (gamblerChipSpinBonus * 1.5f);
			gamblerChipSpin = gamblerChipSpin > 720f ? gamblerChipSpin - 720f : gamblerChipSpin;
			gamblerPauseChipRotation -= gamblerPauseChipRotation > 0 ? 1 : 0;
			gamblerChipCooldownCurrent += gamblerChipCooldownCurrent < (int)(300 * gamblerChipCooldown) ? 1 : 0; 
			gamblerTimerHoney++;
			gamblerDieAnimationPause = 10;
			gamblerDieDisplay = false;

			GamblerDeckInHand = false;
			GamblerDummyInHand = false;
			gamblerUIFightDisplay = false;
			gamblerUIChipSpinDisplay = false;

			gamblerDungeon = false;
			gamblerLuckySprout = false;
			gamblerPennant = false;
			gamblerElementalLens = false;
			gamblerVulture = false;
			gamblerImp = false;
			gamblerSlimyLollipop = false;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			Reset();
		}

		public override void OnRespawn()
		{
			Reset();
		}

		public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
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

		public void RollGamblerDice(OrchidModGamblerDie die, int duration)
		{
			this.gamblerDie = die;
			gamblerDieDuration = 60 * duration;
			gamblerDieValue = (gamblerDieAnimation > OrchidModGamblerDie.AnimationDuration / 3) ? 0 + gamblerDieValueCurrent : 0 + gamblerDieValuePrevious;
		}

		public void AddGamblerChip(bool resetCooldown = false)
		{
			gamblerChips += gamblerChips < gamblerChipsMax ? 1 : 0;
			if (resetCooldown) gamblerChipCooldownCurrent = 0;
		}

		public void TryAddGamblerChip(int bonusLuck = 0)
		{
			int luck = gamblerChipCooldownCurrent + 1;
			int cap = (int)(600 * gamblerChipCooldown); // 600 = 1 chip guaranteed per 10 sec
			luck += (int)(cap * (bonusLuck / 100));
			luck = luck > cap ? cap : luck;

			if (Main.rand.NextBool(cap / luck))
			{
				AddGamblerChip(true);
				gamblerChipCooldownCurrent = 0;
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
				int rand = Main.rand.Next(2) + 1;
				for (int i = 0; i < rand; i++)
				{
					AddGamblerChip();
				}
			}

			if (gamblerPennant)
			{
				if (GetCurrentGamblerItem().cardSets.Contains(GamblerCardSet.Boss))
				{
					Player.AddBuff(BuffType<Content.Gambler.Buffs.ConquerorsPennantBuff>(), 60 * 10);
				}
			}

			if (gamblerVulture)
			{
				int rand = Main.rand.Next(3) + 1;
				int projType = ProjectileType<Content.Gambler.Projectiles.Equipment.VultureCharmProj>();
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
					OrchidGlobalItemPerEntity orchidItem = item.GetGlobalItem<OrchidGlobalItemPerEntity>();
					if (orchidItem.gamblerDeck) return true;
				}
			}
			return false;
		}

		public int CheckSetCardsInDeck(GamblerCardSet sets)
		{
			int nbCards = 0;
			for (int i = 0; i < 20; i++)
			{
				if (gamblerCardsItem[i].ModItem is OrchidModGamblerCard gamblerItem && gamblerItem.cardSets.Contains(sets))
				{
					nbCards++;
				}
			}
			return nbCards;
		}

		public void ShootBonusProjectiles(bool dummy)
		{
			if (gamblerSlimyLollipop)
			{
				if (GetCurrentGamblerItem().cardSets.Contains(GamblerCardSet.Slime) && Main.rand.NextBool(180))
				{
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					int rand = Main.rand.Next(3) + 1;
					int projType = ProjectileType<Content.Gambler.Projectiles.SlimeRainCardProj2>();
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
					OrchidGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
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
				OrchidGlobalProjectile modProjectile = Main.projectile[proj].GetGlobalProjectile<OrchidGlobalProjectile>();
				modProjectile.gamblerDummyProj = true;
			}
			return proj;
		}
	}
}
