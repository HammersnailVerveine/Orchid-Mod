using OrchidMod.Alchemist.Accessories;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Catalysts;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Content.Items.Materials;
using OrchidMod.Gambler.Accessories;
using OrchidMod.Gambler.Decks;
using OrchidMod.Gambler.Misc;
using OrchidMod.Gambler.Weapons.Cards;
using OrchidMod.Gambler.Weapons.Chips;
using OrchidMod.Gambler.Weapons.Dice;
using OrchidMod.General.Items.Misc;
using OrchidMod.General.Items.Vanity;
using OrchidMod.Shaman.Accessories;
using OrchidMod.Shaman.Misc.Thorium;
using OrchidMod.Shaman.Weapons;
using OrchidMod.Shaman.Weapons.Hardmode;
using OrchidMod.Shaman.Weapons.Thorium;
using OrchidMod.Shaman.Weapons.Thorium.Hardmode;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.Globals.Items
{
	public partial class OrchidGlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			switch (context)
			{
				case "bossBag": OpenBossBag(player, arg); break;
				case "crate": OpenCrate(player, arg); break;
				case "lockBox": OpenGoldenLockBox(player); break;
			}
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
								(ModContent.ItemType<BeeSeeker>(), 1),
								(ModContent.ItemType<WaxyVial>(), 1),
								(ModContent.ItemType<QueenBeeFlask>(), 1)
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.SkeletronBossBag:
					{
						QuickSpawnItem<SkeletronCard>(player, 1, 1);
					}
					break;
				case ItemID.WallOfFleshBossBag:
					{
						QuickSpawnItem<ShamanEmblem>(player, 1, 4);
						QuickSpawnItem<OrchidEmblem>(player, 1, 1);
					}
					break;
				case ItemID.PlanteraBossBag:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<BulbScepter>(), 1),
								(ModContent.ItemType<FloralStinger>(), 1),
							},
							chanceDenominator: 3
						);
						QuickSpawnItem<OrnateOrchid>(player, 1, 20);
					}
					break;
				case ItemID.GolemBossBag:
					{
						QuickSpawnItem<SunRay>(player, 1, 6);
					}
					break;
				case ItemID.MoonLordBossBag:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<Nirvana>(), 1),
								(ModContent.ItemType<TheCore>(), 1)
							},
							chanceDenominator: 5
						);
					}
					break;
			}

			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod == null) goto SkipThorium;

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
						QuickSpawnItem<AdornedBranch>(player, 1, 45);
						QuickSpawnItem<EmberVial>(player, 1, 45);
						QuickSpawnItem<EmbersCard>(player, 1, 45);
						QuickSpawnItem<TsunamiInAVial>(player, 1, 45);
					}
					break;
				case ItemID.IronCrate:
				case ItemID.IronCrateHard:
					{
						QuickSpawnItem<BubbleCard>(player, 1, 20);
						QuickSpawnItem<SeafoamVial>(player, 1, 20);
						QuickSpawnItem<TsunamiInAVial>(player, 1, 20);
						QuickSpawnItem<DeckEnchanted>(player, 1, 20);
					}
					break;
				case ItemID.CorruptFishingCrate:
				case ItemID.CorruptFishingCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<ShadowWeaver>(), 1),
								(ModContent.ItemType<DemoniteCatalyst>(), 1),
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.JungleFishingCrate:
				case ItemID.JungleFishingCrateHard:
					{
						if (Main.rand.Next(5) < 3) // 0.6
						{
							QuickSpawnRandomItemFromList(
								player: player,
								items: new()
								{
									(ModContent.ItemType<DeepForestCharm>(), 1),
									(ModContent.ItemType<BloomingBud>(), 1),
									(ModContent.ItemType<BundleOfClovers>(), 1),
								},
								chanceDenominator: 1
							);
						}

						QuickSpawnItem<JungleLily>(player, 1, 2);
						QuickSpawnItem<DeckJungle>(player, 1, 20);
						QuickSpawnItem<IvyChestCard>(player, 1, 5);
					}
					break;
				case ItemID.CrimsonFishingCrate:
				case ItemID.CrimsonFishingCrateHard:
					{
						QuickSpawnRandomItemFromList(
							player: player,
							items: new()
							{
								(ModContent.ItemType<BloodCaller>(), 1),
								(ModContent.ItemType<CrimtaneCatalyst>(), 1),
							},
							chanceDenominator: 2
						);
					}
					break;
				case ItemID.FloatingIslandFishingCrate:
				case ItemID.FloatingIslandFishingCrateHard:
					{
						QuickSpawnItem<SunplateFlask>(player, 1, 4);
					}
					break;
				case ItemID.OasisCrate:
				case ItemID.OasisCrateHard:
					{
						QuickSpawnItem<RuneOfHorus>(player, 1, 8);
					}
					break;
			}
		}

		private static void OpenGoldenLockBox(Player player)
		{
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
		}

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