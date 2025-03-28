﻿using OrchidMod.Content.Alchemist.Accessories;
using OrchidMod.Content.Alchemist.Weapons.Air;
using OrchidMod.Content.Alchemist.Weapons.Catalysts;
using OrchidMod.Content.Alchemist.Weapons.Fire;
using OrchidMod.Content.Alchemist.Weapons.Water;
using OrchidMod.Content.General.Materials;
using OrchidMod.Content.Gambler.Accessories;
using OrchidMod.Content.Gambler.Decks;
using OrchidMod.Content.Gambler.Misc;
using OrchidMod.Content.Gambler.Weapons.Cards;
using OrchidMod.Content.Gambler.Weapons.Chips;
using OrchidMod.Content.Gambler.Weapons.Dice;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Content.General.Armor.Vanity;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using OrchidMod.Content.Guardian.Weapons.Runes;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Content.Guardian.Accessories;
using OrchidMod.Content.Guardian.Weapons.Standards;
using OrchidMod.Content.Guardian.Weapons.Shields;
using OrchidMod.Content.General.Misc;
using OrchidMod.Content.General.Mounts;
using OrchidMod.Content.General.Melee;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Shapeshifter.Weapons.Predator;
using OrchidMod.Content.Guardian.Weapons.Quarterstaves;
using OrchidMod.Utilities;

namespace OrchidMod.Common.Global.Items
{
	public partial class OrchidGlobalItem
	{
		/*
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			switch (context)
			{
				case "bossBag": OpenBossBag(player, arg); break;
				case "crate": OpenCrate(player, arg); break;
				case "lockBox": OpenGoldenLockBox(player); break;
			}
		}
		*/

		// temp ..

		public override void RightClick(Item item, Player player)
		{
			OpenBossBag(player, item.type);
			OpenCrate(player, item.type);
			if (item.type == ItemID.LockBox) OpenGoldenLockBox(player);
		}

		// ...

		private static void OpenBossBag(Player player, int arg)
		{
			switch (arg)
			{
				case ItemID.KingSlimeBossBag:
					{
						QuickSpawnItem<KingSlimeFlask>(player, 1, 3);
						QuickSpawnItem<KingSlimeCard>(player, 1, 3);
					}
					break;
				case ItemID.EyeOfCthulhuBossBag:
					{
						QuickSpawnItem<EyeCard>(player, 1, 3);
						if (Main.rand.NextBool(20))
						{
							QuickSpawnItem<SquareMinecart>(player);
							QuickSpawnItem<PrototypeSecrecy>(player);
						}
					}
					break;
				case ItemID.EaterOfWorldsBossBag:
					{
						QuickSpawnItem<PreservedCorruption>(player, 1, 3);
						QuickSpawnItem<EaterCard>(player, 1, 3);
					}
					break;
				case ItemID.BrainOfCthulhuBossBag:
					{
						QuickSpawnItem<PreservedCrimson>(player, 1, 3);
						QuickSpawnItem<BrainCard>(player, 1, 3);
					}
					break;
				case ItemID.QueenBeeBossBag:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<QueenBeeCard>(), 1),
								(ModContent.ItemType<HoneyDie>(), 1)
							},
							chanceDenominator: 2
						);
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<BeeSeeker>(), 1),
								// (ModContent.ItemType<WaxyVial>(), 1),
								(ModContent.ItemType<QueenBeeFlask>(), 1)
							},
							chanceDenominator: 2
							//chanceDenominator: 3
						);
						QuickSpawnItem<BeeRune>(player, 1, 4);
					}
					break;
				case ItemID.SkeletronBossBag:
					{
						QuickSpawnItem<SkeletronCard>(player, 1, 1);
					}
					break;
				case ItemID.WallOfFleshBossBag:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<ShamanEmblem>(), 1),
								(ModContent.ItemType<GuardianEmblem>(), 1)
							}
						);
						QuickSpawnItem<OrchidEmblem>(player, 1, 1);
					}
					break;
				case ItemID.PlanteraBossBag:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<BulbScepter>(), 1),
								//(ModContent.ItemType<FloralStinger>(), 1),
								//(ModContent.ItemType<PlanteraStandard>(), 1),
							}
						);
						QuickSpawnItem<OrnateOrchid>(player, 1, 20);
						QuickSpawnItem<PlanteraStandard>(player, 1, 3); // Delete when shaman items are readded
					}
					break;
				case ItemID.GolemBossBag:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<SunRay>(), 1),
								(ModContent.ItemType<TempleWarhammer>(), 1)
							},
							//chanceDenominator: 2
							chanceDenominator: 3
						);
					}
					break;
				case ItemID.FairyQueenBossBag:
					{
						QuickSpawnItem<GuardianEmpressMaterial>(player, 20 + Main.rand.Next(10), 1);
					}
					break;
				case ItemID.MoonLordBossBag:
					{ // Vanilla is 2 random items from the loot pool
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<Nirvana>(), 1),
								//(ModContent.ItemType<TheCore>(), 1)
							}
						);
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<MoonLordRune>(), 1),
								(ModContent.ItemType<MoonLordShield>(), 1)
							}
						);
					}
					break;
				default:
					break;
			}

			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod == null) goto SkipThorium;

			if (thoriumMod.IsItemTypeEquals("ScouterBag", arg))
			{
				QuickSpawnItem<ThoriumStarScouterStandard>(player, 1, 7);
				return;
			}

			/*
			if (thoriumMod.IsItemTypeEquals("ThunderBirdBag", arg))
			{
				QuickSpawnItem<ThunderScepter>(player, 1, 4);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("JellyFishBag", arg))
			{
				QuickSpawnItem<QueenJellyfishScepter>(player, 1, 5);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("GraniteBag", arg))
			{
				QuickSpawnItem<GraniteEnergyScepter>(player, 1, 5);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("CountBag", arg))
			{
				QuickSpawnItem<GraniteEnergyScepter>(player, 1, 7);
				QuickSpawnItem<ViscountMaterial>(player, 30, 1);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("ScouterBag", arg))
			{
				QuickSpawnItem<StarScouterScepter>(player, 1, 6);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("BeholderBag", arg))
			{
				QuickSpawnItem<CoznixScepter>(player, 1, 5);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("BoreanBag", arg))
			{
				QuickSpawnItem<BoreanStriderScepter>(player, 1, 5);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("LichBag", arg))
			{
				QuickSpawnItem<LichScepter>(player, 1, 7);
				return;
			}

			if (thoriumMod.IsItemTypeEquals("AbyssionBag", arg))
			{
				QuickSpawnItem<AbyssionScepter>(player, 1, 6);
				return;
			}
			*/

			SkipThorium:
			return;
		}

		private static void OpenCrate(Player player, int arg)
		{
			switch (arg)
			{
				case ItemID.WoodenCrate:
				case ItemID.WoodenCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<AdornedBranch>(), 1),
								(ModContent.ItemType<EmberVial>(), 1),
								(ModContent.ItemType<EmbersCard>(), 1),
								(ModContent.ItemType<TsunamiInAVial>(), 1),
								(ModContent.ItemType<Quarterstaff>(), 1),
								(ModContent.ItemType<GuideShield>(), 1),
							}
						);
					}
					break;
				case ItemID.IronCrate:
				case ItemID.IronCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<AdornedBranch>(), 1),
								(ModContent.ItemType<EmberVial>(), 1),
								(ModContent.ItemType<EmbersCard>(), 1),
								(ModContent.ItemType<TsunamiInAVial>(), 1),
								(ModContent.ItemType<Quarterstaff>(), 1),
								(ModContent.ItemType<GuideShield>(), 1),
							}
						);
					}
					break;
				case ItemID.CorruptFishingCrate:
				case ItemID.CorruptFishingCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<ShadowWeaver>(), 1),
								(ModContent.ItemType<DemoniteCatalyst>(), 1),
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.JungleFishingCrate:
				case ItemID.JungleFishingCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
									//(ModContent.ItemType<DeepForestCharm>(), 1),
									(ModContent.ItemType<BloomingBud>(), 1),
									(ModContent.ItemType<BundleOfClovers>(), 1),
									(ModContent.ItemType<JungleGauntlet>(), 1),
							}
						);

						QuickSpawnItem<JungleLily>(player, 1, 2);
						QuickSpawnItem<GuardianHoneyPotion>(player, 1, 2);
						QuickSpawnItem<DeckJungle>(player, 1, 20);
						QuickSpawnItem<IvyChestCard>(player, 1, 5);
						QuickSpawnItem<WardenTortoise>(player, 1, 5);
					}
					break;
				case ItemID.CrimsonFishingCrate:
				case ItemID.CrimsonFishingCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<BloodCaller>(), 1),
								(ModContent.ItemType<CrimtaneCatalyst>(), 1),
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.FloatingIslandFishingCrate:
				case ItemID.FloatingIslandFishingCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<SunplateFlask>(), 1),
								(ModContent.ItemType<SkywareShield>(), 1)
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.OasisCrate:
				case ItemID.OasisCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<RuneOfHorus>(), 1),
								(ModContent.ItemType<DesertWarhammer>(), 1),
								(ModContent.ItemType<DesertStandard>(), 1),
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.LavaCrate:
				case ItemID.LavaCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								//(ModContent.ItemType<FireBatScepter>(), 1),
								(ModContent.ItemType<ShadowChestFlask>(), 1),
								(ModContent.ItemType<KeystoneOfTheConvent>(), 1),
								(ModContent.ItemType<ImpDiceCup>(), 1),
								(ModContent.ItemType<NightShield>(), 1),
								(ModContent.ItemType<HellRune>(), 1),
							}
						);
						QuickSpawnItem<GuardianRunePotion>(player, 1, 2);
					}
					break;

				case ItemID.FrozenCrate:
				case ItemID.FrozenCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<BlizzardInAVial>(), 1),
								(ModContent.ItemType<IceChestFlask>(), 1),
								(ModContent.ItemType<IceStandard>(), 1)
							},
							chanceDenominator: 2
						);
						QuickSpawnItem<PredatorIceFox>(player, 1, 5);
					}
					break;
				default:
					break;
			}
		}

		private static void OpenGoldenLockBox(Player player)
		{
			QuickSpawnRandomItemFromList(
				player: player,
				items: new()
				{
					(ModContent.ItemType<DungeonFlask>(), 1),
					(ModContent.ItemType<DungeonCatalyst>(), 1),
					(ModContent.ItemType<Rusalka>(), 1)
				},
				chanceDenominator: 2
			);
			QuickSpawnItem<TiamatRelic>(player, 1, 2);
			QuickSpawnItem<DeckBone>(player, 1, 40);
		}

		/* // For some reason, opening an obsidian lockbox doesn't call this method.
		private static void OpenLockBox(Player player, int arg)
		{
			switch (arg)
			{
				case ItemID.LockBox:
					QuickSpawnRandomItemFromList(
						player: player,
						items: new()
						{
							(ModContent.ItemType<SpiritedWater>(), 1),
							(ModContent.ItemType<DungeonFlask>(), 1),
							(ModContent.ItemType<DungeonCatalyst>(), 1),
							(ModContent.ItemType<Rusalka>(), 1)
						},
						chanceDenominator: 2
					);
					QuickSpawnItem<TiamatRelic>(player, 1, 2);
					QuickSpawnItem<DeckBone>(player, 1, 40);
					break;
				case ItemID.ObsidianLockbox:
					QuickSpawnRandomItemFromList(
						player: player,
						items: new()
						{
							(ModContent.ItemType<ShadowChestFlask>(), 1),
							(ModContent.ItemType<FireBatScepter>(), 1),
							(ModContent.ItemType<KeystoneOfTheConvent>(), 1),
							(ModContent.ItemType<ImpDiceCup>(), 1)
						},
						chanceDenominator: 2
					);
					break;
			}
		}
		*/

		private static void QuickSpawnRandomItemFromList(Player player, List<(int type, int stack)> items, int chanceDenominator = 1)
		{
			if (items == null || items.Count == 0) return;

			(int type, int stack) = items[Main.rand.Next(items.Count)];

			QuickSpawnItem(player, type, stack, chanceDenominator);
		}

		private static void QuickSpawnItem<T>(Player player, int stack = 1, int chanceDenominator = 1) where T : ModItem
			=> QuickSpawnItem(player, ModContent.ItemType<T>(), stack, chanceDenominator);

		private static void QuickSpawnItem(Player player, int type, int stack = 1, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;

			player.QuickSpawnItem(player.GetSource_OpenItem(type), type, stack);
		}
	}
}