using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using OrchidMod.Shaman;
using OrchidMod.Alchemist;
using OrchidMod.Gambler;
using OrchidMod.Dancer;
using OrchidMod;
using OrchidMod.Dusts;
using static Terraria.ModLoader.ModContent;
using Terraria.UI;

namespace OrchidMod
{
	public class OrchidModPlayer : ModPlayer
	{
		public Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
		
		public float OchidScreenH = Main.screenHeight;
		public float OchidScreenW = Main.screenWidth;
		public float OchidScreenHCompare;
		public float OchidScreenWCompare;
		
		public bool abyssalWings = false;
		public bool hauntedCandle = false;
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
	
		public int customCrit = 0;
		
		/*General*/
		
		public bool generalTools = false;
		public bool generalStatic = false;
		
		public int generalStaticTimer = 0;
		
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
		public float gamblerChipChance = 1.0f;
		public int gamblerCrit = 0;
		public Item[] gamblerCardsItem = new Item[20];
		public Item[] gamblerCardNext = new Item[3];
		public Item gamblerCardCurrent = new Item();
		public Item gamblerCardDummy = new Item();
		public int gamblerShuffleCooldown = 0;
		public int gamblerShuffleCooldownMax = 600;
		public int gamblerChips = 0;
		public int gamblerChipsMax = 5;
		public int gamblerChipsConsume = 0;
		public int gamblerSeeCards = 0;
		public int gamblerRedraws = 0;
		public int gamblerRedrawsMax = 0;
		public int gamblerRedrawCooldown = 0;
		public int gamblerRedrawCooldownMax = 1800;
		public int gamblerRedrawCooldownUse = 0;
		public int gamblerDiceID = -1;
		public int gamblerDiceValue = 0;
		public int gamblerDiceDuration = 0;
		public int gamblerUIDisplayTimer = 0;
		public bool gamblerUIDeckDisplay = true;
		public bool gamblerJustSwitched = false;
		public bool gamblerAttackInHand = false;
		public bool gamblerHasCardInDeck = false;
		
		public int gamblerTimerHoney = 0;
		public bool gamblerDungeon = false;
		public bool gamblerLuckySprout = false;
		public bool gamblerPennant = false;
		public bool gamblerVulture = false;
		public bool gamblerSlimyLollipop = false;
	
		/*Alchemist*/
		
		public List<int> alchemistKnownReactions = new List<int>();
		public List<int> alchemistKnownHints = new List<int>();
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
		public bool alchemistChemistFirstInteraction = false;
		public bool alchemistShootProjectile = false;
		public bool alchemistBookUIDisplay = false;
		public bool alchemistBookUIItem = false;
		public bool alchemistBookUIInitialize = false;
		
		public bool alchemistMeteor = false;
		public bool alchemistFlowerSet = false;
		public bool alchemistMushroomSpores	= false;
		public bool alchemistReactiveVials = false;
		public int alchemistFlower = 0;
		public int alchemistFlowerTimer = 0;

		/*Shaman*/
		public float shamanDamage = 1.0f;
		public int shamanCrit = 0;
		public int shamanBuffTimer = 5;
		public int UIDisplayTimer = 0;
		public int UIDisplayDelay = 60 * 3; // 3 Seconds
		
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
		
		public int shamanFireBuff = 0;
		public int shamanWaterBuff = 0;
		public int shamanAirBuff = 0;
		public int shamanEarthBuff = 0;
		public int shamanSpiritBuff = 0;
		
		public int shamanFireTimer = 0;
		public int shamanWaterTimer = 0;
		public int shamanAirTimer = 0;
		public int shamanEarthTimer = 0;
		public int shamanSpiritTimer = 0;
		
		public int shamanFireBonus = 0;
		public int shamanWaterBonus = 0;
		public int shamanAirBonus = 0;
		public int shamanEarthBonus = 0;
		public int shamanSpiritBonus = 0;
		
		public int shamanFireBondLoading = 0;
		public int shamanWaterBondLoading = 0;
		public int shamanAirBondLoading = 0;
		public int shamanEarthBondLoading = 0;
		public int shamanSpiritBondLoading = 0;
		
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
		
		public bool harpyAnkletLock = true;
		public int shamanTimerCrimson = 0;
		public int shamanTimerViscount = 0;
		public int shamanTimerHellDamage = 600;
		public int shamanTimerHellDefense = 300;
		public int shamanTimerDestroyer = 0;
		public int shamanDestroyerCount = 0;
		public int shamanTimerDiabolist = 0;
		public int shamanDiabolistCount = 0;
		public int shamanTimerImmobile = 0;
		public int shamanTimerCombat = 0;
		public int shamanHitDelay = 0;
		
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk) {
			if (junk) {
				return;
			}
			if (player.ZoneSkyHeight && liquidType == 0 && Main.rand.Next(100) == 0 && Main.hardMode)
			{
                caughtType = mod.ItemType("WyvernMoray");
            }
		}
		
		public override void Initialize()
        {
			OrchidModGamblerHelper.clearGamblerCards(player, this);
			OrchidModAlchemistHelper.onRespawnAlchemist(player, this, mod);
			OrchidModShamanHelper.onRespawnShaman(player, this, mod);
			OrchidModGamblerHelper.onRespawnGambler(player, this);
			this.alchemistKnownReactions = new List<int>();
			this.alchemistKnownHints = new List<int>();
        }
		
		public override TagCompound Save()
		{
			return new TagCompound
			{
				["GamblerCardsItem"] = gamblerCardsItem.Select(ItemIO.Save).ToList(),
				["ChemistHint"] = alchemistChemistFirstInteraction,
				["AlchemistHidden"] = alchemistKnownReactions.ToList(),
				["AlchemistHints"] = alchemistKnownHints.ToList()
			};
		}
		
		public override void Load(TagCompound tag)
		{
			gamblerCardsItem = tag.GetList<TagCompound>("GamblerCardsItem").Select(ItemIO.Load).ToArray();
			alchemistChemistFirstInteraction = tag.GetBool("ChemistHint");
			alchemistKnownReactions = tag.Get<List<int>>("AlchemistHidden");
			alchemistKnownHints = tag.Get<List<int>>("AlchemistHints");
		}

		public override void PreUpdate() {
			if (player.whoAmI == Main.myPlayer) {
				if (autoRevertSelectedItem) {
					if (player.itemTime == 0 && player.itemAnimation == 0) {
						player.selectedItem = originalSelectedItem;
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
			OrchidModShamanHelper.shamanPostUpdateEquips(player, this, mod);
			OrchidModAlchemistHelper.alchemistPostUpdateEquips(player, this, mod);
			OrchidModGamblerHelper.gamblerPostUpdateEquips(player, this, mod);
			OrchidModDancerHelper.dancerPostUpdateEquips(player, this, mod);
			
			if (thoriumMod != null) {
                //int thoriumCrit = player.GetModPlayer<ThoriumPlayer>().allCrit; // Impossible : can't add [using ThoriumMod;] because I don't have the ThoriumMod.dll file
				
				ModPlayer thoriumPlayer = player.GetModPlayer(this.thoriumMod, "ThoriumPlayer");
				FieldInfo field = thoriumPlayer.GetType().GetField("allCrit", BindingFlags.Public | BindingFlags.Instance);
				if (field != null) {
					int thoriumCrit = (int)field.GetValue(thoriumPlayer);
					this.customCrit += thoriumCrit;
				}
            }
     
			this.shamanCrit += this.customCrit;
			this.alchemistCrit += this.customCrit;
			this.gamblerCrit += this.customCrit;
			this.dancerCrit += this.customCrit;
			
			this.CheckWoodBreak(player);
		}
		
		public override void PostUpdate() {
			OrchidModShamanHelper.postUpdateShaman(player, this, mod);
			OrchidModGamblerHelper.postUpdateGambler(player, this, mod);
			OrchidModAlchemistHelper.postUpdateAlchemist(player, this, mod);
		}
		
		public override void OnHitNPCWithProj(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.shamanProjectile) {
				OrchidModShamanHelper.OnHitNPCWithProjShaman(projectile, target, damage, knockback, crit, player, this, mod);
			}
		}
		
		public override void ModifyHitNPCWithProj(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if	(modProjectile.alchemistProjectile) {
				if (Main.rand.Next(101) <= this.alchemistCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;
			}
			
			if	(modProjectile.gamblerProjectile) {
				if (Main.rand.Next(101) <= this.gamblerCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;
				
				OrchidModGamblerHelper.ModifyHitNPCWithProjGambler(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection, player, this, mod);
			}
			
			if	(modProjectile.shamanProjectile) {
				if (Main.rand.Next(101) <= this.shamanCrit + modProjectile.baseCritChance) 
					crit = true;
				else crit = false;
				
				OrchidModShamanHelper.ModifyHitNPCWithProjShaman(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection, player, this, mod);
			}
        }
		
		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			OrchidModShamanHelper.DrawEffectsShaman(drawInfo, ref r, ref g, ref b, ref a, ref fullBright, player, this, mod);
		}
		
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
		ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			bool toReturn = true;
			
			if (!OrchidModShamanHelper.PreHurtShaman(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, player, this, mod)) {
				toReturn = false;
			}
			
			if (!OrchidModDancerHelper.PreHurtDancer(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, player, this, mod)) {
				toReturn = false;
			}
			return toReturn;
		}
		
		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (OrchidMod.AlchemistCatalystHotKey.JustPressed && player.itemAnimation == 0) {
				for (int i = 0; i < Main.maxInventory; i++) {
					Item item = Main.LocalPlayer.inventory[i];
					if (item.type != 0) {
						OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
						if (orchidItem.alchemistCatalyst) {
							this.originalSelectedItem = player.selectedItem;
							this.autoRevertSelectedItem = true;
							player.selectedItem = i;
							player.controlUseItem = true;
							player.ItemCheck(player.whoAmI);
							return;
						}
					}
				}
			}
			
			if (OrchidMod.ShamanBondHotKey.JustPressed) {
				OrchidModShamanHelper.BondHotKeyPressed(player, this, mod);
			}
			
			if (OrchidMod.AlchemistReactionHotKey.JustPressed) {
				if (this.alchemistNbElements < 2 || player.FindBuffIndex(mod.BuffType("ReactionCooldown")) > -1) {
					return;
				} else {
					AlchemistHiddenReactionHelper.triggerAlchemistReaction(mod, player, this);
				}
			}
		}
		
		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit) {
			OrchidModDancerHelper.ModifyHitByNPCDancer(npc, ref damage, ref crit, player, this, mod);
		}
		
		public override void ResetEffects() {
			customCrit = 0;
			
			generalTools = false;
			generalStatic = false;

			OrchidModDancerHelper.ResetEffectsDancer(player, this, mod);
			OrchidModAlchemistHelper.ResetEffectsAlchemist(player, this, mod);
			OrchidModGamblerHelper.ResetEffectsGambler(player, this, mod);
			OrchidModShamanHelper.ResetEffectsShaman(player, this, mod);
		}
		
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			OrchidModAlchemistHelper.onRespawnAlchemist(player, this, mod);
			OrchidModShamanHelper.onRespawnShaman(player, this, mod);
			OrchidModGamblerHelper.onRespawnGambler(player, this);
		}
		
		public override void OnRespawn(Player player) {
			OrchidModAlchemistHelper.onRespawnAlchemist(player, this, mod);
			OrchidModShamanHelper.onRespawnShaman(player, this, mod);
			OrchidModGamblerHelper.onRespawnGambler(player, this);
		}
		
		public void updateBuffEffects() {	
			if (player.FindBuffIndex(26) > -1) { // WELL FED
				this.customCrit += 2;
			}
			
			if (player.FindBuffIndex(115) > -1) { // RAGE
				this.customCrit += 10;
			}
		}
		
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (player.HasBuff(mod.BuffType("EarthTotemPlus")) && !(player.HasBuff(mod.BuffType("EarthTotemRevive")))) {
				Main.PlaySound(SoundID.Item29, player.position);
				player.AddBuff(mod.BuffType("EarthTotemRevive"), 60 * 60 * 5);
				for (int i = 0 ; i < 15 ; i ++) {
					int randX = Main.rand.Next(150);
					int randY = Main.rand.Next(100);
					int dust = Dust.NewDust(new Vector2((int)(player.Center.X + 75 - randX), (int)(player.Center.Y  + 15 - randY)), 0, 0, 64, 0f, 0f, 0, default(Color), 1.3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 3f;
				}
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(200, true);
				player.statLife = 200;	
				return false;
			}
			return true;
		}

		public void updateItemEffects() {
			if (player.armor[1].type == 374) this.customCrit += 3;// COBALT BREASPLATE
			if (player.armor[1].type == 1208) this.customCrit += 2; // PALLADIUM BREASTPLATE
			if (player.armor[2].type == 1209) this.customCrit += 1; // PALLADIUM LEGS
			if (player.armor[2].type == 380) this.customCrit += 3; // MYTHRIL LEGS
			if (player.armor[1].type == 1213) this.customCrit += 6; // ORICHALCUM BREASPLATE
			if (player.armor[2].type == 404) this.customCrit += 4; // ADAMANTITE LEGS
			if (player.armor[1].type == 1218) this.customCrit += 3; // TITANIUM BREASTPLATE
			if (player.armor[2].type == 1219) this.customCrit += 3; // TITANIUM LEGS
			if (player.armor[2].type == 2277) this.customCrit += 5; // GI
			if (player.armor[1].type == 551) this.customCrit += 7; // HALLOWED BREASPLATE
			if (player.armor[1].type == 1004) this.customCrit += 7; // CHLOROPHITE BREASTPLATE
			if (player.armor[2].type == 1005) this.customCrit += 8; // CHLOROPHITE LEGS
			
			for (int k = 3; k < 8 + player.extraAccessorySlots; k++) {
				if (player.armor[k].type == 1301) this.customCrit += 8; // DESTROYER EMBLEM
				if (player.armor[k].type == 1248) this.customCrit += 10; // EYE OF THE GOLEM
				if (player.armor[k].type == 3015) this.customCrit += 5; // PUTRID SCENT
				if (player.armor[k].type == 3110) this.customCrit += 2; // CELESTIAL SHELL
				if (player.armor[k].type == 1865) this.customCrit += 2; // CELESTIAL STONE
				if (player.armor[k].type == 899 && Main.dayTime) this.customCrit += 2; // CELESTIAL STONE
				if (player.armor[k].type == 900 && (!Main.dayTime || Main.eclipse)) this.customCrit += 2; // CELESTIAL STONE
				
				if (player.armor[k].prefix == PrefixID.Lucky) this.customCrit += 4;
				if (player.armor[k].prefix == PrefixID.Precise) this.customCrit += 2;
			}	
		}
		
		public void generalPostUpdateEquips() {
			if (generalStaticTimer == 299) {
				Main.PlaySound(SoundID.Item93, player.position);
				for (int i = 0 ; i < 10 ; i ++) {
					int dust = Dust.NewDust(player.position, player.width, player.height, 60);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= 1.5f;
				}
			}
			if ((player.velocity.X != 0f || player.velocity.Y != 0f) && generalStaticTimer >= 300) {
				player.AddBuff(BuffType<Buffs.StaticQuartArmorBuff>(), 60 * 10);
				for (int i = 0 ; i < 10 ; i ++) {
					int dust = Dust.NewDust(player.position, player.width, player.height, 60);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= 1.5f;
				}
			}
			generalStaticTimer = (generalStatic && player.velocity.X == 0f && player.velocity.Y == 0f) ? generalStaticTimer < 300 ? generalStaticTimer + 1 : 300 : 0;
		}
		
		public void CheckWoodBreak(Player player) { // From Vanilla Source
			if (player.velocity.Y <= 1f || this.generalTools)
				return;
			Vector2 vector2 = player.position + player.velocity;
			int num1 = (int) (vector2.X / 16.0);
			int num2 = (int) ((vector2.X + (double) player.width) / 16.0);
			int num3 = (int) ((player.position.Y + (double) player.height + 1.0) / 16.0);
			for (int i = num1; i <= num2; ++i) {
				for (int j = num3; j <= num3 + 1; ++j) {
					if (Main.tile[i, j].nactive() && (int) Main.tile[i, j].type == TileType<Tiles.Ambient.FragileWood>() && !WorldGen.SolidTile(i, j - 1)) {
						WorldGen.KillTile(i, j, false, false, false);
						// if (Main.netMode == 1)
							// NetMessage.SendData(17, -1, -1, (NetworkText) null, 0, (float) i, (float) j, 0.0f, 0, 0, 0);
					}
				}
			}
		}
		public override void clientClone(ModPlayer clientClone) {
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
			
			clone.shamanFireBuff = this.shamanFireBuff;
			clone.shamanWaterBuff = this.shamanWaterBuff;
			clone.shamanAirBuff = this.shamanAirBuff;
			clone.shamanEarthBuff = this.shamanEarthBuff;
			clone.shamanSpiritBuff = this.shamanSpiritBuff;
			
			clone.gamblerHasCardInDeck = this.gamblerHasCardInDeck;
		}
		
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)OrchidModMessageType.ORCHIDPLAYERSYNCPLAYER);
			packet.Write((byte)player.whoAmI);
			
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
			
			packet.Write(shamanFireBuff);
			packet.Write(shamanWaterBuff);
			packet.Write(shamanAirBuff);
			packet.Write(shamanEarthBuff);
			packet.Write(shamanSpiritBuff);
			
			packet.Write(shamanFireTimer);
			packet.Write(shamanWaterTimer);
			packet.Write(shamanAirTimer);
			packet.Write(shamanEarthTimer);
			packet.Write(shamanSpiritTimer);
			
			packet.Write(gamblerHasCardInDeck);

			packet.Send(toWho, fromWho);
		}
		
		public override void SendClientChanges(ModPlayer clientPlayer) {
			OrchidModPlayer clone = clientPlayer as OrchidModPlayer;
			
			//Orb Types
			if (clone.shamanOrbSmall != shamanOrbSmall) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)shamanOrbSmall);
				packet.Send();
			}
			
			if (clone.shamanOrbBig != shamanOrbBig) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDBIG);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)shamanOrbBig);
				packet.Send();
			}
			
			if (clone.shamanOrbLarge != shamanOrbLarge) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)shamanOrbLarge);
				packet.Send();
			}
			
			if (clone.shamanOrbUnique != shamanOrbUnique) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)shamanOrbUnique);
				packet.Send();
			}
			
			if (clone.shamanOrbCircle != shamanOrbCircle) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)shamanOrbCircle);
				packet.Send();
			}
			
			// Orb Counts
			if (clone.orbCountSmall != orbCountSmall) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL);
				packet.Write((byte)player.whoAmI);
				packet.Write(orbCountSmall);
				packet.Send();
			}
			
			if (clone.orbCountBig != orbCountBig) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG);
				packet.Write((byte)player.whoAmI);
				packet.Write(orbCountBig);
				packet.Send();
			}
			
			if (clone.orbCountLarge != orbCountLarge) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE);
				packet.Write((byte)player.whoAmI);
				packet.Write(orbCountLarge);
				packet.Send();
			}
			
			if (clone.orbCountUnique != orbCountUnique) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE);
				packet.Write((byte)player.whoAmI);
				packet.Write(orbCountUnique);
				packet.Send();
			}
			
			if (clone.orbCountCircle != orbCountCircle) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE);
				packet.Write((byte)player.whoAmI);
				packet.Write(orbCountCircle);
				packet.Send();
			}
			
			// Empowerment Levels
			if (clone.shamanFireBuff != shamanFireBuff) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDATTACK);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanFireBuff);
				packet.Send();
			}
			
			if (clone.shamanWaterBuff != shamanWaterBuff) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDARMOR);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanWaterBuff);
				packet.Send();
			}
			
			if (clone.shamanAirBuff != shamanAirBuff) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDCRITICAL);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanAirBuff);
				packet.Send();
			}
			
			if (clone.shamanEarthBuff != shamanEarthBuff) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDREGENERATION);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanEarthBuff);
				packet.Send();
			}
			
			if (clone.shamanSpiritBuff != shamanSpiritBuff) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDSPEED);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanSpiritBuff);
				packet.Send();
			}
			
			//Empowerment Timers
			if (clone.shamanFireTimer != shamanFireTimer) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanFireTimer);
				packet.Send();
			}
			
			if (clone.shamanWaterTimer != shamanWaterTimer) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanWaterTimer);
				packet.Send();
			}
			
			if (clone.shamanAirTimer != shamanAirTimer) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanAirTimer);
				packet.Send();
			}
			
			if (clone.shamanEarthTimer != shamanEarthTimer) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanEarthTimer);
				packet.Send();
			}
			
			if (clone.shamanSpiritTimer != shamanSpiritTimer) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED);
				packet.Write((byte)player.whoAmI);
				packet.Write(shamanSpiritTimer);
				packet.Send();
			}
			
			if (clone.gamblerHasCardInDeck != gamblerHasCardInDeck) {
				var packet = mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
				packet.Write((byte)player.whoAmI);
				packet.Write(gamblerHasCardInDeck);
				packet.Send();
			}
		}
	}
}
