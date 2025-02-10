using OrchidMod.Content.Alchemist.Weapons.Fire;
using OrchidMod.Content.Alchemist.Weapons.Nature;
using OrchidMod.Content.Alchemist.Weapons.Water;
using OrchidMod.Content.Gambler.Accessories;
using OrchidMod.Content.Gambler.Weapons.Cards;
using OrchidMod.Content.Shaman.Accessories;
using OrchidMod.Content.Shaman.Misc;
using OrchidMod.Content.Shaman.Weapons;
using OrchidMod.Content.Shaman.Weapons.Hardmode;
using OrchidMod.Content.Shaman.Weapons.Thorium;
using OrchidMod.Common.ItemDropRules.Conditions;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Content.General.Pets;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using OrchidMod.Content.Guardian.Weapons.Runes;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Content.Guardian.Weapons.Shields;
using OrchidMod.Content.Guardian.Accessories;
using OrchidMod.Common.ModSystems;
using OrchidMod.Content.Guardian.Armors.Misc;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Guardian.Weapons.Standards;
using OrchidMod.Content.General.Misc;
using OrchidMod.Content.General.Armor.Vanity;
using OrchidMod.Content.Shapeshifter.Accessories;
using OrchidMod.Content.Shapeshifter.Misc;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using OrchidMod.Content.Guardian.Weapons.Quarterstaves;
using OrchidMod.Content.Shapeshifter.Weapons.Sage;

namespace OrchidMod.Common.Global.NPCs
{
	public class NPCLootAndShop : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{
			switch (shop.NpcType)
			{
				case NPCID.WitchDoctor:
					{
						// shop.Add(ItemType<ShamanRod>());
						// shop.Add(ItemType<RitualScepter>(), [Condition.Hardmode]);
					}
					break;
				case NPCID.Demolitionist:
					{
						shop.Add(ItemType<GunpowderFlask>());
					}
					break;
				case NPCID.Cyborg:
					{
						shop.Add(ItemType<NanitesGauntlet>());
					}
					break;
				case NPCID.Dryad:
					{
						// shop.Add(ItemType<DryadsGift>());
						shop.Add(ItemType<ShapeshifterBlankEffigy>());
					}
					break;
				case NPCID.Merchant:
					{
						shop.Add(ItemType<GuardianBuffStation>(), [Condition.Hardmode]);
					}
					break;
				case NPCID.SkeletonMerchant:
					{
						shop.Add(ItemType<GuardianGitHelm>());
					}
					break;
				case NPCID.Clothier:
					{
						shop.Add(ItemType<EmpressPlateHead>(), [Condition.DownedEmpressOfLight, Condition.InHallow]);
						shop.Add(ItemType<EmpressPlateChest>(), [Condition.DownedEmpressOfLight, Condition.InHallow]);
						shop.Add(ItemType<EmpressPlateLegs>(), [Condition.DownedEmpressOfLight, Condition.InHallow]);
					}
					break;
				case NPCID.BestiaryGirl:
					{
						shop.Add(ItemType<ShapeshifterShampoo>(), [Condition.BestiaryFilledPercent(30)]);
					}
					break;
			}

			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod == null) goto SkipThorium;

			if (thoriumMod.IsNPCTypeEquals("Blacksmith", shop.NpcType))
			{
				OrchidUtils.AddItemToShop<JewelerGauntlet>(shop);
				return;
			}

			SkipThorium:
			return;
		}


		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (Main.rand.NextBool())
			{
				if (WorldGen.shadowOrbSmashed) OrchidUtils.AddItemToShop<Skateboard>(shop, ref nextSlot);
			}
			else
			{
				OrchidUtils.AddItemToShop<PileOfChips>(shop, ref nextSlot);
			}

			if (Main.hardMode)
			{
				if (Main.rand.NextBool())
				{
					OrchidUtils.AddItemToShop<BijouShield>(shop, ref nextSlot);
				}
				else
				{
					OrchidUtils.AddItemToShop<HockeyQuarterstaff>(shop, ref nextSlot);
				}
			}
		}

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			globalLoot.Add(new ItemDropWithConditionRule(ItemType<ShroomKey>(), 2500, 1, 1, new OrchidDropConditions.ShroomKeyCondition(), 1));
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{

			// Common drop
			switch (npc.type)
			{
				// Certain NPCs
				case NPCID.SpikedJungleSlime:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<JungleSlimeCard>(), 25));
					}
					break;
				case NPCID.LavaSlime:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<LavaSlimeCard>(), 25));
					}
					break;
				case NPCID.WyvernHead:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<WyvernTailFeather>(), 15));
					}
					break;
				case NPCID.UndeadViking:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<FrostburnSigil>(), 30));
					}
					break;
				case NPCID.Demon:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<FurnaceSigil>(), 30));
						npcLoot.Add(ItemDropRule.Common(ItemType<DemonicPocketMirror>(), 20));
					}
					break;
				case NPCID.DarkCaster:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<Blum>(), 15));
						npcLoot.Add(ItemDropRule.Common(ItemType<DungeonCard>(), 25));
					}
					break;
				case NPCID.FireImp:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<MeltedRing>(), 20));
					}
					break;
				case NPCID.IceQueen:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<IceFlakeCone>(), 10));
					}
					break;
				case NPCID.RuneWizard:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<RuneScepter>(), 2));
						npcLoot.Add(ItemDropRule.Common(ItemType<RuneRune>(), 2));
					}
					break;
				case NPCID.Everscream:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<FrostRune>(), 5));
					}
					break;
				case NPCID.GoblinSummoner:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<GoblinStick>(), 3));
					}
					break;
				case NPCID.MourningWood:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<MourningTorch>(), 10));
					}
					break;
				case NPCID.SantaNK1:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<FragilePresent>(), 10));
					}
					break;
				case NPCID.Mimic:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<HeavyBracer>(), 10));
					}
					break;
				case NPCID.IceMimic:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<IceMimicScepter>(), 3));
					}
					break;
				case NPCID.UndeadMiner:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<TreasuredBaubles>(), 5));
					}
					break;
				case NPCID.Harpy:
					{
						// npcLoot.Add(ItemDropRule.ByCondition(new OrchidDropConditions.DownedEyeOfCthulhu(), ItemType<HarpyTalon>(), 5));
					}
					break;
				case NPCID.ElfCopter:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<RCRemote>(), 50));
					}
					break;

				// Multiple NPCs
				case NPCID.Hornet:
				case NPCID.HornetFatty:
				case NPCID.HornetHoney:
				case NPCID.HornetLeafy:
				case NPCID.HornetSpikey:
				case NPCID.HornetStingy:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<PoisonSigil>(), 30));
						npcLoot.Add(ItemDropRule.Common(ItemType<PoisonVial>(), 20));
					}
					break;
				case NPCID.GoblinWarrior:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<GoblinSpike>(), 15));
						npcLoot.Add(ItemDropRule.Common(ItemType<GoblinArmyCard>(), 50));
					}
					break;
				case NPCID.GoblinPeon:
				case NPCID.GoblinThief:
				case NPCID.GoblinArcher:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<GoblinArmyFlask>(), 50));
						npcLoot.Add(ItemDropRule.Common(ItemType<GoblinArmyCard>(), 50));
					}
					break;
				case NPCID.GoblinSorcerer:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<GoblinArmyCard>(), 50));
						npcLoot.Add(ItemDropRule.Common(ItemType<GoblinRune>(), 10));
					}
					break;
				case NPCID.Drippler:
				case NPCID.BloodZombie:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<BloodMoonFlask>(), 40));
					}
					break;
				case NPCID.PirateCorsair:
				case NPCID.PirateDeadeye:
				case NPCID.PirateDeckhand:
				case NPCID.PirateCrossbower:
					npcLoot.Add(ItemDropRule.Common(ItemType<PirateWarhammer>(), 100));
					break;
				case NPCID.PirateCaptain:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<PirateWarhammer>(), 20));
						npcLoot.Add(ItemDropRule.Common(ItemType<PirateStandard>(), 10));
					}
					break;
				case NPCID.DiabolistRed:
				case NPCID.DiabolistWhite:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<DiabolistRune>(), 20));
					}
					break;
				case NPCID.MartianSaucerCore:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<MartianWarhammer>(), 6));
					}
					break;
				case NPCID.Lihzahrd:
				case NPCID.LihzahrdCrawler:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<LihzahrdSilk>(), 4));
						npcLoot.Add(ItemDropRule.Common(ItemType<SunPriestTorch>(), 100));
						npcLoot.Add(ItemDropRule.Common(ItemType<SunPriestBelt>(), 300));
					}
					break;
				case NPCID.BlackRecluse:
				case NPCID.BlackRecluseWall:
					{
						// npcLoot.Add(ItemDropRule.Common(ItemType<VenomSigil>(), 40));
					}
					break;
				case NPCID.Paladin:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<PaladinGauntlet>(), 15));
					}
					break;
				case NPCID.DevourerHead:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<ColossalWormTooth>(), 10));
					}
					break;
				case NPCID.FaceMonster:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<TerrifyingMonsterFang>(), 10));
					}
					break;
				case NPCID.ShortBones:
				case NPCID.AngryBones:
				case NPCID.BigBoned:
				case NPCID.AngryBonesBig:
				case NPCID.AngryBonesBigHelmet:
				case NPCID.AngryBonesBigMuscle:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<BadgeBattlesPast>(), 20));
					}
					break;
				case NPCID.WallCreeper:
				case NPCID.WallCreeperWall:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<WardenSpider>(), 15));
					}
					break;
				case NPCID.CaveBat:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<SageBat>(), 100));
					}
					break;
				default:
					break;
			}

			var notExpertCondition = new Conditions.NotExpert();
			var isExpertCondition = new Conditions.IsExpert();

			// From bosses
			switch (npc.type)
			{
				case NPCID.QueenBee:
					{

					}
					break;
				case NPCID.CultistBoss:
					{

					}
					break;
				default:
					break;
			}
		}

		public override void OnKill(NPC npc)
		{
			OrchidGlobalNPC globalNPC = npc.GetGlobalNPC<OrchidGlobalNPC>();

			if (npc.type == NPCID.CultistBoss)
			{
				int rand;
				if (Main.expertMode) rand = Main.rand.Next(73) + 18;
				else rand = Main.rand.Next(49) + 12;
				// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<AbyssFragment>(), rand, false, 0, false, false);
			}

			if (npc.aiStyle == 94) // Celestial Pillar AI 
			{
				int quantity = Main.rand.Next(3, 15);
				if (Main.expertMode) quantity = (int)(quantity * 1.5f);
				for (int i = 0; i < quantity; i++)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<HorizonFragment>(), Main.rand.Next(1, 4), false, 0, false, false);
				}
			}

			// BOSSES
			if (npc.type == NPCID.QueenBee)
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(2))
					{
						if (Main.rand.NextBool(2))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<QueenBeeCard>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Dice.HoneyDie>());
						}
					}

					int rand = Main.rand.Next(3);
					if (rand == 0)
					{
						// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BeeSeeker>());
					}
					else if (rand == 1)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<WaxyVial>());
					}
					else
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Weapons.Air.QueenBeeFlask>());
					}

					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BeeRune>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			if (npc.type == NPCID.MoonLordCore)
			{
				if (!Main.expertMode)
				{ // Vanilla is 2 random items from the loot pool
					if (Main.rand.NextBool())
					{
						// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Nirvana>());
					}
					else
					{
						// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<TheCore>());
					}

					if (Main.rand.NextBool())
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<MoonLordRune>());
					}
					else
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<MoonLordShield>());
					}
				}
			}

			if (npc.type == NPCID.WallofFlesh)
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool())
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ShamanEmblem>());
					}
					else
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<GuardianEmblem>());
					}

					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<OrchidEmblem>());
				}
			}

			if (npc.type == NPCID.KingSlime) // King Slime
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<KingSlimeFlask>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<KingSlimeCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if (npc.type == NPCID.Plantera)
			{
				if (!Main.expertMode)
				{
					switch (Main.rand.Next(3))
					{
						default:
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<PlanteraStandard>());
							break;
						case 1:
							// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FloralStinger>());
							break;
						case 2:
							// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BulbScepter>());
							break;
					}
				}
			}

			if (npc.type == NPCID.HallowBoss)
			{
				if (!Main.expertMode)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<GuardianEmpressMaterial>(), 16 + Main.rand.Next(11));
				}
			}

			if (npc.type == NPCID.Golem)
			{
				if (!Main.expertMode)
				{
					switch (Main.rand.Next(3))
					{
						default:
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<TempleWarhammer>());
							break;
						case 1:
							// Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SunRay>());
							break;
						case 2:
							break;
					}
				}
			}

			if (npc.type == NPCID.EyeofCthulhu)  // Eye of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<EyeCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if (npc.type == NPCID.SkeletronHead)  // Skeletron
			{
				if (!Main.expertMode)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SkeletronCard>());
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier3>());
				}
			}

			if (npc.type == NPCID.BrainofCthulhu)  // Brain of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Accessories.PreservedCrimson>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BrainCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			if (Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1 && npc.boss)
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Accessories.PreservedCorruption>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<EaterCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			//THORIUM

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{

				if (thoriumMod.IsNPCTypeEquals("ThePrimeScouter", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(7))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ThoriumStarScouterStandard>());
						}
					}
				}

				/*
				if (thoriumMod.IsNPCTypeEquals("TheGrandThunderBirdv2", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(4))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ThunderScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier1>());
					}
				}

				if (thoriumMod.IsNPCTypeEquals("QueenJelly", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<QueenJellyfishScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if (thoriumMod.IsNPCTypeEquals("GraniteEnergyStorm", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<GraniteEnergyScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if (thoriumMod.IsNPCTypeEquals("Viscount", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(7))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ViscountScepter>());
						}
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Content.Shaman.Misc.Thorium.ViscountMaterial>(), 30, false, 0, false, false);
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if (thoriumMod.IsNPCTypeEquals("ThePrimeScouter", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(6))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<StarScouterScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if (thoriumMod.IsNPCTypeEquals("FallenDeathBeholder", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.Hardmode.CoznixScepter>());
						}
					}
				}

				if (thoriumMod.IsNPCTypeEquals("BoreanStriderPopped", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>());
						}
					}
				}

				if (thoriumMod.IsNPCTypeEquals("Lich", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(7))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.Hardmode.LichScepter>());
						}
					}
				}

				if (thoriumMod.IsNPCTypeEquals("Abyssion", npc.type) || thoriumMod.IsNPCTypeEquals("AbyssionCracked", npc.type) || thoriumMod.IsNPCTypeEquals("AbyssionReleased", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(6))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>());
						}
					}
				}

				if (thoriumMod.IsNPCTypeEquals("PatchWerk", npc.type))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<PatchWerkScepter>());
				}
				*/
			}
			else
			{
				if (npc.type == NPCID.Mothron)
				{
					if (Main.rand.NextBool(4))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BrokenHeroScepter>());
					}
				}

				if (npc.type == NPCID.Vulture)
				{
					int rand = Main.rand.Next(2) + 1;
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Misc.VultureTalon>(), rand);
				}
			}

			if (npc.type == NPCID.BlueSlime || npc.type == -3 || npc.type == -8 || npc.type == -9 || npc.type == -6 || npc.type == NPCID.IceSlime || npc.type == -10) // Most Surface Slimes
			{
				if (Main.rand.NextBool(!OrchidWorld.foundSlimeCard ? 5 : 1000))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SlimeCard>());
					OrchidWorld.foundSlimeCard = true;
				}
			}

			if (npc.type == NPCID.PirateShip)
			{
				if (Main.rand.NextBool(5))
				{
					//Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<PiratesGlory>());
				}
				if (Main.rand.NextBool(10))
				{
					if (Main.rand.NextBool(20))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Decks.DeckDog>());
					}
					else
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Decks.DeckPirate>());
					}
				}
			}
		}
	}
}