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
		public int gamblerCrit = 0;
		public Item[] gamblerCardsItem = new Item[20];
		public Item[] gamblerCardNext = new Item[3];
		public Item gamblerCardCurrent = new Item();
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
		
		public float alchemistDamage = 1.0f;
		public float alchemistVelocity = 1.0f;
		public int alchemistCrit = 0;
		public bool[] alchemistElements = new bool[6]; // mp Sync
		public int[] alchemistFlasks = new int[6]; // mp Sync
		public int[] alchemistDusts = new int[6]; // mp Sync
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
		
		public bool alchemistMeteor = false;
		public bool alchemistFlowerSet	= false;
		public bool alchemistMushroomSpores	= false;
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
		public int shamanTimerHellDamage = 0;
		public int shamanTimerHellDefense = 0;
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
			this.clearGamblerCards();
			this.onRespawnAlchemist();
			this.onRespawnShaman();
			this.onRespawnGambler();
        }
		
		public override TagCompound Save()
		{
			return new TagCompound
			{
				["GamblerCardsItem"] = gamblerCardsItem.Select(ItemIO.Save).ToList(),
				["ChemistHint"] = alchemistChemistFirstInteraction
			};
		}
		
		public override void Load(TagCompound tag)
		{
			gamblerCardsItem = tag.GetList<TagCompound>("GamblerCardsItem").Select(ItemIO.Load).ToArray();
			alchemistChemistFirstInteraction = tag.GetBool("ChemistHint");
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
			
			this.shamanPostUpdateEquips();
			this.alchemistPostUpdateEquips();
			this.gamblerPostUpdateEquips();
			this.dancerPostUpdateEquips();
			
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
		}
		
		public override void PostUpdate() {
			this.postUpdateShaman();
			this.postUpdateGambler();
			this.postUpdateAlchemist();
		}
		
		public override void OnHitNPCWithProj(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if	(modProjectile.shamanProjectile) {
				this.shamanTimerCombat = 120;
				
				if (shamanCrimtane && shamanEarthBuff != 0 && this.shamanTimerCrimson == 30) {
					this.shamanTimerCrimson = 0;
					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(2, true);
					player.statLife += 2;	
				}
				
				if (this.shamanVampire && Main.rand.Next(5) == 0 && this.shamanTimerViscount == 180) {
					this.shamanTimerViscount = 0;
					Vector2 perturbedSpeed = new Vector2(0f, - 5f).RotatedByRandom(MathHelper.ToRadians(40));
					if (Main.rand.Next(2) == 0) {
						Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("ViscountBlood"), 0, 0.0f, projectile.owner, 0.0f, 0.0f);
					} else {
						Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("ViscountSound"), 0, 0.0f, projectile.owner, 0.0f, 0.0f);
					}
				}
				
				if (this.shamanHell && this.shamanTimerHellDamage == 600 && this.shamanFireTimer > 0) {
					this.shamanTimerHellDamage = 0;
					for (int i = 0 ; i < 10 ; i ++) {
						Vector2 perturbedSpeed = new Vector2(0f, - 5f).RotatedByRandom(MathHelper.ToRadians(360));
						int dmg = (int)(50 * this.shamanDamage);
						Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("ShamanHellHoming"), dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
					}
				}
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
				
				if (this.gamblerDiceID == 2 && this.gamblerTimerHoney == 30) {
					player.HealEffect(this.gamblerDiceValue, true);
					player.statLife += this.gamblerDiceValue;
					this.gamblerTimerHoney = 0;
				}
			}
			
			if	(modProjectile.shamanProjectile) {
				if (Main.rand.Next(101) <= this.shamanCrit + modProjectile.baseCritChance) 
					crit = true;
				else crit = false;
				
				if (crit) {
					if (player.FindBuffIndex(mod.BuffType("OpalEmpowerment")) > -1) {
						damage += 5;
					}
					
					if (player.FindBuffIndex(mod.BuffType("DestroyerFrenzy")) > -1) {
						damage = (int)(damage * 1.15f);
					}
					
					if (projectile.type == mod.ProjectileType("TitanicScepterProj")) {
						damage = (int)(damage * 1.5f);
					}
					
					if (player.FindBuffIndex(mod.BuffType("TitanicBuff")) > -1) {
						damage = (int)(damage * 1.2f);
					}
					
					if (this.shamanDestroyer && this.shamanWaterTimer > 0) {
						this.shamanTimerDestroyer = 60;
						this.shamanDestroyerCount ++;
					}
				}
			
				if (target.type != NPCID.TargetDummy) {
					if (projectile.type != mod.ProjectileType("LavaDroplet")
						&& projectile.type != mod.ProjectileType("LostSoul")
						&& projectile.type != mod.ProjectileType("HarpyAnkletProj")
						&& projectile.type != mod.ProjectileType("DeepForestCharmProj")
						&& projectile.type != mod.ProjectileType("Smite")
						){
						if (this.shamanFireBuff > 0) {
							if (this.shamanPoison) target.AddBuff((20), 5 * 60);
							if (this.shamanVenom) target.AddBuff((70), 5 * 60);
							if (this.shamanFire) target.AddBuff((24), 5 * 60);
							if (this.shamanIce) target.AddBuff((44), 5 * 60);
							if (this.shamanDemonite) target.AddBuff(153, 20); // Shadowflame
						}
						
						if (crit == true && shamanSkull && shamanWaterBuff != 0) {
							
							int dmg = (int)(80 * shamanDamage + 5E-06f);
							Vector2 mouseTarget = Main.screenPosition + new Vector2((float)Main.mouseX - 8, (float)Main.mouseY);
							Vector2 heading = mouseTarget - Main.player[projectile.owner].position;
							heading.Normalize();
							heading *= new Vector2(5f, 5f).Length();
							Vector2 projectileVelocity = ( new Vector2(heading.X, heading.Y).RotatedByRandom(MathHelper.ToRadians(10)));
							Projectile.NewProjectile(Main.player[projectile.owner].Center.X, Main.player[projectile.owner].Center.Y, projectileVelocity.X, projectileVelocity.Y, mod.ProjectileType("LostSoul"), dmg, 0f, projectile.owner, 0f, 0f);
						}
						
						if (crit == true && shamanWaterHoney && shamanWaterBuff != 0 && this.timerVial == 30) {
							this.timerVial = 0;
							player.HealEffect(3, true);
							player.statLife += 3;
						}
						
						if (Main.rand.Next(15) == 0 && this.shamanDownpour && this.getNbShamanicBonds() > 2) {
							int dmg = (int)(50 * shamanDamage + 5E-06f);
							target.StrikeNPCNoInteraction(dmg, 0f, 0);
							Main.PlaySound(2, (int)target.Center.X ,(int)target.Center.Y - 200, 93);
							
							for(int i=0; i < 15; i++)
							{
								int dust = Dust.NewDust(target.position, target.width, target.height, 226);
								Main.dust[dust].noGravity = true;
								Main.dust[dust].velocity *= 2f;
							}
						}
					}
				}
			}
        }
		
		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (shamanShadowEmpowerment)
			{
				if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 27, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
			}
			
			if (abyssalWings && player.controlJump) {
				if (Main.rand.Next(6) == 0 && drawInfo.shadow == 0f && player.wingTime > 0)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(15f, 2f), player.width + 30, player.height + 4, DustType<AbyssalDust>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.dust[dust].scale = 1.75f;
					Main.playerDrawDust.Add(dust);
				}
				if (Main.rand.Next(6) == 0 && drawInfo.shadow == 0f && player.wingTime > 0)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(15f, 2f), player.width + 30, player.height + 4, DustType<AbyssalDustBright>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.dust[dust].scale = 1.75f;
					Main.playerDrawDust.Add(dust);
				}
			}
		}
		
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
		ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (this.shamanMourningTorch) {
				shamanFireTimer -= 5 * 60;
				shamanWaterTimer -= 5 * 60;
				shamanAirTimer -= 5 * 60;
				shamanEarthTimer -= 5 * 60;
				shamanSpiritTimer -= 5 * 60;
			}
			
			if (this.shamanSunBelt) {
				player.AddBuff((mod.BuffType("BrokenPower")), 60 * 15);
			}
			
			if (this.shamanHell && this.shamanTimerHellDefense == 300 && this.shamanWaterTimer > 0) {
				this.shamanTimerHellDefense = 0;
				int dmg = (int)(50 * this.shamanDamage);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("ShamanHellExplosion"), dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 14);
			}
			
			if (this.shamanDiabolist && this.shamanEarthTimer > 0) {
				this.shamanTimerDiabolist = 60;
				this.shamanDiabolistCount += damage;
			}
			
			if (this.dancerInvincibility > 0) {
				return false;
			}
			
			return true;
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
				if (this.shamanFireBondLoading >= 100) {
					int level = (this.shamanFireBuff + this.shamanFireBonus);
					int dmg = (int)(10 * level * this.shamanDamage + 5E-06f);
					float spd = 4f + (float)(level / 3);
					int proj = mod.ProjectileType("FireBondProj1");
					if (level >= 3) proj = mod.ProjectileType("FireBondProj2");
					if (level >= 5) proj = mod.ProjectileType("FireBondProj3");
					if (level >= 8) proj = mod.ProjectileType("FireBondProj4");
					if (level >= 12) proj = mod.ProjectileType("FireBondProj5");
					Vector2 mouseTarget = Main.screenPosition + new Vector2((float)Main.mouseX - 8, (float)Main.mouseY);
					Vector2 heading = mouseTarget - player.position;
					heading.Normalize();
					heading *= new Vector2(spd, spd).Length();
					//Vector2 vel = ( new Vector2(heading.X, heading.Y).RotatedByRandom(MathHelper.ToRadians(10)));
					Vector2 vel = (new Vector2(heading.X, heading.Y));
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, dmg, 0f, player.whoAmI);
					this.shamanFireBondLoading = 0;
					Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				}
				
				if (this.shamanWaterBondLoading >= 100) {
					int level = (this.shamanWaterBuff + this.shamanWaterBonus);
					int dmg = (int)(5 * level * this.shamanDamage + 5E-06f);
					int nb = 3;
					int proj = mod.ProjectileType("WaterBondProj");
					int rotation = -25;
					int ai = 0;
					
					if (level >= 3) {
						nb ++;
						ai ++;
					}
					
					if (level >= 5) {
						nb ++;
						ai ++;
					}
					
					if (level >= 8) {
						nb ++;
						ai ++;
					}
					
					if (level >= 12) {
						nb ++;
						ai ++;
					}
					
					Vector2 heading = new Vector2(player.position.X, player.position.Y - 10f);
					heading = heading - player.position;
					heading.Normalize();
					heading *= new Vector2(-5f, -5f).Length();
					int rotationVal = (int)(50 / nb);
					while (nb > 0) {
						Vector2 vel = (new Vector2(heading.X, heading.Y).RotatedBy(MathHelper.ToRadians(rotation)));
						int waterProj = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, proj, dmg, 0f, player.whoAmI);
						Main.projectile[waterProj].ai[0] = ai;
						Main.projectile[waterProj].netUpdate = true;
						rotation += rotationVal;
						nb --;
					}
					this.shamanWaterBondLoading = 0;
					Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				}
				
				if (this.shamanAirBondLoading >= 100) {
					int level = (this.shamanAirBuff + this.shamanAirBonus);
					int dmg = (int)(10 * level * this.shamanDamage + 5E-06f);
					int proj = mod.ProjectileType("WindBondProj1");
					int ai = 0;
					
					if (level >= 3) {
						ai ++;
					}
					
					if (level >= 5) {
						ai ++;
					}
					
					if (level >= 8) {
						ai ++;
					}
					
					if (level >= 12) {
						ai ++;
					}
					
					int airProj = Projectile.NewProjectile(player.Center.X, player.position.Y - 300, 0f, 1f, proj, 0, 0.0f, player.whoAmI);
					Main.projectile[airProj].ai[1] = ai;
					Main.projectile[airProj].netUpdate = true;
					this.shamanAirBondLoading = 0;
					Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				}
				
				if (this.shamanEarthBondLoading >= 100) {
					int level = (this.shamanEarthBuff + this.shamanEarthBonus);
					
					if (level >= 3 && level < 8) {
						float posX = player.position.X;
						float posY = player.position.Y;
						
						for (int k = 0; k < 255; k++) {
							Player targetPlayer = Main.player[k];
							if (MathHelper.Distance(posX, targetPlayer.position.X) < 100f && MathHelper.Distance(posY, targetPlayer.position.Y) < 100f) {
								targetPlayer.AddBuff((mod.BuffType("EarthDefense")), 60 * 10);
							}
						}
					}
					
					if (level >= 8) {
						float posX = player.position.X;
						float posY = player.position.Y;
						
						for (int k = 0; k < 255; k++) {
							Player targetPlayer = Main.player[k];
							if (MathHelper.Distance(posX, targetPlayer.position.X) < 100f && MathHelper.Distance(posY, targetPlayer.position.Y) < 100f) {
								targetPlayer.AddBuff((mod.BuffType("EarthRegeneration")), 60 * 10);
							}
						}
					}
					
					if (level >= 5) {
						int projTotem = mod.ProjectileType("EarthBondProj");
						
						for (int l = 0; l < Main.projectile.Length; l++)
						{  
							Projectile proj = Main.projectile[l];
							if (proj.active && proj.type == projTotem && proj.owner == player.whoAmI)
							{
								proj.active = false;
							}
						}
						
						int earthProj = Projectile.NewProjectile(player.Center.X, player.position.Y, 0f, 1f, projTotem, 0, 0.0f, player.whoAmI);
						
						 
						if (level >= 12) {
							Main.projectile[earthProj].ai[1] = 1;
						} else {
							Main.projectile[earthProj].ai[1] = 0;
						}
						Main.projectile[earthProj].netUpdate = true;
					}
					
					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(15 * level, true);
					player.statLife += 10 * level;	
					
					this.shamanEarthBondLoading = 0;
					Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				}
				
				if (this.shamanSpiritBondLoading >= 100) {
					int level = (this.shamanSpiritBuff + this.shamanSpiritBonus);
					
					if (level >= 3 && level < 8) {
						float posX = player.position.X;
						float posY = player.position.Y;
						
						for (int k = 0; k < 255; k++) {
							Player targetPlayer = Main.player[k];
							if (MathHelper.Distance(posX, targetPlayer.position.X) < 100f && MathHelper.Distance(posY, targetPlayer.position.Y) < 100f) {
								targetPlayer.AddBuff((mod.BuffType("SpiritAttack")), 60 * 10);
							}
						}
					}
					
					if (level >= 8) {
						float posX = player.position.X;
						float posY = player.position.Y;
						
						for (int k = 0; k < 255; k++) {
							Player targetPlayer = Main.player[k];
							if (MathHelper.Distance(posX, targetPlayer.position.X) < 100f && MathHelper.Distance(posY, targetPlayer.position.Y) < 100f) {
								targetPlayer.AddBuff((mod.BuffType("SpiritRegeneration")), 60 * 10);
							}
						}
					}
					
					if (level >= 5) {
						int projTotem = mod.ProjectileType("SpiritBondProj1");
						
						for (int l = 0; l < Main.projectile.Length; l++)
						{  
							Projectile proj = Main.projectile[l];
							if (proj.active && proj.type == projTotem && proj.owner == player.whoAmI)
							{
								proj.active = false;
							}
						}
						
						int earthProj = Projectile.NewProjectile(player.Center.X, player.position.Y, 0f, -1f, projTotem, 0, 0.0f, player.whoAmI);
						
						 
						if (level >= 12) {
							Main.projectile[earthProj].ai[1] = 1;
						} else {
							Main.projectile[earthProj].ai[1] = 0;
						}
						Main.projectile[earthProj].netUpdate = true;
					}
					
					int maxBufftimer = 60 * this.shamanBuffTimer;
					int toAdd = 90 + (30 * level);
					this.shamanFireTimer = this.shamanFireBuff == 0 ? 0 : this.shamanFireTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanFireTimer + toAdd;
					this.shamanWaterTimer = this.shamanWaterBuff == 0 ? 0 : this.shamanWaterTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanWaterTimer + toAdd;
					this.shamanAirTimer = this.shamanAirBuff == 0 ? 0 : this.shamanAirTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanAirTimer + toAdd;
					this.shamanEarthTimer = this.shamanEarthBuff == 0 ? 0 : this.shamanEarthTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanEarthTimer + toAdd;
					
					this.shamanSpiritBondLoading = 0;
					Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 45);
				}
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
			if (this.dancerWeaponType != OrchidModDancerItemType.NULL) {
				if (!(npc.boss || npc.type == NPCID.TargetDummy) && npc.knockBackResist > 0f) {
					npc.velocity.X = player.velocity.X * this.dancerWeaponKnockback * npc.knockBackResist / 5;
					npc.velocity.Y = player.velocity.Y * this.dancerWeaponKnockback * npc.knockBackResist / 5;
				}
				npc.StrikeNPCNoInteraction(this.dancerWeaponDamage, 0f, 0);
				
				switch (this.dancerWeaponType) {
					case OrchidModDancerItemType.IMPACT:
						player.velocity = this.dancerVelocity * -0.5f;
						this.clearDancerEffects();
						break;
					case OrchidModDancerItemType.PHASE:
						break;
					case OrchidModDancerItemType.MOMENTUM:
						player.velocity = this.dancerVelocity * -1f;
						break;
					default:
						break;
				}
				
				damage = 0;
				this.dancerInvincibility = 10;
			}
		}
		
		public override void ResetEffects()
		{
			if (this.getNbAlchemistFlasks() == 0) {
				clearAlchemistFlasks();
				clearAlchemistElements();
				clearAlchemistDusts();
				clearAlchemistColors();
			}
			
			customCrit = 0;
			
			dancerDamage = 1.0f;
			dancerCrit = 0;
			dancerPoiseMax = 20;
			dancerPoiseConsume = 0;
			dancerInvincibility -= this.dancerInvincibility > 0 ? 1 : 0;
			
			if (this.dancerDashTimer > 0) {
				dancerDashTimer --;
				if (this.dancerDashTimer == 0) {
					player.velocity *= 0f;
					this.clearDancerEffects();
				}
			}
			
			alchemistPotencyMax = 8;
			alchemistRegenPotency = 60;
			alchemistNbElementsMax = 2;
			alchemistCrit = 0;
			alchemistDamage = 1.0f;
			alchemistVelocity = 1.0f;
			alchemistSelectUIDisplay = alchemistSelectUIItem ? alchemistSelectUIDisplay : false;
			alchemistSelectUIItem = false;
			
			alchemistMeteor = false;
			alchemistFlowerSet = false;
			alchemistMushroomSpores = false;
			
			gamblerDamage = 1.0f;
			gamblerCrit = 0;
			gamblerChipsMax = 5;
			gamblerChipsConsume = 0;
			gamblerSeeCards = 0;
			gamblerRedrawsMax = 0;
			gamblerRedrawCooldownMax = 1800;
			gamblerShuffleCooldownMax = 600;
			gamblerAttackInHand = false;
			gamblerHasCardInDeck = this.gamblerCardsItem[0].type != 0;
			
			gamblerDungeon = false;
			gamblerLuckySprout = false;
			gamblerPennant = false;
			gamblerVulture = false;
			gamblerSlimyLollipop = false;
			
			shamanCrit = 0;
			shamanDamage = 1.0f;
			shamanBuffTimer = 5;
			hauntedCandle = false;
			spawnedGhost = false;
			doubleJumpHarpy = false;
			abyssalWings = false;
			abyssSet = false;
			
			shamanFireBonus = 0;
			shamanWaterBonus = 0;
			shamanAirBonus = 0;
			shamanEarthBonus = 0;
			shamanSpiritBonus = 0;
			
			shamanFire = false;
			shamanIce = false;
			shamanPoison = false;
			shamanVenom = false;
			shamanHoney = false;
			shamanFeather = false;
			shamanVampire = false;
			shamanDestroyer = false;
			shamanDiabolist = false;
			shamanDripping = false;
			shamanAmber = false;
			shamanDryad = false;
			shamanForest = false;
			shamanHeavy = false;
			shamanSkull = false;
			shamanWaterHoney = false;
			shamanSmite = false;
			shamanCrimtane = false;
			shamanDemonite = false;
			shamanDownpour = false;
			shamanHell = false;
			shamanHarpyAnklet = false;
			shamanWyvern = false;
			shamanRage = false;
			
			shamanShadowEmpowerment = false;
			shamanMourningTorch = false;
			shamanSunBelt = false;
			
			shamanHitDelay -= shamanHitDelay > 0 && this.timer120 % 5 == 0 ? 1 : 0;
		}
		
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			this.onRespawnAlchemist();
			this.onRespawnShaman();
			this.onRespawnGambler();
		}
		
		public override void OnRespawn(Player player) {
			this.onRespawnAlchemist();
			this.onRespawnShaman();
			this.onRespawnGambler();
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
		
		public void dancerPostUpdateEquips() {
			if (this.dancerVelocity.X != 0f || this.dancerVelocity.Y != 0f) {
				player.velocity *= 0f;
				
				int Height = !player.onTrack ? player.height : player.height - 20;
				this.dancerVelocity = Collision.TileCollision(player.position, this.dancerVelocity, player.width, Height, true, false, (int) player.gravDir);
				
				player.position.X += this.dancerVelocity.X;
				player.position.Y += this.dancerVelocity.Y;
				// player.position = Vector2.op_Addition(player.position, currentVelocity);
			}
		}
		
		public void alchemistPostUpdateEquips() {
			int regenComparator = this.alchemistPotencyWait <= 120 ? this.alchemistPotencyWait <= 0 ? (int)(this.alchemistRegenPotency / 3) : (int)(this.alchemistRegenPotency / 2) : this.alchemistRegenPotency;
			if (this.alchemistPotency < this.alchemistPotencyMax && ((this.alchemistRegenPotency > 0) ? this.timer120 % regenComparator == 0 : true)) {
				this.alchemistPotencyDisplayTimer = 180;
				this.alchemistPotency ++;
				this.alchemistFlowerTimer = this.alchemistFlowerSet ? 600 : 0;
			} else {
				if (this.alchemistPotency > this.alchemistPotencyMax) {
					this.alchemistPotency = this.alchemistPotencyMax;
				}
				this.alchemistPotencyDisplayTimer --;
			}
			
			this.alchemistResetTimer = (this.alchemistPotencyDisplayTimer > 0) ? 300 : this.alchemistResetTimer - 1;
			this.alchemistResetTimer = (this.alchemistResetTimer == -1) ? 0 : this.alchemistResetTimer;
			this.alchemistPotencyWait -= this.alchemistPotencyWait > 0 ? 1 : 0;
			
			if (this.alchemistResetTimer == 1) {
				this.alchemistFlaskDamage = 0;
				this.alchemistNbElements = 0;
				this.clearAlchemistElements();
				this.clearAlchemistFlasks();
				this.clearAlchemistDusts();
				this.clearAlchemistColors();
			}
			
			if (this.alchemistColorRDisplay != this.alchemistColorR) {
				bool superior = this.alchemistColorRDisplay > this.alchemistColorR;
				int abs = Math.Abs(this.alchemistColorRDisplay - this.alchemistColorR);
				int val = (int)(abs / 10);
				if (abs > val) {
					this.alchemistColorRDisplay += superior ? -val : val;
				}
				this.alchemistColorRDisplay += superior ? -1 : 1;
			}
			
			if (this.alchemistColorGDisplay != this.alchemistColorG) {
				bool superior = this.alchemistColorGDisplay > this.alchemistColorG;
				int abs = Math.Abs(this.alchemistColorGDisplay - this.alchemistColorG);
				int val = (int)(abs / 10);
				if (abs > val) {
					this.alchemistColorGDisplay += superior ? -val : val;
				}
				this.alchemistColorGDisplay += superior ? -1 : 1;
			}
			
			if (this.alchemistColorBDisplay != this.alchemistColorB) {
				bool superior = this.alchemistColorBDisplay > this.alchemistColorB;
				int abs = Math.Abs(this.alchemistColorBDisplay - this.alchemistColorB);
				int val = (int)(abs / 10);
				if (abs > val) {
					this.alchemistColorBDisplay += superior ? -val : val;
				}
				this.alchemistColorBDisplay += superior ? -1 : 1;
			}
		}
		
		public void gamblerPostUpdateEquips() {
			if (this.gamblerRedrawsMax > 0) {
				this.gamblerRedrawCooldown = this.gamblerRedraws >= this.gamblerRedrawsMax ? this.gamblerRedrawCooldownMax : this.gamblerRedrawCooldown;
				this.gamblerRedrawCooldown = this.gamblerRedrawCooldown > this.gamblerRedrawCooldownMax ? this.gamblerRedrawCooldownMax : this.gamblerRedrawCooldown;
				this.gamblerRedrawCooldown = this.gamblerRedrawCooldown <= 0 ? this.gamblerRedrawCooldownMax : this.gamblerRedrawCooldown - 1;
				if (this.gamblerRedrawCooldown <= 0 && this.gamblerRedraws < this.gamblerRedrawsMax) {
					this.gamblerRedraws ++;
				}
			} else {
				this.gamblerRedrawCooldown = -1;
			}
			
			this.gamblerRedrawCooldownUse -= gamblerRedrawCooldownUse > 0 ? 1 : 0;
			this.gamblerShuffleCooldown -= this.gamblerShuffleCooldown > 0 ? 1 : 0;
			this.gamblerUIDisplayTimer = this.gamblerShuffleCooldown <= 0 && this.gamblerDiceDuration <= 0 ? this.gamblerUIDisplayTimer > 0 ? this.gamblerUIDisplayTimer - 1 : this.gamblerUIDisplayTimer : 300 + (600 - gamblerShuffleCooldownMax);
			if (this.gamblerChips > 0 && this.gamblerUIDisplayTimer <= 0 && this.timer120 % 60 == 0) {
				this.gamblerChips --;
				this.gamblerUIDisplayTimer = this.gamblerChips == 0 ? 60 : 0;
			}
			
			if (this.gamblerUIDisplayTimer == 0 && this.gamblerChips == 0 && this.gamblerCardCurrent.type == 0) {
				this.clearGamblerCardCurrent();
				this.gamblerRedraws = 0;
				this.clearGamblerCardsNext();
			}
			
			if (this.gamblerRedraws > this.gamblerRedrawsMax) this.gamblerRedraws = this.gamblerRedrawsMax;
			if (this.gamblerChips > this.gamblerChipsMax) this.gamblerChips = this.gamblerChipsMax;
			
			if (this.gamblerDiceDuration <= 0) {
				this.gamblerDiceID = -1;
				this.gamblerDiceValue = 0;
			} else {
				this.gamblerDiceDuration --;
				switch (this.gamblerDiceID) {
					case 0:
						this.gamblerDamage += (0.02f * this.gamblerDiceValue);	
						break;
					case 1:
						this.gamblerChipsConsume += 2 * this.gamblerDiceValue;
						break;
					default:
						break;
				}
			}
		}
		
		public void shamanPostUpdateEquips() {
			if (this.UIDisplayTimer == 0) {
				this.shamanFireBondLoading = 0;
				this.shamanWaterBondLoading = 0;
				this.shamanAirBondLoading = 0;
				this.shamanEarthBondLoading = 0;
				this.shamanSpiritBondLoading = 0;
			}
			
			if (shamanFireTimer > 0) {
				this.shamanFireTimer -= player.HasBuff(BuffType<Shaman.Buffs.SpiritBuffMana>()) ? 0 : 1;
				this.shamanPollFire += this.timer120 % 15 == 0 ? this.shamanTimerCombat > 0 ? 2 : 1 : 0;
				if (this.shamanPollFire > 0) {
					this.shamanPollFire --;
					this.shamanFireBondLoading += shamanFireBondLoading < 100 ? 1 : 0;
				}
				
				if (shamanSmite && timer120 % 60 == 0) {
					int dmg = (int)(100*shamanDamage);
					
					int randX = Main.rand.Next(50);
					int randY = Main.rand.Next(50);
						
					for (int i = 0 ; i < 3 ; i ++) {
						int dust = Dust.NewDust(new Vector2((int)(player.Center.X + 25 - randX), (int)(player.Center.Y  + 15 - randY)), 0, 0, 162, (float)(1 - Main.rand.Next(2)), (float)(1 - Main.rand.Next(2)), 0, default(Color), 2f);
						Main.dust[dust].noGravity = true;
					}
					
					Projectile.NewProjectile((int)(player.Center.X + 25 - randX), (int)(player.Center.Y  + 15 - randY), 0f, 0f, mod.ProjectileType("Smite"), dmg, 0f, player.whoAmI);
				}
				
				//shamanDamage += (0.04f * shamanFireBuff) + (0.04f * shamanFireBonus);
				this.shamanTimerCombat -= this.shamanTimerCombat > 0 ? 1 : 0;
			} else {
				this.shamanFireBondLoading -= this.shamanFireBondLoading > 0 && this.timer120 % 10 == 0 ? 1 : 0;
				this.shamanPollFire = 0;
				this.shamanTimerCombat = 0;
			}
			
			if (shamanWaterTimer > 0) {
				shamanWaterTimer -= player.HasBuff(BuffType<Shaman.Buffs.SpiritBuffMana>()) ? 0 : 1;
				if (this.shamanPollWater > 0) {
					this.shamanPollWater --;
					this.shamanWaterBondLoading += shamanWaterBondLoading < 100 ? 1 : 0;
				}
				//player.endurance += (0.04f * shamanWaterBuff) + (0.04f * shamanWaterBonus);
				
				if (shamanHeavy) {
					player.statDefense += 10;
					player.moveSpeed -= 0.2f;
				}
				
			} else {
				this.shamanWaterBondLoading -= this.shamanWaterBondLoading > 0 && this.timer120 % 10 == 0 ? 1 : 0;
				this.shamanPollWater = 0;
			}
			
			if (shamanAirTimer > 0) {
				shamanAirTimer -= player.HasBuff(BuffType<Shaman.Buffs.SpiritBuffMana>()) ? 0 : 1;
				//shamanCrit += (4 * shamanAirBuff) + (4 * shamanAirBonus);
				float vel = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				this.shamanPollAir += this.timer120 % 20 == 0 && vel > 1f ? vel > 3.5f ? vel > 6f ? 3 : 2 : 1 : 0;
				
				if (shamanDripping) {
					if (shamanSpiritTimer % 10 == 0)
					{
						int dmg = (int)(30 * shamanDamage + 5E-06f);
						Projectile.NewProjectile(player.Center.X - 10 + (Main.rand.Next(20)), player.Center.Y + 16, 0f, -0.001f, mod.ProjectileType("LavaDroplet"), dmg, 0f, player.whoAmI);
					}	
				}
				
				if (shamanFeather) {
					if (!player.controlDown) player.gravity /= 3;
				}

				if (shamanHarpyAnklet) {
					this.doubleJumpHarpy = true;
				}
				
				if (this.shamanPollAir > 0) {
					this.shamanPollAir --;
					this.shamanAirBondLoading += this.shamanAirBondLoading < 100 ? 1 : 0;
				}
			} else {
				this.shamanAirBondLoading -= this.shamanAirBondLoading > 0 && this.timer120 % 10 == 0 ? 1 : 0;
				this.shamanPollAir = 0;
			}
			
			if (shamanEarthTimer > 0) {
				shamanEarthTimer -= player.HasBuff(BuffType<Shaman.Buffs.SpiritBuffMana>()) ? 0 : 1;
				//player.lifeRegen += (2 * shamanEarthBuff) + (2 * shamanEarthBonus);
				this.shamanTimerImmobile = player.velocity == player.velocity * 0f ? this.shamanTimerImmobile < 240 ? this.shamanTimerImmobile + 1 : this.shamanTimerImmobile : 0;
				
				if (this.timer120 % 8 == 0) {
					this.shamanPollEarth += this.shamanTimerImmobile >= 80 ? this.shamanTimerImmobile >= 160 ? this.shamanTimerImmobile >= 240 ? 3 : 2 : 1 : 0;
				}
				
				if (this.shamanPollEarth > 0) {
					this.shamanPollEarth --;
					this.shamanEarthBondLoading += shamanEarthBondLoading < 100 ? 1 : 0;
				}
				if (shamanHoney) {
					player.AddBuff((48), 1); // Honey
					if (shamanEarthTimer % 90 == 0) {
						
						int randX = Main.rand.Next(150);
						int randY = Main.rand.Next(100);
						
						for (int i = 0 ; i < 3 ; i ++) {
							int dust = Dust.NewDust(new Vector2((int)(player.Center.X + 75 - randX), (int)(player.Center.Y  + 15 - randY)), 0, 0, 152, 0f, 0f, 0, default(Color), 1.3f);
							Main.dust[dust].noGravity = true;
						}
						
						if (Main.player[Main.myPlayer].strongBees && Main.rand.Next(2) == 0) 
							Projectile.NewProjectile((int)(player.Center.X + 75 - randX), (int)(player.Center.Y  + 15 - randY), (float)(Main.rand.Next(3) - 1.5), (float)(Main.rand.Next(3) - 1.5), 566, (int)(12), 0f, player.whoAmI, 0f, 0f);
						else
							Projectile.NewProjectile((int)(player.Center.X + 75 - randX), (int)(player.Center.Y  + 15 - randY), (float)(Main.rand.Next(3) - 1.5), (float)(Main.rand.Next(3) - 1.5), 181, (int)(10), 0f, player.whoAmI, 0f, 0f);
					}
				}
				
				if (shamanForest) {
					player.AddBuff(mod.BuffType("DeepForestAura"), 2);
				}
				
				if (shamanAmber)
					player.statLifeMax2 += 50;
				
			} else {
				this.shamanEarthBondLoading -= this.shamanEarthBondLoading > 0 && this.timer120 % 10 == 0 ? 1 : 0;
				this.shamanPollEarth = 0;
				this.shamanTimerImmobile = 0;
			}
			
			if (shamanSpiritTimer > 0) {
				shamanSpiritTimer -= player.HasBuff(BuffType<Shaman.Buffs.SpiritBuffMana>()) ? 0 : 1;
				//player.moveSpeed += (0.05f * shamanSpiritBuff) + (0.05f * shamanSpiritBonus);
				if (this.shamanPollSpirit > 0) {
					this.shamanPollSpirit --;
					this.shamanSpiritBondLoading += shamanSpiritBondLoading < 100 ? 1 : 0;
				}
			} else {
				this.shamanSpiritBondLoading -= this.shamanSpiritBondLoading > 0 && this.timer120 % 10 == 0 ? 1 : 0;
				this.shamanPollSpirit = 0;
			}
			
			if (doubleJumpHarpy)  // Vanilla double jump code is insanely weird.
			{
				if (!player.controlJump) this.harpySpaceKeyReleased = true;
				
				if (!(player.doubleJumpCloud || player.doubleJumpSail || player.doubleJumpSandstorm 
				|| player.doubleJumpBlizzard || player.doubleJumpFart || player.doubleJumpUnicorn)) 
					player.doubleJumpCloud = true;
					
				if (player.velocity.Y == 0 || player.grappling[0] >= 0 || (shamanHarpyAnklet && shamanSpiritTimer > 0 && !player.controlJump)) 
				{
					if (player.jumpAgainCloud)
					{
					  jumpHeightCheck = (int) ((double) Player.jumpHeight * 0.75);
					}
					if (player.jumpAgainSail)
					{
					  jumpHeightCheck = (int) ((double) Player.jumpHeight * 1.25);
					}
					if (player.jumpAgainFart)
					{
					  jumpHeightCheck = Player.jumpHeight * 2;
					}
					if (player.jumpAgainBlizzard)
					{
					  jumpHeightCheck = (int) ((double) Player.jumpHeight * 1.5);
					}
					if (player.jumpAgainSandstorm)
					{
					  jumpHeightCheck = Player.jumpHeight * 3;
					}
					if (player.jumpAgainUnicorn)
					{
					  jumpHeightCheck = Player.jumpHeight * 2;
					}
				}
				
				if (player.jumpAgainCloud && player.jump == (int) ((double) Player.jumpHeight * 0.75))
					player.jump --;
				
				if ((player.jump == jumpHeightCheck && this.harpySpaceKeyReleased == true)) {
					this.harpySpaceKeyReleased = false;
					int dmg = 10;
					if (shamanHarpyAnklet && shamanSpiritTimer > 0) {
						dmg = (int)(12 * this.shamanDamage);
						if (player.FindBuffIndex(mod.BuffType("HarpyAgility")) > -1)
							dmg += (int)(12 * this.shamanDamage); 
					}
								
					for (float dirX = -1 ; dirX < 2; dirX ++) {
						for (float dirY = -1 ; dirY < 2; dirY ++) {
							bool ankletCanShoot = !(dirX == 0 && dirY == 0 && dirX == dirY);
							float ankletSpeed = 10f;
							if (dirX != 0 && dirY != 0) ankletSpeed = 7.5f;
							if (ankletCanShoot) {
								Projectile.NewProjectile(player.Center.X, player.Center.Y, (dirX*ankletSpeed), (dirY*ankletSpeed), mod.ProjectileType("HarpyAnkletProj"), dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
							}
						}
					}
				}
			}
			
			if (shamanFireTimer <= 0) shamanFireBuff = 0;
			else UIDisplayTimer = UIDisplayDelay;
			if (shamanWaterTimer <= 0) shamanWaterBuff = 0;
			else UIDisplayTimer = UIDisplayDelay;
			if (shamanAirTimer <= 0) shamanAirBuff = 0;
			else UIDisplayTimer = UIDisplayDelay;
			if (shamanEarthTimer <= 0) shamanEarthBuff = 0;
			else UIDisplayTimer = UIDisplayDelay;
			if (shamanSpiritTimer <= 0) shamanSpiritBuff = 0;
			else UIDisplayTimer = UIDisplayDelay;
			
			if (shamanFireBuff+shamanWaterBuff+shamanAirBuff+shamanEarthBuff+shamanSpiritBuff == 0 && UIDisplayTimer > 0) {
				UIDisplayTimer --;
			}
			
				
			OchidScreenH = Main.screenHeight;
			OchidScreenW = Main.screenWidth;
			if (OchidScreenHCompare != OchidScreenH || OchidScreenWCompare  != OchidScreenW) {
				OrchidMod.reloadShamanUI = true;
				OchidScreenHCompare = OchidScreenH;
				OchidScreenWCompare = OchidScreenW;
			}
			
			if(doubleTap > 0) doubleTap --;
			else doubleTapLock = false;
			if(doubleTapCooldown > 0) doubleTapCooldown--;
			
			if (!Main.ReversedUpDownArmorSetBonuses) {
				if (player.controlDown && doubleTap == 0 && doubleTapCooldown == 0) {
					doubleTap += 30;
					doubleTapCooldown = 60;
					doubleTapLock = true;
				}
				
				if (!player.controlDown && doubleTap > 0 && doubleTapLock) {
					doubleTapLock = false;
				}
				
				if (player.controlDown && doubleTap > 0 && !doubleTapLock) {
					if (abyssSet) {
						Projectile.NewProjectile((Main.mouseX + Main.screenPosition.X), (Main.mouseY + Main.screenPosition.Y), 0f, 0f, mod.ProjectileType("AbyssPortal"), 0, 5, player.whoAmI);
						Main.PlaySound(SoundID.Item122, player.Center);
					}
					doubleTap = 0;
					doubleTapCooldown += 1000;
				}
			}
			else {
					if (player.controlUp && doubleTap == 0 && doubleTapCooldown == 0) {
					doubleTap = 30;
					doubleTapLock = true;
				}
				
				if (!player.controlUp && doubleTap > 0 && doubleTapLock) {
					doubleTapLock = false;
				}
				
				if (player.controlUp && doubleTap > 0 && !doubleTapLock) {
					if (abyssSet) {
						Projectile.NewProjectile((Main.mouseX + Main.screenPosition.X), (Main.mouseY + Main.screenPosition.Y), 0f, 0f, mod.ProjectileType("AbyssPortal"), 0, 5, player.whoAmI);
						Main.PlaySound(SoundID.Item122, player.Center);
					}
					doubleTap = 0;
					doubleTapCooldown += 1000;
				}
			}
			
			timer120 ++;
			if (timer120 == 120) {
				timer120 = 0;	
			}
		}
		
		public void postUpdateAlchemist() {
			this.alchemistFlowerTimer -= this.alchemistFlowerTimer > 0 ? 1 : 0;
			this.alchemistFlower = this.alchemistFlowerTimer == 0 ? 0 : this.alchemistFlower;
			
			if (this.alchemistShootProjectile) {	
				float shootSpeed = 10f * this.alchemistVelocity;
				int projType = ProjectileType<Alchemist.Projectiles.AlchemistProj>();				
				Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
				Vector2 heading = target - player.Center;
				heading.Normalize();
				heading *= shootSpeed;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, heading.X, heading.Y, projType, 1, 1f, player.whoAmI);
				this.alchemistShootProjectile = false;
			}
		}
		
		public void postUpdateGambler() {
			switch (this.gamblerDiceID) {
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
			
			if (this.gamblerTimerHoney < 30) {
				this.gamblerTimerHoney ++;
			}
		}
		
		public void postUpdateShaman() {
			if (shamanFireTimer <= 0
				&& shamanWaterTimer <= 0
				&& shamanAirTimer <= 0
				&& shamanEarthTimer <= 0
				&& shamanSpiritTimer <= 0) 
			{

				this.shamanOrbBig = ShamanOrbBig.NULL;
				this.shamanOrbSmall = ShamanOrbSmall.NULL;
				this.shamanOrbLarge = ShamanOrbLarge.NULL;
				this.shamanOrbUnique = ShamanOrbUnique.NULL;
				
				this.orbCountSmall = 0;
				this.orbCountBig = 0;
				this.orbCountLarge = 0;
				this.orbCountUnique = 0;
				this.orbCountCircle = 0;
			}
			
			if (!(player.FindBuffIndex(mod.BuffType("SpiritualBurst")) > -1) && this.orbCountCircle > 39 && this.shamanOrbCircle == ShamanOrbCircle.REVIVER)
					this.orbCountCircle = 0;

			if (this.orbCountBig < 0) 
				this.orbCountBig = 0;
			
			if (this.shamanTimerCrimson < 30) {
				this.shamanTimerCrimson ++;
			}

			if (this.shamanTimerViscount < 180) {
				this.shamanTimerViscount ++;
			}
			
			if (this.shamanTimerHellDamage < 600) {
				this.shamanTimerHellDamage ++;
			}
			
			if (this.shamanTimerHellDefense < 300) {
				this.shamanTimerHellDefense ++;
			}
			
			if (this.timerVial < 30) {
				this.timerVial ++;
			}
		}
		
		public void onRespawnGambler() {
			this.gamblerShuffleCooldown = 0;
			this.gamblerChips = 0;
			this.gamblerRedraws = 0;
			this.gamblerUIDisplayTimer = 0;
			this.clearGamblerCardCurrent();
			this.clearGamblerCardsNext();
		}
		
		public void onRespawnAlchemist() {
			this.alchemistPotency = this.alchemistPotencyMax;
			this.alchemistFlaskDamage = 0;
			this.alchemistNbElements = 0;
			this.alchemistPotencyDisplayTimer = 0;
			this.clearAlchemistElements();
			this.clearAlchemistFlasks();
			this.clearAlchemistDusts();
			this.clearAlchemistColors();
		}
		
		public void onRespawnShaman() {
			this.orbCountSmall = 0;
			this.orbCountBig = 0;
			this.orbCountLarge = 0;
			this.orbCountUnique = 0;
			this.orbCountCircle = 0;
			
			this.shamanOrbBig = ShamanOrbBig.NULL;
			this.shamanOrbSmall = ShamanOrbSmall.NULL;
			this.shamanOrbLarge = ShamanOrbLarge.NULL;
			this.shamanOrbUnique = ShamanOrbUnique.NULL;
			this.shamanOrbCircle = ShamanOrbCircle.NULL;
			
			this.shamanFireTimer = 0;
			this.shamanFireBuff = 0;
			this.shamanWaterTimer = 0;
			this.shamanWaterBuff = 0;
			this.shamanAirTimer = 0;
			this.shamanAirBuff = 0;
			this.shamanEarthTimer = 0;
			this.shamanEarthBuff = 0;
			this.shamanSpiritTimer = 0;
			this.shamanSpiritBuff = 0;
			this.UIDisplayTimer = 0;
		}
		
		public void clearDancerEffects() {
			this.dancerDashTimer = 0;
			this.dancerWeaponDamage = 0;
			this.dancerWeaponKnockback = 0f;
			this.dancerWeaponType = OrchidModDancerItemType.NULL;
			this.dancerVelocity = new Vector2(0f, 0f);
		}
		
		public void clearAlchemistColors() {
			this.alchemistColorR = 50;
			this.alchemistColorG = 100;
			this.alchemistColorB = 255;
		}
		
		public void clearAlchemistFlasks() {
			this.alchemistFlasks = new int[6];
			
			for (int i = 0 ; i < 6 ; i ++) {
				this.alchemistFlasks[i] = 0;
			}
		}
		
		public void clearAlchemistElements() {
			this.alchemistElements = new bool[6];
			
			for (int i = 0 ; i < 6 ; i ++) {
				this.alchemistElements[i] = false;
			}
		}
		
		public void clearAlchemistDusts() {
			this.alchemistDusts = new int[6];
			
			for (int i = 0 ; i < 6 ; i ++) {
				this.alchemistDusts[i] = -1;
			}
		}
		
		public void clearGamblerCardsNext() {
			this.gamblerCardNext = new Item[3];
			for (int i = 0 ; i < 3 ; i ++) {
				this.gamblerCardNext[i] = new Item();
				this.gamblerCardNext[i].SetDefaults(0, true);
			}
		}
		
		public void clearGamblerCardCurrent() {
			gamblerCardCurrent = new Item();
			gamblerCardCurrent.SetDefaults(0, true);
		}
		
		public void clearGamblerCards() {
			this.gamblerCardsItem = new Item[20];
			for (int i = 0; i < 20; i++)
			{
				gamblerCardsItem[i] = new Item();
				gamblerCardsItem[i].SetDefaults(0, true);
			}
			
			gamblerCardCurrent = new Item();
			gamblerCardCurrent.SetDefaults(0, true);
		}
		
		public void removeGamblerCard(Item card) {
			if (this.containsGamblerCard(card)) {
				bool found = false;
				for (int i = 0; i < 20 ; i ++) {
					if (this.gamblerCardsItem[i].type == card.type) {
						found = true;
					}
					if (found) {
						this.gamblerCardsItem[i] = new Item();
						this.gamblerCardsItem[i].SetDefaults(i == 19 ? 0 : this.gamblerCardsItem[i + 1].type, true);
					}
				}
			}
		}
		
		public int getNbAlchemistFlasks() {
			int val = 0;
			for (int i = 0; i < 6; i ++) {
				val += this.alchemistFlasks[i] != 0 ? 1 : 0;
			}
			return val;
		}
		
		public int getNbGamblerCards() {
			if (this.gamblerCardsItem.Count() != 20) {
				this.clearGamblerCards();
			}
			int val = 0;
			for (int i = 0; i < 20; i ++) {
				if (this.gamblerCardsItem[i].type != 0) {
					val ++;
				} else {
					return val;
				}
			}
			return val;
		}
		
		public bool containsAlchemistFlask(int flaskType) {
			for (int i = 0; i < 6; i ++) {
				if (this.alchemistFlasks[i] == flaskType) {
					return true;
				}
			}
			return false;
		}
		
		public bool containsGamblerCard(Item card) {
			for (int i = 0; i < 20; i ++) {
				if (this.gamblerCardsItem[i].type == card.type) {
					return true;
				}
			}
			return false;
		}
		
		public int getNbShamanicBonds() {
			int val = 0;
			
			if (this.shamanFireBuff != 0)
				val ++;
			
			if (this.shamanWaterBuff != 0)
				val ++;
			
			if (this.shamanAirBuff != 0)
				val ++;
			
			if (this.shamanEarthBuff != 0)
				val ++;
				
			if (this.shamanSpiritBuff != 0)
				val ++;
			
			return val;
		}
		
		public void rollGamblerDice(int diceID, int diceDuration) {
			this.gamblerDiceID = diceID;
			this.gamblerDiceValue = Main.rand.Next(6) + 1;
			this.gamblerDiceDuration = 60 * diceDuration;
		}
		
		public void addGamblerChip(int chance) {
			if (Main.rand.Next(100) < chance) {
				this.gamblerChips += this.gamblerChips < this.gamblerChipsMax ? 1 : 0;
			}
			this.gamblerUIDisplayTimer = 300 + (600 - this.gamblerShuffleCooldownMax);
		}
		
		public void removeGamblerChip(int chance, int number) {
			for (int i = 0 ; i < number ; i ++) {
				if (Main.rand.Next(100) < (chance - this.gamblerChipsConsume)) {
					this.gamblerChips --;
				}
			}
			this.gamblerUIDisplayTimer = 300 + (600 - this.gamblerShuffleCooldownMax);
		}
		
		public void removeDancerPoise(int chance, int number) {
			for (int i = 0 ; i < number ; i ++) {
				if (Main.rand.Next(100) < (chance - this.dancerPoiseConsume)) {
					this.dancerPoise --;
				}
			}
			//this.gamblerUIDisplayTimer = 300 + (600 - this.gamblerShuffleCooldownMax);
		}
		
		public void drawGamblerCard() {
			this.gamblerJustSwitched = true;
			
			if (this.gamblerCardNext.Count() != 3) {
				this.clearGamblerCardsNext();
			}
			
			for (int i = 0; i < 3 ; i ++) {
				if (this.gamblerCardNext[i].type == 0) {
					int rand = Main.rand.Next(this.getNbGamblerCards());
					this.gamblerCardNext[i] = new Item();
					this.gamblerCardNext[i].SetDefaults(this.gamblerCardsItem[rand].type, true);
				}
			}
			
			this.gamblerCardCurrent = new Item();
			this.gamblerCardCurrent.SetDefaults(this.gamblerCardNext[0].type, true);
			
			for (int i = 0; i < 2 ; i ++) {
				this.gamblerCardNext[i] = new Item();
				this.gamblerCardNext[i].SetDefaults(this.gamblerCardNext[i + 1].type, true);
			}
			
			this.gamblerCardNext[2] = new Item();
			this.gamblerCardNext[2].SetDefaults(0, true);
			
			for (int i = 0; i < 3 ; i ++) {
				if (this.gamblerCardNext[i].type == 0) {
					int rand = Main.rand.Next(this.getNbGamblerCards());
					this.gamblerCardNext[i] = new Item();
					this.gamblerCardNext[i].SetDefaults(this.gamblerCardsItem[rand].type, true);
				}
			}
			
			if (this.getNbGamblerCards() > 3) {
				for (int i = 2; i > -1 ; i --) {
					for (int j = 2; j > -1 ; j --) {
						int currentType = this.gamblerCardNext[i].type;
						if ((currentType == this.gamblerCardNext[j].type || currentType == this.gamblerCardCurrent.type) && i != j) {
							int k = 0;
							while (k < 5 && (currentType == this.gamblerCardNext[j].type || currentType == this.gamblerCardCurrent.type)) {
								k ++;
								int rand = Main.rand.Next(this.getNbGamblerCards());
								this.gamblerCardNext[i] = new Item();		
								this.gamblerCardNext[i].SetDefaults(this.gamblerCardsItem[rand].type, true);
								currentType = this.gamblerCardNext[i].type;
							}
						}
					}
				}
			}
			
			this.gamblerShuffleCooldown = this.gamblerShuffleCooldownMax;
			
			if (this.gamblerDungeon) {
				int rand = Main.rand.Next(3);
				for (int i = 0; i < rand ; i ++) {
					this.addGamblerChip(100);
				}
			}
			
			if (this.gamblerPennant) {
				OrchidModGlobalItem orchidItem = this.gamblerCardCurrent.GetGlobalItem<OrchidModGlobalItem>();
				if (orchidItem.gamblerCardSets.Contains("Boss")) {
					player.AddBuff(BuffType<Gambler.Buffs.ConquerorsPennantBuff>(), 60 * 10);
				}
			}
			
			if (this.gamblerVulture) {
				int rand = Main.rand.Next(3) + 1;
				int projType = ProjectileType<Gambler.Projectiles.Equipment.VultureCharmProj>();
				for (int i = 0; i < rand; i++) {
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
					Vector2 heading = target - player.Center;
					heading.Normalize();
					heading *= new Vector2(0f, 10f).Length();
					Vector2 vel = heading.RotatedByRandom(MathHelper.ToRadians(20));
					vel = vel * scale; 
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, projType, 12, 0f, player.whoAmI);
					if (i == 0) {
						OrchidModProjectile.spawnDustCircle(player.Center, 31, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);	
					}
				}
			}
		}
		
		public void addShamanicEmpowerment(int type, int level) {
			if (type == 0) {
				return;
			}
			
			if (this.shamanForest && type == 4  && this.shamanEarthTimer == 0) {
				player.AddBuff(BuffType<Shaman.Buffs.DeepForestAura>(), 1);
				
				Projectile.NewProjectile(player.Center.X, player.position.Y, 0f, 0f, ProjectileType<Shaman.Projectiles.Equipment.DeepForestCharmProj>(), 1, 0, player.whoAmI, 0f, 0f);
				Projectile.NewProjectile(player.Center.X, player.position.Y, 0f, 0f, ProjectileType<Shaman.Projectiles.Equipment.DeepForestCharmProj>(), 2, 0, player.whoAmI, 0f, 0f);	
			}
			

			bool newEmpowerment = false;
			int currentTimer = 0;
			int lowestDuration = 60 * this.shamanBuffTimer + 1;
					
			lowestDuration = (this.shamanFireTimer < lowestDuration) ? this.shamanFireTimer : lowestDuration;
			lowestDuration = (this.shamanWaterTimer < lowestDuration) ? this.shamanWaterTimer : lowestDuration;
			lowestDuration = (this.shamanAirTimer < lowestDuration) ? this.shamanAirTimer : lowestDuration;
			lowestDuration = (this.shamanEarthTimer < lowestDuration) ? this.shamanEarthTimer : lowestDuration;
			lowestDuration = (this.shamanSpiritTimer < lowestDuration) ? this.shamanSpiritTimer : lowestDuration;
					
			switch (type) {
				case 1:
					currentTimer = this.shamanFireTimer;
					break;
				case 2:
					currentTimer = this.shamanWaterTimer;
					break;
				case 3:
					currentTimer = this.shamanAirTimer;
					break;
				case 4:
					currentTimer = this.shamanEarthTimer;
					break;
				case 5:
					currentTimer = this.shamanSpiritTimer;
					break;
				default:
					return;
			}
				
			newEmpowerment = currentTimer == 0;
			if (currentTimer == lowestDuration) this.shamanPollWater += 3;
			if (currentTimer < (this.shamanBuffTimer * 60) / 2) this.shamanPollWater += 2;
			if (currentTimer < ((this.shamanBuffTimer * 60) / 4) * 3) this.shamanPollWater += 2;
			if (currentTimer < (this.shamanBuffTimer * 60) / 4) this.shamanPollWater += 2;
				
			if (newEmpowerment) {
				this.shamanPollWater += 2;
				if (this.shamanDryad && this.getNbShamanicBonds() > 0) {
					int lowestNoZeroDuation = 60 * this.shamanBuffTimer + 1;
							
					lowestNoZeroDuation = (this.shamanFireTimer != 0 && this.shamanFireTimer < lowestDuration) ? 0 + this.shamanFireTimer : lowestDuration;
					lowestNoZeroDuation = (this.shamanWaterTimer != 0 && this.shamanWaterTimer < lowestDuration) ? 0 +this.shamanWaterTimer : lowestDuration;
					lowestNoZeroDuation = (this.shamanAirTimer != 0 && this.shamanAirTimer < lowestDuration) ? 0 + this.shamanAirTimer : lowestDuration;
					lowestNoZeroDuation = (this.shamanEarthTimer != 0 && this.shamanEarthTimer < lowestDuration) ? 0 + this.shamanEarthTimer : lowestDuration;
					lowestNoZeroDuation = (this.shamanSpiritTimer != 0 && this.shamanSpiritTimer < lowestDuration) ? 0 + this.shamanSpiritTimer : lowestDuration;
						
					this.shamanFireTimer = (this.shamanFireTimer == lowestDuration) ? this.shamanFireTimer + 60 * 3 : this.shamanFireTimer;
					this.shamanWaterTimer = (this.shamanWaterTimer == lowestDuration) ? this.shamanWaterTimer + 60 * 3 : this.shamanWaterTimer;
					this.shamanAirTimer = (this.shamanAirTimer == lowestDuration) ? this.shamanAirTimer + 60 * 3 : this.shamanAirTimer;
					this.shamanEarthTimer = (this.shamanEarthTimer == lowestDuration) ? this.shamanEarthTimer + 60 * 3 : this.shamanEarthTimer;
					this.shamanSpiritTimer = (this.shamanSpiritTimer == lowestDuration) ? this.shamanSpiritTimer + 60 * 3 : this.shamanSpiritTimer;
				}
			}
			
			int maxBufftimer = 60 * this.shamanBuffTimer;
			int toAdd = (int)(maxBufftimer / (2 + this.shamanHitDelay - (player.HasBuff(BuffType<Shaman.Buffs.SpiritRegeneration>()) ? 1 : 0)));
			switch (type) {
				case 1:
					this.shamanFireBuff = level > this.shamanFireBuff ? level : this.shamanFireBuff;
					this.shamanFireTimer = this.shamanFireTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanFireTimer + toAdd;
					break;
				case 2:
					this.shamanWaterBuff = level > this.shamanWaterBuff ? level : this.shamanWaterBuff;
					this.shamanWaterTimer = this.shamanWaterTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanWaterTimer + toAdd;
					break;
				case 3:
					this.shamanAirBuff = level > this.shamanAirBuff ? level : this.shamanAirBuff;
					this.shamanAirTimer = this.shamanAirTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanAirTimer + toAdd;
					break;
				case 4:
					this.shamanEarthBuff = level > this.shamanEarthBuff ? level : this.shamanEarthBuff;
					this.shamanEarthTimer = this.shamanEarthTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanEarthTimer + toAdd;
					break;
				case 5:
					this.shamanSpiritBuff = level > this.shamanSpiritBuff ? level : this.shamanSpiritBuff;
					this.shamanSpiritTimer = this.shamanSpiritTimer + toAdd > maxBufftimer ? maxBufftimer : this.shamanSpiritTimer + toAdd;
					break;
				default:
					return;
			}
			this.shamanHitDelay = 8;
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
			
			// clone.alchemistElements = this.alchemistElements;
			// clone.alchemistFlasks = this.alchemistFlasks;
			// clone.alchemistDusts = this.alchemistDusts;
			// clone.alchemistColorR = this.alchemistColorR;
			// clone.alchemistColorG = this.alchemistColorG;
			// clone.alchemistColorB = this.alchemistColorB;
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
			
			// packet.Write(alchemistElements[0]);
			// packet.Write(alchemistElements[1]);
			// packet.Write(alchemistElements[2]);
			// packet.Write(alchemistElements[3]);
			// packet.Write(alchemistElements[4]);
			// packet.Write(alchemistElements[5]);
			// packet.Write(alchemistFlasks[0]);
			// packet.Write(alchemistFlasks[1]);
			// packet.Write(alchemistFlasks[2]);
			// packet.Write(alchemistFlasks[3]);
			// packet.Write(alchemistFlasks[4]);
			// packet.Write(alchemistFlasks[5]);
			// packet.Write(alchemistDusts[0]);
			// packet.Write(alchemistDusts[1]);
			// packet.Write(alchemistDusts[2]);
			// packet.Write(alchemistDusts[3]);
			// packet.Write(alchemistDusts[4]);
			// packet.Write(alchemistDusts[5]);
			// packet.Write(alchemistColorR);
			// packet.Write(alchemistColorG);
			// packet.Write(alchemistColorB);

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
			
			// if (clone.alchemistElements[0] != alchemistElements[0]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED0);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistElements[0]);
				// packet.Send();
			// }
			
			// if (clone.alchemistElements[1] != alchemistElements[1]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED1);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistElements[1]);
				// packet.Send();
			// }
			
			// if (clone.alchemistElements[2] != alchemistElements[2]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED2);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistElements[2]);
				// packet.Send();
			// }
			
			// if (clone.alchemistElements[3] != alchemistElements[3]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED3);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistElements[3]);
				// packet.Send();
			// }
			
			// if (clone.alchemistElements[4] != alchemistElements[4]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED4);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistElements[4]);
				// packet.Send();
			// }
			
			// if (clone.alchemistElements[5] != alchemistElements[5]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED5);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistElements[5]);
				// packet.Send();
			// }
			
			// if (clone.alchemistFlasks[0] != alchemistFlasks[0]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED0);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistFlasks[0]);
				// packet.Send();
			// }
			
			// if (clone.alchemistFlasks[1] != alchemistFlasks[1]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED1);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistFlasks[1]);
				// packet.Send();
			// }
			
			// if (clone.alchemistFlasks[2] != alchemistFlasks[2]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED2);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistFlasks[2]);
				// packet.Send();
			// }
			
			// if (clone.alchemistFlasks[3] != alchemistFlasks[3]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED3);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistFlasks[3]);
				// packet.Send();
			// }
			
			// if (clone.alchemistFlasks[4] != alchemistFlasks[4]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED4);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistFlasks[4]);
				// packet.Send();
			// }
			
			// if (clone.alchemistFlasks[5] != alchemistFlasks[5]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED5);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistFlasks[5]);
				// packet.Send();
			// }
			
			// if (clone.alchemistDusts[0] != alchemistDusts[0]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED0);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistDusts[0]);
				// packet.Send();
			// }
			
			// if (clone.alchemistDusts[1] != alchemistDusts[1]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED1);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistDusts[1]);
				// packet.Send();
			// }
			
			// if (clone.alchemistDusts[2] != alchemistDusts[2]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED2);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistDusts[2]);
				// packet.Send();
			// }
			
			// if (clone.alchemistDusts[3] != alchemistDusts[3]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED3);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistDusts[3]);
				// packet.Send();
			// }
			
			// if (clone.alchemistDusts[4] != alchemistDusts[4]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED4);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistDusts[4]);
				// packet.Send();
			// }
			
			// if (clone.alchemistDusts[5] != alchemistDusts[5]) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED5);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistDusts[5]);
				// packet.Send();
			// }
			
			// if (clone.alchemistColorR != alchemistColorR) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTCOLORCHANGEDR);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistColorR);
				// packet.Send();
			// }
			
			// if (clone.alchemistColorG != alchemistColorG) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTCOLORCHANGEDG);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistColorG);
				// packet.Send();
			// }
			
			// if (clone.alchemistColorB != alchemistColorB) {
				// var packet = mod.GetPacket();
				// packet.Write((byte)OrchidModMessageType.ALCHEMISTCOLORCHANGEDB);
				// packet.Write((byte)player.whoAmI);
				// packet.Write(alchemistColorB);
				// packet.Send();
			// }
		}
	}
}