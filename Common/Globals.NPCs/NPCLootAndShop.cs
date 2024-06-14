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
using OrchidMod.Content.Items.Placeables;
using OrchidMod.Content.Items.Pets;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Content.Items;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using OrchidMod.Content.Guardian.Weapons.Runes;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Content.Guardian.Weapons.Shields;
using OrchidMod.Content.Guardian.Accessories;

namespace OrchidMod.Common.Globals.NPCs
{
	public class NPCLootAndShop : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{
			switch (shop.NpcType)
			{
				case NPCID.WitchDoctor:
					{
						OrchidUtils.AddItemToShop<ShamanRod>(shop);

						if (Main.hardMode)
						{
							OrchidUtils.AddItemToShop<RitualScepter>(shop);
						}
					}
					break;
				case NPCID.Demolitionist:
					{
						OrchidUtils.AddItemToShop<GunpowderFlask>(shop);
					}
					break;
				case NPCID.Dryad:
					{
						OrchidUtils.AddItemToShop<DryadsGift>(shop);
					}
					break;
			}

			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod == null) goto SkipThorium;

			if (thoriumMod.IsNPCTypeEquals("ConfusedZombie", shop.NpcType))
			{
				OrchidUtils.AddItemToShop<PatchWerkScepter>(shop);
				return;
			}

		SkipThorium:
			return;
		}


		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			OrchidUtils.AddItemToShop<PileOfChips>(shop, ref nextSlot, 3);
			if (Main.hardMode) OrchidUtils.AddItemToShop<BijouShield>(shop, ref nextSlot, 2);
		}

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			globalLoot.Add(new ItemDropWithConditionRule(ModContent.ItemType<ShroomKey>(), 2500, 1, 1, new OrchidDropConditions.ShroomKeyCondition(), 1));
		}

		public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
		{

			// Common drop
			switch (npc.type)
			{
				// Certain NPCs
				case NPCID.SpikedJungleSlime:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<JungleSlimeCard>(), 25));
					}
					break;
				case NPCID.LavaSlime:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LavaSlimeCard>(), 25));
					}
					break;
				case NPCID.WyvernHead:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WyvernTailFeather>(), 15));
					}
					break;
				case NPCID.UndeadViking:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrostburnSigil>(), 30));
					}
					break;
				case NPCID.Demon:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FurnaceSigil>(), 30));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DemonicPocketMirror>(), 20));
					}
					break;
				case NPCID.DarkCaster:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Blum>(), 2));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DungeonCard>(), 25));
					}
					break;
				case NPCID.FireImp:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeltedRing>(), 20));
					}
					break;
				case NPCID.IceQueen:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceFlakeCone>(), 10));
					}
					break;
				case NPCID.RuneWizard:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneScepter>(), 2));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneRune>(), 2));
					}
					break;
				case NPCID.Everscream:
					{
						npcLoot.Add(ItemDropRule.Common(ItemType<FrostRune>(), 5));
					}
					break;
				case NPCID.GoblinSummoner:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoblinStick>(), 3));
					}
					break;
				case NPCID.MourningWood:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MourningTorch>(), 10));
					}
					break;
				case NPCID.SantaNK1:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragilePresent>(), 10));
					}
					break;
				case NPCID.Mimic:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeavyBracer>(), 10));
					}
					break;
				case NPCID.IceMimic:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceMimicScepter>(), 3));
					}
					break;
				case NPCID.UndeadMiner:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TreasuredBaubles>(), 5));
					}
					break;
				case NPCID.Harpy:
					{
						npcLoot.Add(ItemDropRule.ByCondition(new OrchidDropConditions.DownedEyeOfCthulhu(), ModContent.ItemType<HarpyTalon>(), 5));
					}
					break;
				case NPCID.ElfCopter:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RCRemote>(), 50));
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
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PoisonSigil>(), 30));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PoisonVial>(), 20));
					}
					break;
				case NPCID.GoblinWarrior:
					npcLoot.Add(ItemDropRule.Common(ItemType<GoblinSpike>(), 20));
					break;
				case NPCID.GoblinPeon:
				case NPCID.GoblinThief:
				case NPCID.GoblinSorcerer:
				case NPCID.GoblinArcher:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoblinArmyFlask>(), 50));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoblinArmyCard>(), 50));
					}
					break;
				case NPCID.Drippler:
				case NPCID.BloodZombie:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodMoonFlask>(), 40));
					}
					break;
				case NPCID.PirateCorsair:
				case NPCID.PirateDeadeye:
				case NPCID.PirateDeckhand:
				case NPCID.PirateCrossbower:
				case NPCID.PirateCaptain:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PirateWarhammer>(), npc.type == NPCID.PirateCaptain ? 20 : 100));
					}
					break;
				case NPCID.DiabolistRed:
				case NPCID.DiabolistWhite:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DiabolistRune>(), 20));
					}
					break;
				case NPCID.MartianSaucerCore:
				case NPCID.MartianSaucer:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MartianBeamer>(), 4));
					}
					break;
				case NPCID.Lihzahrd:
				case NPCID.LihzahrdCrawler:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LihzahrdSilk>(), 4));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunPriestTorch>(), 100));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunPriestBelt>(), 300));
					}
					break;
				case NPCID.BlackRecluse:
				case NPCID.BlackRecluseWall:
					{
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VenomSigil>(), 40));
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
			// TODO : UDPATE THE UNDER

			OrchidGlobalNPC globalNPC = npc.GetGlobalNPC<OrchidGlobalNPC>();

			
			if ((npc.type == NPCID.CultistBoss))
			{
				int rand;
				if (Main.expertMode) rand = Main.rand.Next(73) + 18;
				else rand = Main.rand.Next(49) + 12;
				Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<AbyssFragment>(), rand, false, 0, false, false);
			}

			if (npc.aiStyle == 94) // Celestial Pillar AI 
			{
				float numberOfPillars = 4;
				int quantity = (int)(Main.rand.Next(25, 41) / 2 / numberOfPillars);
				if (Main.expertMode) quantity = (int)(quantity * 1.5f);
				for (int i = 0; i < quantity; i++)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<GuardianFragmentMaterial>(), Main.rand.Next(1, 4), false, 0, false, false);
				}
			}

			// BOSSES
			if ((npc.type == NPCID.QueenBee))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(2))
					{
						if (Main.rand.NextBool(2))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.QueenBeeCard>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Dice.HoneyDie>());
						}
					}

					int rand = Main.rand.Next(3);
					if (rand == 0)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.BeeSeeker>());
					}
					else if (rand == 1)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Accessories.WaxyVial>());
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

			if ((npc.type == NPCID.MoonLordCore))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(5))
					{
						if (Main.rand.NextBool(2))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Hardmode.Nirvana>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Hardmode.TheCore>());
						}
					}
				}
			}

			if ((npc.type == NPCID.WallofFlesh))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool()) Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ShamanEmblem>());
					else Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<GuardianEmblem>());
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<OrchidEmblem>());
				}
			}

			if (npc.type == NPCID.KingSlime) // King Slime
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Weapons.Water.KingSlimeFlask>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.KingSlimeCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if ((npc.type == NPCID.Plantera))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						if (Main.rand.NextBool(2))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Hardmode.BulbScepter>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Accessories.FloralStinger>());
						}
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

			if ((npc.type == NPCID.Golem))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Hardmode.SunRay>());
					}
					else
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<TempleWarhammer>());
					}
				}
			}

			if ((npc.type == NPCID.EyeofCthulhu))  // Eye of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.EyeCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if ((npc.type == NPCID.SkeletronHead))  // Skeletron
			{
				if (!Main.expertMode)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.SkeletronCard>());
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Misc.Scrolls.ScrollTier3>());
				}
			}

			if ((npc.type == NPCID.BrainofCthulhu))  // Brain of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Alchemist.Accessories.PreservedCrimson>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.BrainCard>());
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
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.EaterCard>());
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
				if (thoriumMod.IsNPCTypeEquals("TheGrandThunderBirdv2", npc.type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(4))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.ThunderScepter>());
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
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.QueenJellyfishScepter>());
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
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.GraniteEnergyScepter>());
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
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.ViscountScepter>());
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
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.StarScouterScepter>());
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
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Thorium.PatchWerkScepter>());
				}
			}
			else
			{
				if ((npc.type == NPCID.Mothron))
				{
					if (Main.rand.NextBool(4))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Misc.BrokenHeroScepter>());
					}
				}

				if ((npc.type == NPCID.Vulture))
				{
					int rand = Main.rand.Next(2) + 1;
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Misc.VultureTalon>(), rand);
				}
			}

			if (npc.type == NPCID.BlueSlime || npc.type == -3 || npc.type == -8 || npc.type == -9 || npc.type == -6 || npc.type == NPCID.IceSlime || npc.type == -10) // Most Surface Slimes
			{
				if (Main.rand.NextBool(!OrchidWorld.foundSlimeCard ? 5 : 1000))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Gambler.Weapons.Cards.SlimeCard>());
					OrchidWorld.foundSlimeCard = true;
				}
			}

			if ((npc.type == NPCID.PirateShip))
			{
				if (Main.rand.NextBool(5))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Content.Shaman.Weapons.Hardmode.PiratesGlory>());
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