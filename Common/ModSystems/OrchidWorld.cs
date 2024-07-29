using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;
using Terraria.IO;
using OrchidMod.Content.Shaman.Weapons;
using OrchidMod.Content.Gambler.Weapons.Cards;
using OrchidMod.Content.Alchemist.Weapons.Fire;
using OrchidMod.Content.Alchemist.Weapons.Water;
using OrchidMod.Content.Alchemist.Weapons.Nature;
using OrchidMod.Content.Alchemist.Weapons.Air;
using OrchidMod.Content.Gambler.Misc;
using OrchidMod.Content.Shaman.Accessories;
using OrchidMod.Content.Alchemist.Weapons.Catalysts;
using OrchidMod.Content.Gambler.Weapons.Chips;
using OrchidMod.Content.Alchemist.Accessories;
using OrchidMod.Content.Gambler.Decks;
using OrchidMod.Content.Gambler.Accessories;
using OrchidMod.Utilities;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using OrchidMod.Content.Guardian.Weapons.Runes;
using OrchidMod.Content.Guardian.Weapons.Shields;
using OrchidMod.Content.Shaman.Weapons.Hardmode;
using OrchidMod.Content.Guardian.Accessories;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Guardian.Weapons.Standards;

namespace OrchidMod.Common.ModSystems
{
	public class OrchidWorld : ModSystem
	{
		public static bool foundChemist = false;
		public static bool foundSlimeCard = false;

		public static float alchemistMushroomArmorProgress = 0.5f; // 0.5f -> 1f -> 0.5f -> ...

		public override void OnWorldLoad()
		{
			foundChemist = false;
			foundSlimeCard = false;
		}

		public override void SaveWorldData(TagCompound tag)/* Suggestion: Edit tag parameter rather than returning new TagCompound */
		{
			var orchidTags = new List<string>();

			if (foundChemist)
			{
				orchidTags.Add("chemist");
			}

			if (foundSlimeCard)
			{
				orchidTags.Add("slimecard");
			}

			//return new TagCompound
			//{
			//	["downed"] = downed,
			//};

			tag.Add("orchidTags", orchidTags);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var orchidTags = tag.GetList<string>("orchidTags");
			foundChemist = orchidTags.Contains("chemist");
			foundSlimeCard = orchidTags.Contains("slimecard");
		}

		/*
		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				foundChemist = flags[0];
				foundSlimeCard = flags[1];
			}
			else
			{
				Mod.Logger.WarnFormat("OrchidMod: Unknown loadVersion: {0}", loadVersion);
			}
		}
		*/

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = foundChemist;
			flags[1] = foundSlimeCard;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			foundChemist = flags[0];
		}

		public override void PreUpdateWorld()
		{
			// Mushroom Armor Lighting Progress and ... Shroomite Scepter
			{
				alchemistMushroomArmorProgress = (float)Math.Sin(Main.GlobalTimeWrappedHourly * (Math.PI / 2f)) * 0.25f + 0.75f;
			}
		}

		private void placeLilies()
		{
			for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.015); k++)
			{
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);
				int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY);

				if (!Framing.GetTileSafely(x, y).HasTile && !Framing.GetTileSafely(x + 1, y).HasTile &&
				!Framing.GetTileSafely(x, y - 1).HasTile && !Framing.GetTileSafely(x + 1, y - 1).HasTile)
				{
					if (Framing.GetTileSafely(x, y + 1).TileType == 60 && Framing.GetTileSafely(x + 1, y + 1).TileType == 60)
					{

						for (int w = 0; w < 2; w++)
						{
							for (int q = 0; q < 2; q++)
							{
								Tile tile = Framing.GetTileSafely(x + w, y - q);
								tile.ClearTile();
							}
						}
						WorldGen.PlaceTile(x, y, TileType<Content.Items.Materials.JungleLilyTile>(), style: WorldGen.genRand.Next(4));
					}
				}
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			// int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			// if (ShiniesIndex != -1) {
			// tasks.Insert(ShiniesIndex + 1, new PassLegacy("Static Quartz", placeQuartz));
			// }

			int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
			if (LivingTreesIndex != -1)
			{
				tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress, GameConfiguration gameConfiguration)
				{
					progress.Message = "Generating Jungle Lilies";
					placeLilies();
				}));
			}

			int ChestsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Chests"));
			if (ChestsIndex != -1)
			{
				tasks.Insert(ChestsIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress, GameConfiguration gameConfiguration)
				{
					// Get dungeon size field infos. These fields are private for some reason
					int MinX = GenVars.dMinX + 25;
					int MaxX = GenVars.dMaxX - 25;
					int MaxY = GenVars.dMaxY - 25;

					progress.Message = "Orchid Mod: Biome Chests";

					int rounds = 1;
					// Terra Custom Support
					// Type TerraCustom_Settings = typeof(Main).Assembly.GetType("Terraria.TerraCustom.Setting");
					// if (TerraCustom_Settings != null)
					// {
					// OrchidMod.mod.Logger.Info("Getting Biome Chest Set Amount From TerraCustom Settings");
					// object terraCustom_setting = typeof(Main).GetField("setting", BindingFlags.Static | BindingFlags.Public).GetValue(null);
					// rounds = (int)(TerraCustom_Settings.GetProperty("BiomeChestSets", BindingFlags.Instance | BindingFlags.Public).GetValue(terraCustom_setting, null));
					// }

					/* REIMPLEMENT THIS
					for (int i = 0; i < rounds; i++)
					{
						Chest chest = null;
						int attempts = 0;
						while (chest == null && attempts < 10000)
						{
							attempts++;
							int x = WorldGen.genRand.Next(MinX, MaxX);
							int y = WorldGen.genRand.Next((int)Main.worldSurface, MaxY);
							if (Main.wallDungeon[Main.tile[x, y].WallType] && !Main.tile[x, y].HasTile)
							{
								while (y < Main.maxTilesY - 210)
								{
									if (!WorldGen.SolidTile(x, y))
									{
										y++;
										continue;
									}

									int chestIndex = WorldGen.PlaceChest(x - 1, y - 1, (ushort)TileType<Content.Items.Placeables.ShamanBiomeChestTile>(), false, 1);

									if (chestIndex < 0)
									{
										break;
									}

									chest = Main.chest[chestIndex];
								}
							}
						}
					}
					*/
				}));
			}
		}

		public bool placeInChest(Chest chest, int itemToPlace, int quantity)
		{
			if (chest != null)
			{
				for (int inventoryIndex = 39; inventoryIndex > 1; inventoryIndex--)
				{
					if (chest.item[inventoryIndex - 1].type != ItemID.None)
					{
						chest.item[inventoryIndex].SetDefaults(chest.item[inventoryIndex - 1].type);
						chest.item[inventoryIndex].stack = chest.item[inventoryIndex - 1].stack;
					}
				}
				chest.item[1].SetDefaults(itemToPlace);
				if (quantity > 1)
				{
					chest.item[1].stack = quantity;
				}
			}
			else
			{
				return false;
			}
			return true;
		}

		public override void PostWorldGen() // TODO : Better chest spawn & item guarantee code
		{
			List<ChestLoot> chestLoots = new List<ChestLoot>();
			List<Chest> usedChests = new List<Chest>();

			// Surface Chests
			chestLoots.Add(new ChestLoot(ItemType<EmbersCard>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<AdornedBranch>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<EmberVial>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<Warhammer>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<GuideShield>(), ChestType.SurfaceWooden, 20));

			// Water Chests
			chestLoots.Add(new ChestLoot(ItemType<BubbleCard>(), ChestType.Water, 20));
			chestLoots.Add(new ChestLoot(ItemType<SeafoamVial>(), ChestType.Water, 20));

			// Living Tree Chests
			chestLoots.Add(new ChestLoot(ItemType<SapCard>(), ChestType.LivingTree, 50));
			chestLoots.Add(new ChestLoot(ItemType<LivingSapVial>(), ChestType.LivingTree, 50));
			chestLoots.Add(new ChestLoot(ItemType<LivingRune>(), ChestType.LivingTree, 50));

			// Gold Chests
			chestLoots.Add(new ChestLoot(ItemType<GoldChestCard>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<EnchantedScepter>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<EnchantedRune>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<EnchantedPavise>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<CloudInAVial>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<FartInAVial>(), ChestType.Gold, 1, needToPlace: 0, ignoreChestLimit: true));
			chestLoots.Add(new ChestLoot(ItemType<DeckEnchanted>(), ChestType.Gold, 5, needToPlace: 0, ignoreChestLimit: true));

			// Ice Chests
			chestLoots.Add(new ChestLoot(ItemType<BlizzardInAVial>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<IceChestFlask>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<IceChestCard>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<AvalancheScepter>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<IceStandard>(), ChestType.Ice, 20));

			// Dungeon Chests (Locked)
			chestLoots.Add(new ChestLoot(ItemType<TiamatRelic>(), ChestType.DungeonLocked, 50, 1, 3, true));
			chestLoots.Add(new ChestLoot(ItemType<SpiritedWater>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<DungeonFlask>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<DungeonCatalyst>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<Rusalka>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<DeckBone>(), ChestType.DungeonLocked, 5, needToPlace: 0, ignoreChestLimit: true));

			// Shadow Chests
			chestLoots.Add(new ChestLoot(ItemType<FireBatScepter>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<ShadowChestFlask>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<KeystoneOfTheConvent>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<ImpDiceCup>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<NightShield>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<HellRune>(), ChestType.Shadow, 30));

			// Jungle Chests
			chestLoots.Add(new ChestLoot(ItemType<DeepForestCharm>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<IvyChestCard>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<BloomingBud>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<BundleOfClovers>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<JungleGauntlet>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<DeckJungle>(), ChestType.Ivy, 5, needToPlace: 0, ignoreChestLimit: true));

			// Sky Island Chests
			chestLoots.Add(new ChestLoot(ItemType<SunplateFlask>(), ChestType.SkyIsland, 30));
			chestLoots.Add(new ChestLoot(ItemType<SkywareShield>(), ChestType.SkyIsland, 30));

			// Underground Desert Chests
			chestLoots.Add(new ChestLoot(ItemType<RuneOfHorus>(), ChestType.Sandstone, 20));
			chestLoots.Add(new ChestLoot(ItemType<StormWarhammer>(), ChestType.Sandstone, 20));

			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				HandleSpecialChests(chest);

				List<ChestLoot> possibleLoot = new List<ChestLoot>();
				foreach (ChestLoot loot in chestLoots)
				{
					if (loot.ValidChest(chest) && loot.RollPlace())
					{
						if (!usedChests.Contains(chest) && !loot.ignoreChestLimit)
							possibleLoot.Add(loot);

						if (loot.ignoreChestLimit)
						{
							placeInChest(chest, loot.itemType, loot.quantity);
							loot.placed++;
						}
					}
				}

				ChestLoot chosenLoot = null;
				int lowestPriority = int.MaxValue;
				foreach (ChestLoot loot in possibleLoot)
				{
					if (loot.placed < lowestPriority)
					{
						chosenLoot = loot;
						lowestPriority = loot.placed;
					}
				}

				if (chosenLoot != null)
				{
					placeInChest(chest, chosenLoot.itemType, chosenLoot.quantity);
					chosenLoot.placed++;
					usedChests.Add(chest);
				}
			}

			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				foreach (ChestLoot loot in chestLoots)
				{
					if (loot.ValidChest(chest) && loot.placed < loot.needToPlace)
					{
						placeInChest(chest, loot.itemType, loot.quantity);
						loot.placed++;
					}
				}
			}
		}

		public void HandleSpecialChests(Chest chest)
		{
			/*
			if (chest != null && Main.tile[chest.x, chest.y].TileType == (ushort)TileType<Content.Items.Placeables.MinersLockboxTile>())
			{
				int[] specialItemPoll = {49, 50, 53, 54, 55, 975, 997, 930, ItemType<Content.Shaman.Weapons.EnchantedScepter>()
					, ItemType<Content.Alchemist.Weapons.Air.CloudInAVial>(), ItemType<Content.Gambler.Weapons.Cards.GoldChestCard>()};
				int rand = Main.rand.Next(specialItemPoll);

				chest.item[0].SetDefaults(Mod.Find<ModItem>("HauntedCandle").Type);
				placeInChest(chest, 71, Main.rand.Next(80, 99)); // Copper Coins
				placeInChest(chest, 72, Main.rand.Next(80, 99)); // Silver Coins
				placeInChest(chest, 28, Main.rand.Next(3, 8)); // Healing Pots
				placeInChest(chest, 2, Main.rand.Next(5, 15)); // Iron Bars
				placeInChest(chest, 166, Main.rand.Next(10, 20)); // Bombs
				placeInChest(chest, 965, Main.rand.Next(50, 100)); // Ropes
				placeInChest(chest, ItemType<Content.Gambler.Weapons.Cards.DetonatorCard>(), 1); // Ropes

				if (rand == 930) placeInChest(chest, 931, Main.rand.Next(21) + 30); // Flares
				placeInChest(chest, rand, 1);
			}
			*/

			if (chest != null && Main.tile[chest.x, chest.y].TileType == (ushort)TileType<Content.Items.Placeables.ShamanBiomeChestTile>())
			{
				chest.item[0].SetDefaults(ItemType<ShroomiteScepter>());
				chest.item[1].SetDefaults(183); // Glowing Mushroom
				chest.item[1].stack = Main.rand.Next(10) + 20;
				chest.item[2].SetDefaults(188); // Healing Potion
				chest.item[2].stack = Main.rand.Next(5) + 3;
				chest.item[3].SetDefaults(298); // Shine Potion
				chest.item[3].stack = Main.rand.Next(3) + 1;
				chest.item[4].SetDefaults(289); // Regeneration Potion
				chest.item[4].stack = Main.rand.Next(3) + 1;
			}

		}
	}

	public class ChestLoot
	{
		public int itemType;
		public ChestType chestType;
		public int quantity;
		public int needToPlace;
		public int chance;
		public int placed;
		public bool ignoreChestLimit;
		public bool RollPlace() => Main.rand.Next(100) < chance;

		public ChestLoot(int itemType, ChestType chestType, int chance, int quantity = 1, int needToPlace = 1, bool ignoreChestLimit = false)
		{
			this.itemType = itemType;
			this.quantity = quantity;
			this.chestType = chestType;
			this.needToPlace = needToPlace;
			this.ignoreChestLimit = ignoreChestLimit;
			this.chance = chance;
			placed = 0;
		}

		public bool ValidChest(Chest chest)
		{
			if (chest == null || !(Framing.GetTileSafely(chest.x, chest.y) is Tile tile)) return false;
			if (!tile.HasTile || !TileID.Sets.IsAContainer[tile.TileType]) return false;

			switch (chestType)
			{
				case ChestType.SurfaceWooden:
					bool flag = true;
					flag &= tile.TileType == TileID.Containers;
					flag &= tile.TileFrameX == 0 * 36;
					flag &= !tile.WallType.Between(WallID.BlueDungeonSlabUnsafe, WallID.GreenDungeonTileUnsafe);
					flag &= !tile.WallType.Between(WallID.BlueDungeonUnsafe, WallID.PinkDungeonUnsafe);
					return flag;
				case ChestType.Water:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 17 * 36;
				case ChestType.LivingTree:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 12 * 36;
				case ChestType.SkyIsland:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 13 * 36;
				case ChestType.Gold:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 1 * 36 && tile.WallType != WallID.SandstoneBrick;
				case ChestType.DungeonLocked:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 2 * 36;
				case ChestType.Shadow:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 4 * 36;
				case ChestType.Ice:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 11 * 36;
				case ChestType.Ivy:
					return tile.TileType == TileID.Containers && tile.TileFrameX == 10 * 36;
				case ChestType.Sandstone:
					return tile.TileType == TileID.Containers2 && tile.TileFrameX == 10 * 36;
				default:
					return false;
			}
		}
	}

	public enum ChestType
	{
		SurfaceWooden,
		Gold,
		DungeonLocked,
		Shadow,
		Ivy,
		Sandstone,
		Water,
		LivingTree,
		SkyIsland,
		Ice
	}
}