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

namespace OrchidMod
{
	public class OrchidModPlayer : ModPlayer
	{
		public float OchidScreenH = Main.screenHeight;
		public float OchidScreenW = Main.screenWidth;
		public float OchidScreenHCompare;
		public float OchidScreenWCompare;

		public bool abyssalWings = false;
		public bool hauntedCandle = false;
		public bool remoteCopterPet = false;
		public bool spawnedGhost = false;
		public bool doubleJumpHarpy = false;
		public bool harpySpaceKeyReleased = false;
		public int jumpHeightCheck = -1;
		public int originalSelectedItem;
		public bool autoRevertSelectedItem = false;

		public int timer120 = 0;
		public int timerVial = 0;
		public int doubleTap = 0;
		public int doubleTapCooldown = 0;
		public bool doubleTapLock = false;
		public int keepSelected = -1;

		public int customCrit = 0;

		public bool ignoreScrollHotbar = false;

		/*General*/

		public bool generalTools = false;
		public bool generalStatic = false;

		public int generalStaticTimer = 0;
		
		/*Guardian*/
		
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
		
		/*Dancer*/

		public float dancerDamage = 1.0f;
		public int dancerCrit = 0;
		public int dancerPoise = 0;
		public int dancerPoiseConsume = 0;
		public int dancerPoiseMax = 20;
		public int dancerWeaponDamage = 0;
		public float dancerWeaponKnockback = 0f;
		public OrchidModDancerItemType dancerWeaponType = OrchidModDancerItemType.NULL;
		public int dancerDashTimer = 0;
		public int dancerInvincibility = 0;
		public Vector2 dancerVelocity = new Vector2(0f, 0f);

		/*Gambler*/

		public float gamblerDamage = 1.0f;
		public float gamblerDamageChip = 0f;
		public float gamblerChipChance = 1.0f;
		public int gamblerCrit = 0;
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

		/*Alchemist*/

		public List<string> alchemistKnownReactions = new List<string>();
		public List<string> alchemistKnownHints = new List<string>();
		public Item[] alchemistPotionBag = new Item[16];
		public float alchemistDamage = 1.0f;
		public float alchemistVelocity = 1.0f;
		public int alchemistCrit = 0;
		public bool[] alchemistElements = new bool[6];
		public Item[] alchemistFlasks = new Item[6];
		public int alchemistFlaskDamage = 0;
		public int alchemistNbElements = 0; // mp Sync
		public int alchemistNbElementsMax = 2;
		public int alchemistPotencyMax = 8;
		public int alchemistPotency = 100;
		public int alchemistRegenPotency = 60;
		public int alchemistPotencyWait = 0;
		public int alchemistPotencyDisplayTimer = 0;
		public int alchemistResetTimer = 300;
		public int alchemistColorR = 50; // mp Sync
		public int alchemistColorG = 100; // mp Sync
		public int alchemistColorB = 255; // mp Sync
		public int alchemistColorRDisplay = 0;
		public int alchemistColorGDisplay = 0;
		public int alchemistColorBDisplay = 0;
		public bool alchemistSelectUIDisplay = true;
		public bool alchemistSelectUIItem = false;
		public bool alchemistSelectUIInitialize = false;
		public bool alchemistSelectUIKeysDisplay = true;
		public bool alchemistSelectUIKeysItem = false;
		public bool alchemistSelectUIKeysInitialize = false;
		public bool alchemistShootProjectile = false;
		public bool alchemistBookUIDisplay = false;
		public bool alchemistBookUIItem = false;
		public bool alchemistBookUIInitialize = false;
		public bool alchemistDailyHint = false;
		public bool alchemistEntryTextCooldown = false;
		public int alchemistLastAttackDelay = 0;
		public int alchemistHasBag = 5;

		public int alchemistFlower = 0;
		public int alchemistFlowerTimer = 0;
		
		public bool alchemistMeteor = false;
		public bool alchemistFlowerSet = false;
		public bool alchemistMushroomSpores = false;
		public bool alchemistReactiveVials = false;
		public bool alchemistCovent = false;

		/*Shaman*/
		public float shamanDamage = 1.0f;
		public int shamanCrit = 0;
		public int shamanBuffTimer = 6;
		public float shamanExhaustionRate = 1.0f;
		public int UIDisplayTimer = 0;
		public int UIDisplayDelay = 60 * 3; // 3 Seconds

		public int shamanHitDelay = 0;
		public int shamanSelectedItem = 0;
		public int shamanCatalystIndex = 0;

		public ShamanOrbSmall shamanOrbSmall = ShamanOrbSmall.NULL;
		public ShamanOrbBig shamanOrbBig = ShamanOrbBig.NULL;
		public ShamanOrbLarge shamanOrbLarge = ShamanOrbLarge.NULL;
		public ShamanOrbUnique shamanOrbUnique = ShamanOrbUnique.NULL;
		public ShamanOrbCircle shamanOrbCircle = ShamanOrbCircle.NULL;

		public int orbCountSmall = 0;
		public int orbCountBig = 0;
		public int orbCountLarge = 0;
		public int orbCountUnique = 0;
		public int orbCountCircle = 0;

		public int shamanFireTimer = 0;
		public int shamanWaterTimer = 0;
		public int shamanAirTimer = 0;
		public int shamanEarthTimer = 0;
		public int shamanSpiritTimer = 0;

		public int shamanFireBondLoading = 0;
		public int shamanWaterBondLoading = 0;
		public int shamanAirBondLoading = 0;
		public int shamanEarthBondLoading = 0;
		public int shamanSpiritBondLoading = 0;

		public bool shamanPollFireMax = false;
		public bool shamanPollWaterMax = false;
		public bool shamanPollAirMax = false;
		public bool shamanPollEarthMax = false;
		public bool shamanPollSpiritMax = false;

		public int shamanPollFire = 0;
		public int shamanPollWater = 0;
		public int shamanPollAir = 0;
		public int shamanPollEarth = 0;
		public int shamanPollSpirit = 0;

		public bool shamanFire = false;
		public bool shamanIce = false;
		public bool shamanPoison = false;
		public bool shamanVenom = false;
		public bool shamanHoney = false;
		public bool shamanFeather = false;
		public bool shamanDripping = false;
		public bool shamanAmber = false;
		public bool shamanDryad = false;
		public bool shamanForest = false;
		public bool shamanHeavy = false;
		public bool shamanSkull = false;
		public bool shamanWaterHoney = false;
		public bool shamanSmite = false;
		public bool shamanCrimtane = false;
		public bool shamanDemonite = false;
		public bool shamanDownpour = false;
		public bool shamanHell = false;
		public bool shamanHarpyAnklet = false;
		public bool abyssSet = false;
		public bool shamanShadowEmpowerment = false;
		public bool shamanMourningTorch = false;
		public bool shamanSunBelt = false;
		public bool shamanVampire = false;
		public bool shamanDestroyer = false;
		public bool shamanDiabolist = false;
		public bool shamanWyvern = false;
		public bool shamanRage = false;
		public bool shamanAmethyst = false;
		public bool shamanTopaz = false;
		public bool shamanSapphire = false;
		public bool shamanEmerald = false;
		public bool shamanRuby = false;

		public bool harpyAnkletLock = true;
		public int shamanTimerCrimson = 0;
		public int shamanTimerViscount = 0;
		public int shamanTimerHellDamage = 600;
		public int shamanTimerHellDefense = 300;
		public int shamanTimerDestroyer = 0;
		public int shamanDestroyerCount = 0;
		public int shamanTimerDiabolist = 0;
		public int shamanDiabolistCount = 0;

		public Vector2? ShamanCatalystPosition
		{
			get
			{
				var proj = Main.projectile[this.shamanCatalystIndex];
				if (proj == null || !proj.active) return null;

				return proj.Center;
			}
		}

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			if (Player.ZoneSkyHeight && !attempt.inLava && !attempt.inHoney && Main.rand.Next(10) == 0 && Main.hardMode && attempt.rare)
			{
				itemDrop = ModContent.ItemType<Shaman.Weapons.Hardmode.WyvernMoray>();
			}
		}

		public override void Initialize()
		{
			OrchidModGamblerHelper.clearGamblerCards(Player, this);
			OrchidModAlchemistHelper.onRespawnAlchemist(Player, this, Mod);
			OrchidModShamanHelper.onRespawnShaman(Player, this, Mod);
			OrchidModGamblerHelper.onRespawnGambler(Player, this);
			this.alchemistKnownReactions = new List<string>();
			this.alchemistKnownHints = new List<string>();
			
			this.alchemistPotionBag = new Item[16];
			for (int i = 0; i < 16; i++)
			{
				this.alchemistPotionBag[i] = new Item();
				this.alchemistPotionBag[i].SetDefaults(0, true);
			}
		}

		public override void SaveData(TagCompound tag)/* Suggestion: Edit tag parameter rather than returning new TagCompound */
		{
			/*
			return new TagCompound
			{
				["GamblerCardsItem"] = gamblerCardsItem.Select(ItemIO.Save).ToList(),
				["AlchemistBag"] = alchemistPotionBag.Select(ItemIO.Save).ToList(),
				["ChemistHint"] = alchemistDailyHint,
				["AlchemistKnownReactions"] = alchemistKnownReactions.ToList(),
				["AlchemistKnownHints"] = alchemistKnownHints.ToList(),
			};
			*/

			tag.Add("GamblerCardsItem", gamblerCardsItem.Select(ItemIO.Save).ToList());
			tag.Add("AlchemistBag", alchemistPotionBag.Select(ItemIO.Save).ToList());
			tag.Add("ChemistHint", alchemistDailyHint);
			tag.Add("AlchemistKnownReactions", alchemistKnownReactions.ToList());
			tag.Add("AlchemistKnownHints", alchemistKnownHints.ToList());
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
			
			alchemistPotionBag = tag.GetList<TagCompound>("AlchemistBag").Select(ItemIO.Load).ToArray();
			if (alchemistPotionBag.Length != 16)
			{
				Array.Resize(ref alchemistPotionBag, 16);
				for (int i = 0; i < alchemistPotionBag.Length; i++)
				{
					if (alchemistPotionBag[i] == null)
					{
						alchemistPotionBag[i] = new Item();
						alchemistPotionBag[i].SetDefaults(0, true);
					}
				}
			}

			alchemistDailyHint = tag.GetBool("ChemistHint");
			alchemistKnownReactions = tag.Get<List<string>>("AlchemistKnownReactions");
			alchemistKnownHints = tag.Get<List<string>>("AlchemistKnownHints");
		}

		public override void PreUpdate()
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				if (autoRevertSelectedItem)
				{
					if (Player.itemTime == 0 && Player.itemAnimation == 0)
					{
						Player.selectedItem = originalSelectedItem;
						autoRevertSelectedItem = false;
					}
				}
			}
		}

		public override void PostUpdateEquips()
		{
			this.updateBuffEffects();
			this.updateItemEffects();

			this.generalPostUpdateEquips();
			OrchidModShamanHelper.shamanPostUpdateEquips(Player, this, Mod);
			OrchidModAlchemistHelper.alchemistPostUpdateEquips(Player, this, Mod);
			OrchidModGamblerHelper.gamblerPostUpdateEquips(Player, this, Mod);
			OrchidModDancerHelper.dancerPostUpdateEquips(Player, this, Mod);

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				object result = thoriumMod.Call("GetAllCrit", Player);
				if (result is int thoriumCrit && thoriumCrit > 0)
				{
					this.customCrit += thoriumCrit;
				}
			}

			this.shamanCrit += this.customCrit;
			this.alchemistCrit += this.customCrit;
			this.gamblerCrit += this.customCrit;
			this.dancerCrit += this.customCrit;

			this.CheckWoodBreak(Player);
		}

		public override void PostUpdate()
		{
			ignoreScrollHotbar = false;

			OrchidModShamanHelper.postUpdateShaman(Player, this, Mod);
			OrchidModGamblerHelper.postUpdateGambler(Player, this, Mod);
			OrchidModAlchemistHelper.postUpdateAlchemist(Player, this, Mod);
		}

		public override void OnHitNPCWithProj(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.shamanProjectile)
			{
				OrchidModShamanHelper.OnHitNPCWithProjShaman(projectile, target, damage, knockback, crit, Player, this, Mod);
			}
		}

		public override void ModifyHitNPCWithProj(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.alchemistProjectile)
			{
				if (Main.rand.Next(101) <= this.alchemistCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;
			}

			if (modProjectile.gamblerProjectile)
			{
				if (Main.rand.Next(101) <= this.gamblerCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;

				OrchidModGamblerHelper.ModifyHitNPCWithProjGambler(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection, Player, this, Mod);
			}

			if (modProjectile.shamanProjectile)
			{
				if (Main.rand.Next(101) <= this.shamanCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;

				OrchidModShamanHelper.ModifyHitNPCWithProjShaman(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection, Player, this, Mod);
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			OrchidModShamanHelper.DrawEffectsShaman(drawInfo, ref r, ref g, ref b, ref a, ref fullBright, Player, this, Mod);
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
		ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			bool toReturn = true;

			if (!OrchidModShamanHelper.PreHurtShaman(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, Player, this, Mod))
			{
				toReturn = false;
			}

			if (!OrchidModDancerHelper.PreHurtDancer(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, Player, this, Mod))
			{
				toReturn = false;
			}
			return toReturn;
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (OrchidMod.AlchemistCatalystHotKey.JustPressed && Player.itemAnimation == 0)
			{
				for (int i = 0; i < Main.InventorySlotsTotal; i++)
				{
					Item item = Main.LocalPlayer.inventory[i];
					if (item.type != 0)
					{
						OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
						if (orchidItem.alchemistCatalyst)
						{
							this.originalSelectedItem = Player.selectedItem;
							this.autoRevertSelectedItem = true;
							Player.selectedItem = i;
							Player.controlUseItem = true;
							Player.ItemCheck(Player.whoAmI);
							return;
						}
					}
				}
			}

			if (OrchidMod.AlchemistReactionHotKey.JustPressed)
			{
				if (this.alchemistNbElements < 2 || Player.FindBuffIndex(Mod.Find<ModBuff>("ReactionCooldown").Type) > -1)
				{
					return;
				}
				else
				{
					AlchemistHiddenReactionHelper.triggerAlchemistReaction(Mod, Player, this);
				}
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			OrchidModDancerHelper.ModifyHitByNPCDancer(npc, ref damage, ref crit, Player, this, Mod);
		}

		public override void ResetEffects()
		{
			customCrit = 0;
			remoteCopterPet = false;

			generalTools = false;
			generalStatic = false;

			OrchidModDancerHelper.ResetEffectsDancer(Player, this, Mod);
			OrchidModAlchemistHelper.ResetEffectsAlchemist(Player, this, Mod);
			OrchidModGamblerHelper.ResetEffectsGambler(Player, this, Mod);
			OrchidModShamanHelper.ResetEffectsShaman(Player, this, Mod);
			OrchidModGuardianHelper.ResetEffectsGuardian(Player, this, Mod);

			if (this.keepSelected != -1)
			{
				Player.selectedItem = keepSelected;
				this.keepSelected = -1;
			}
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			OrchidModAlchemistHelper.onRespawnAlchemist(Player, this, Mod);
			OrchidModShamanHelper.onRespawnShaman(Player, this, Mod);
			OrchidModGamblerHelper.onRespawnGambler(Player, this);
		}

		public override void OnRespawn(Player player)
		{
			OrchidModAlchemistHelper.onRespawnAlchemist(player, this, Mod);
			OrchidModShamanHelper.onRespawnShaman(player, this, Mod);
			OrchidModGamblerHelper.onRespawnGambler(player, this);
		}

		public void updateBuffEffects()
		{
			if (Player.FindBuffIndex(26) > -1)
			{ // WELL FED
				this.customCrit += 2;
			}

			if (Player.FindBuffIndex(115) > -1)
			{ // RAGE
				this.customCrit += 10;
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (Player.HasBuff(Mod.Find<ModBuff>("EarthTotemPlus").Type) && !(Player.HasBuff(Mod.Find<ModBuff>("EarthTotemRevive").Type)))
			{
				SoundEngine.PlaySound(SoundID.Item29, Player.position);
				Player.AddBuff(Mod.Find<ModBuff>("EarthTotemRevive").Type, 60 * 60 * 5);
				for (int i = 0; i < 15; i++)
				{
					int randX = Main.rand.Next(150);
					int randY = Main.rand.Next(100);
					int dust = Dust.NewDust(new Vector2((int)(Player.Center.X + 75 - randX), (int)(Player.Center.Y + 15 - randY)), 0, 0, 64, 0f, 0f, 0, default(Color), 1.3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 3f;
				}
				if (Main.myPlayer == Player.whoAmI)
					Player.HealEffect(200, true);
				Player.statLife = 200;
				return false;
			}
			return true;
		}

		public void updateItemEffects()
		{
			if (Player.armor[1].type == 374) this.customCrit += 3;// COBALT BREASPLATE
			if (Player.armor[1].type == 1208) this.customCrit += 2; // PALLADIUM BREASTPLATE
			if (Player.armor[2].type == 1209) this.customCrit += 1; // PALLADIUM LEGS
			if (Player.armor[2].type == 380) this.customCrit += 3; // MYTHRIL LEGS
			if (Player.armor[1].type == 1213) this.customCrit += 6; // ORICHALCUM BREASPLATE
			if (Player.armor[2].type == 404) this.customCrit += 4; // ADAMANTITE LEGS
			if (Player.armor[1].type == 1218) this.customCrit += 3; // TITANIUM BREASTPLATE
			if (Player.armor[2].type == 1219) this.customCrit += 3; // TITANIUM LEGS
			if (Player.armor[2].type == 2277) this.customCrit += 5; // GI
			if (Player.armor[1].type == 551) this.customCrit += 7; // HALLOWED BREASPLATE
			if (Player.armor[1].type == 1004) this.customCrit += 7; // CHLOROPHITE BREASTPLATE
			if (Player.armor[2].type == 1005) this.customCrit += 8; // CHLOROPHITE LEGS

			for (int k = 3; k < 8 + Player.extraAccessorySlots; k++)
			{
				if (Player.armor[k].type == 1301) this.customCrit += 8; // DESTROYER EMBLEM
				if (Player.armor[k].type == 1248) this.customCrit += 10; // EYE OF THE GOLEM
				if (Player.armor[k].type == 3015) this.customCrit += 5; // PUTRID SCENT
				if (Player.armor[k].type == 3110) this.customCrit += 2; // CELESTIAL SHELL
				if (Player.armor[k].type == 1865) this.customCrit += 2; // CELESTIAL STONE
				if (Player.armor[k].type == 899 && Main.dayTime) this.customCrit += 2; // CELESTIAL STONE
				if (Player.armor[k].type == 900 && (!Main.dayTime || Main.eclipse)) this.customCrit += 2; // CELESTIAL STONE

				if (Player.armor[k].prefix == PrefixID.Lucky) this.customCrit += 4;
				if (Player.armor[k].prefix == PrefixID.Precise) this.customCrit += 2;
			}
		}

		public void generalPostUpdateEquips()
		{
			if (generalStaticTimer == 299)
			{
				SoundEngine.PlaySound(SoundID.Item93, Player.position);
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Player.position, Player.width, Player.height, 60);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= 1.5f;
				}
			}
			if ((Player.velocity.X != 0f || Player.velocity.Y != 0f) && generalStaticTimer >= 300)
			{
				Player.AddBuff(BuffType<Buffs.StaticQuartArmorBuff>(), 60 * 10);
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Player.position, Player.width, Player.height, 60);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= 1.5f;
				}
			}
			generalStaticTimer = (generalStatic && Player.velocity.X == 0f && Player.velocity.Y == 0f) ? generalStaticTimer < 300 ? generalStaticTimer + 1 : 300 : 0;
		}
		
		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) {
			return OrchidModGuardianHelper.CanBeHitByNPCGuardian(npc, ref cooldownSlot, Player, this, Mod);
		}

		public void CheckWoodBreak(Player player)
		{ // From Vanilla Source
			if (player.velocity.Y <= 1f || this.generalTools)
				return;
			Vector2 vector2 = player.position + player.velocity;
			int num1 = (int)(vector2.X / 16.0);
			int num2 = (int)((vector2.X + (double)player.width) / 16.0);
			int num3 = (int)((player.position.Y + (double)player.height + 1.0) / 16.0);
			for (int i = num1; i <= num2; ++i)
			{
				for (int j = num3; j <= num3 + 1; ++j)
				{
					if (Main.tile[i, j].HasUnactuatedTile && (int)Main.tile[i, j].TileType == TileType<Tiles.Ambient.FragileWood>() && !WorldGen.SolidTile(i, j - 1))
					{
						WorldGen.KillTile(i, j, false, false, false);
						// if (Main.netMode == 1)
						// NetMessage.SendData(17, -1, -1, (NetworkText) null, 0, (float) i, (float) j, 0.0f, 0, 0, 0);
					}
				}
			}
		}
		public override void clientClone(ModPlayer clientClone)
		{
			OrchidModPlayer clone = clientClone as OrchidModPlayer;

			clone.shamanOrbSmall = this.shamanOrbSmall;
			clone.shamanOrbBig = this.shamanOrbBig;
			clone.shamanOrbLarge = this.shamanOrbLarge;
			clone.shamanOrbUnique = this.shamanOrbUnique;
			clone.shamanOrbCircle = this.shamanOrbCircle;

			clone.orbCountSmall = this.orbCountSmall;
			clone.orbCountBig = this.orbCountBig;
			clone.orbCountLarge = this.orbCountLarge;
			clone.orbCountUnique = this.orbCountUnique;
			clone.orbCountCircle = this.orbCountCircle;

			clone.gamblerHasCardInDeck = this.gamblerHasCardInDeck;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)OrchidModMessageType.ORCHIDPLAYERSYNCPLAYER);
			packet.Write((byte)Player.whoAmI);

			packet.Write((byte)shamanOrbSmall);
			packet.Write((byte)shamanOrbBig);
			packet.Write((byte)shamanOrbLarge);
			packet.Write((byte)shamanOrbUnique);
			packet.Write((byte)shamanOrbCircle);

			packet.Write(orbCountSmall);
			packet.Write(orbCountBig);
			packet.Write(orbCountLarge);
			packet.Write(orbCountUnique);
			packet.Write(orbCountCircle);

			packet.Write(shamanFireTimer);
			packet.Write(shamanWaterTimer);
			packet.Write(shamanAirTimer);
			packet.Write(shamanEarthTimer);
			packet.Write(shamanSpiritTimer);

			packet.Write(gamblerHasCardInDeck);

			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			OrchidModPlayer clone = clientPlayer as OrchidModPlayer;

			//Orb Types
			if (clone.shamanOrbSmall != shamanOrbSmall)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbSmall);
				packet.Send();
			}

			if (clone.shamanOrbBig != shamanOrbBig)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDBIG);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbBig);
				packet.Send();
			}

			if (clone.shamanOrbLarge != shamanOrbLarge)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbLarge);
				packet.Send();
			}

			if (clone.shamanOrbUnique != shamanOrbUnique)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbUnique);
				packet.Send();
			}

			if (clone.shamanOrbCircle != shamanOrbCircle)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbCircle);
				packet.Send();
			}

			// Orb Counts
			if (clone.orbCountSmall != orbCountSmall)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountSmall);
				packet.Send();
			}

			if (clone.orbCountBig != orbCountBig)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountBig);
				packet.Send();
			}

			if (clone.orbCountLarge != orbCountLarge)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountLarge);
				packet.Send();
			}

			if (clone.orbCountUnique != orbCountUnique)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountUnique);
				packet.Send();
			}

			if (clone.orbCountCircle != orbCountCircle)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountCircle);
				packet.Send();
			}

			//Empowerment Timers
			if (clone.shamanFireTimer != shamanFireTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanFireTimer);
				packet.Send();
			}

			if (clone.shamanWaterTimer != shamanWaterTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanWaterTimer);
				packet.Send();
			}

			if (clone.shamanAirTimer != shamanAirTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanAirTimer);
				packet.Send();
			}

			if (clone.shamanEarthTimer != shamanEarthTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanEarthTimer);
				packet.Send();
			}

			if (clone.shamanSpiritTimer != shamanSpiritTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanSpiritTimer);
				packet.Send();
			}

			if (clone.gamblerHasCardInDeck != gamblerHasCardInDeck)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
				packet.Write((byte)Player.whoAmI);
				packet.Write(gamblerHasCardInDeck);
				packet.Send();
			}
		}
	}
}
