using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Gambler.Accessories;
using OrchidMod.Gambler.Weapons.Cards;
using OrchidMod.Shaman.Accessories;
using OrchidMod.Shaman.Misc;
using OrchidMod.Shaman.Weapons;
using OrchidMod.Shaman.Weapons.Hardmode;
using OrchidMod.Shaman.Weapons.Thorium;
using OrchidMod.Common.ItemDropRules.Conditions;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Content.Items.Placeables;
using Microsoft.Xna.Framework;
using OrchidMod.WorldgenArrays;
using OrchidMod.Content.Items.Pets;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Common.Globals.NPCs
{
	public class NPCLootAndShop : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			switch (type)
			{
				case NPCID.WitchDoctor:
					{
						OrchidUtils.AddItemToShop<ShamanRod>(shop, ref nextSlot);

						if (Main.hardMode)
						{
							OrchidUtils.AddItemToShop<RitualScepter>(shop, ref nextSlot);
						}
					}
					break;
				case NPCID.Demolitionist:
					{
						OrchidUtils.AddItemToShop<GunpowderFlask>(shop, ref nextSlot);
					}
					break;
				case NPCID.Dryad:
					{
						OrchidUtils.AddItemToShop<DryadsGift>(shop, ref nextSlot);
					}
					break;
			}

			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod == null) goto SkipThorium;

			if (thoriumMod.IsNPCTypeEquals("ConfusedZombie", type))
			{
				OrchidUtils.AddItemToShop<PatchWerkScepter>(shop, ref nextSlot);
				return;
			}

		SkipThorium:
			return;
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			OrchidUtils.AddItemToShop<PileOfChips>(shop, ref nextSlot, 3);
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
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Blum>(), 50));
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DungeonCard>(), 33));
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
						npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneScepter>()));
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
				case NPCID.GoblinPeon:
				case NPCID.GoblinThief:
				case NPCID.GoblinWarrior:
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
				if (Main.expertMode)
					rand = Main.rand.Next(73) + 18;
				else
					rand = Main.rand.Next(49) + 12;
				Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Shaman.Misc.AbyssFragment>(), rand, false, 0, false, false);
			}

			if (npc.aiStyle == 94) // Celestial Pillar AI 
			{
				float numberOfPillars = 4;
				int quantity = (int)(Main.rand.Next(25, 41) / 2 / numberOfPillars);
				if (Main.expertMode) quantity = (int)(quantity * 1.5f);

				for (int i = 0; i < quantity; i++)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ModContent.ItemType<Shaman.Misc.AbyssFragment>(), Main.rand.Next(1, 4), false, 0, false, false);
				}
			}

			// BOSSES
			if ((npc.type == NPCID.QueenBee))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(2) == 0)
					{
						if (Main.rand.Next(2) == 0)
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.QueenBeeCard>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Dice.HoneyDie>());
						}
					}
					if (Main.rand.Next(2) == 0)
					{
						int rand = Main.rand.Next(3);
						if (rand == 0)
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.BeeSeeker>());
						}
						else if (rand == 1)
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.WaxyVial>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Air.QueenBeeFlask>());
						}
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
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
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.Nirvana>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.TheCore>());
						}
					}
				}
			}

			if ((npc.type == NPCID.WallofFlesh))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(4))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.ShamanEmblem>());
					}
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<General.Items.Misc.OrchidEmblem>());
				}
			}

			if (npc.type == 50) // King Slime
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.KingSlimeFlask>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.KingSlimeCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
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
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.BulbScepter>());
						}
						else
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FloralStinger>());
						}
					}
				}
			}

			if ((npc.type == NPCID.Golem))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(7))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.SunRay>());
					}
				}
			}

			if ((npc.type == 4))  // Eye of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.EyeCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if ((npc.type == 35))  // Skeletron
			{
				if (!Main.expertMode)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.SkeletronCard>());
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier3>());
				}
			}

			if ((npc.type == 266))  // Brain of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Accessories.PreservedCrimson>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.BrainCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			if (Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1 && npc.boss)
			{
				if (!Main.expertMode)
				{
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Accessories.PreservedCorruption>());
					}
					if (Main.rand.NextBool(3))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.EaterCard>());
					}
				}
				if (globalNPC.AlchemistHit)
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			//THORIUM

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				if ((npc.type == thoriumMod.Find<ModNPC>("TheGrandThunderBirdv2").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(4))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.ThunderScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("QueenJelly").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.QueenJellyfishScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("GranityEnergyStorm").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.GraniteEnergyScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("Viscount").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(7))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.ViscountScepter>());
						}
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Shaman.Misc.Thorium.ViscountMaterial>(), 30, false, 0, false, false);
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("ThePrimeScouter").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(6))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.StarScouterScepter>());
						}
					}
					if (globalNPC.AlchemistHit)
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("FallenDeathBeholder").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.CoznixScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("BoreanStriderPopped").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(5))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("Lich").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(7))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.LichScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("Abyssion").Type) || (npc.type == thoriumMod.Find<ModNPC>("AbyssionCracked").Type) || (npc.type == thoriumMod.Find<ModNPC>("AbyssionReleased").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.NextBool(6))
						{
							Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("PatchWerk").Type))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.PatchWerkScepter>());
				}
			}
			else
			{
				if ((npc.type == NPCID.Mothron))
				{
					if (Main.rand.NextBool(4))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Misc.BrokenHeroScepter>());
					}
				}

				if ((npc.type == NPCID.Vulture))
				{
					int rand = Main.rand.Next(2) + 1;
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Misc.VultureTalon>(), rand);
				}
			}

			if (npc.type == 21 || npc.type == -46 || npc.type == -47 || npc.type == 201 || npc.type == -48 || npc.type == -49 || npc.type == 202 || npc.type == -50 || npc.type == -51 || npc.type == 203 || npc.type == -52 || npc.type == -53 || npc.type == 167)
			{ // Skeletons & vikings in mineshaft
				Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
				int MSMinPosX = (Main.maxTilesX / 2) - ((OrchidMSarrays.MSLenght * 15) / 2);
				int MSMinPosY = (Main.maxTilesY / 3 + 100);
				Rectangle rect = new Rectangle(MSMinPosX, MSMinPosY, (OrchidMSarrays.MSLenght * 15), (OrchidMSarrays.MSHeight * 14));
				if (rect.Contains(new Point((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f))))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), Main.rand.Next(3) + 1, false, 0, false, false);
				}
			}

			if (npc.type == 1 || npc.type == -3 || npc.type == -8 || npc.type == -9 || npc.type == -6 || npc.type == 147 || npc.type == -10) // Most Surface Slimes
			{
				if (Main.rand.NextBool(!OrchidWorld.foundSlimeCard ? 5 : 1000))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.SlimeCard>());
					OrchidWorld.foundSlimeCard = true;
				}
			}

			if ((npc.type == NPCID.PirateShip))
			{
				if (Main.rand.NextBool(5))
				{
					Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.PiratesGlory>());
				}
				if (Main.rand.NextBool(10))
				{
					if (Main.rand.NextBool(20))
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Decks.DeckDog>());
					}
					else
					{
						Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Decks.DeckPirate>());
					}
				}
			}
		}
	}
}