using OrchidMod.Tiles.Ores;
using OrchidMod.WorldgenArrays;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;
using Terraria.IO;
using OrchidMod.Shaman.Weapons;
using OrchidMod.Gambler.Weapons.Cards;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Gambler.Misc;
using OrchidMod.Shaman.Accessories;
using OrchidMod.Alchemist.Weapons.Catalysts;
using OrchidMod.Gambler.Weapons.Chips;
using OrchidMod.Alchemist.Accessories;
using OrchidMod.Gambler.Decks;
using OrchidMod.Gambler.Accessories;
using OrchidMod.Utilities;
using OrchidMod.Guardian.Weapons.Warhammers;
using OrchidMod.Guardian.Weapons.Runes;
using OrchidMod.Guardian.Weapons.Shields;

namespace OrchidMod
{
	public class OrchidWorld : ModSystem
	{
		private static int RLenght = 5;
		private static int RHeight = 2;
		private static int MSMinPosX;
		private static int MSMinPosY;
		private static int RMinPosX;
		private static int RMinPosY;

		private int[,] mineshaft = new int[OrchidMSarrays.MSLenght, OrchidMSarrays.MSHeight];
		private int[,] minishaft1 = new int[5, 3];
		private int[,] minishaft2 = new int[5, 3];
		private int[,] ruins = new int[RLenght, RHeight];


		private int minishaft1X;
		private int minishaft1Y;
		private int minishaft2X;
		private int minishaft2Y;

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

		public void PlaceMSRoomTiles(int i, int j, int[,] MS, int[,] MSWall)
		{
			int barRand = Main.rand.Next(6);
			int wallrand = Main.rand.Next(5);
			bool trapRoom = (Main.rand.Next(5) == 0);
			bool normalRoom = (!trapRoom && (Main.rand.Next(3) == 0));
			for (int y = 0; y < MS.GetLength(0); y++)
			{
				for (int x = 0; x < MS.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					int allowPlatforms = Main.rand.Next(3);
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						tile.ClearTile();
						int allowPlatforms2 = Main.rand.Next(3); // One room out of 3 will have a plaftorm above every air block instead of wood.
						int forceWood = Main.rand.Next(2);
						switch (MS[y, x])
						{
							case 1:
								bool canSpawnTile = true;
								if (trapRoom)
								{
									for (int w = 0; w < 3; w++)
									{
										if (Framing.GetTileSafely(k, l - 1 + w).HasTile)
										{
											canSpawnTile = false;
										}
									}
									if (canSpawnTile)
									{
										WorldGen.PlaceTile(k, l, TileType<Tiles.Ambient.FragileWood>());
									}
								}
								else if (!normalRoom)
								{
									if (Framing.GetTileSafely(k, l + 1).HasTile == false)
									{
										if (allowPlatforms == 0)
										{
											WorldGen.PlaceTile(k, l, 19); // One room out of 3 will have a plaftorm above every air block instead of wood.
											canSpawnTile = true;
										}
										if (allowPlatforms != 0 && allowPlatforms2 == 0)
										{
											WorldGen.PlaceTile(k, l, 19); // Other 2 rooms, have a 1 in 3 chance to get every wood block above air to be a platform
											canSpawnTile = true;
										}
									}
								}

								if (!canSpawnTile || normalRoom)
								{ // If neither platform or trap could be placed
									if (Main.rand.Next(10) != 0)
									{
										tile.TileType = 30; //Wood (and every wood block has a 1/10 chance not to be spawned)
										if (Framing.GetTileSafely(k, l + 1).TileType == TileID.Stone || Framing.GetTileSafely(k, l - 1).TileType == TileID.Stone) tile.TileType = TileID.Stone;
										if (Framing.GetTileSafely(k, l + 1).TileType == TileID.Granite || Framing.GetTileSafely(k, l - 1).TileType == TileID.Granite) tile.TileType = TileID.Granite;
										if (Framing.GetTileSafely(k, l + 1).TileType == TileID.Marble || Framing.GetTileSafely(k, l - 1).TileType == TileID.Marble) tile.TileType = TileID.Marble;
										if (Framing.GetTileSafely(k, l + 1).TileType == TileID.IceBlock || Framing.GetTileSafely(k, l - 1).TileType == TileID.IceBlock) tile.TileType = TileID.IceBlock;
										if (forceWood == 0) tile.TileType = 30;
									}
								}
								tile.HasTile = true;
								break;
							case 3:
								tile.TileType = TileID.Cobweb;
								tile.HasTile = true;
								break;
							case 5:
								tile.TileType = TileID.Chain;
								tile.HasTile = true;
								break;
							case 6:
								WorldGen.PlaceObject(k, l, 239, true, barRand); // Bars
								tile.HasTile = true;
								break;
							case 8:
								tile.TileType = 124; //Wooden Beam
								tile.HasTile = true;
								break;
							case 9:
								tile.TileType = TileID.Rope;
								tile.HasTile = true;
								break;
							case 10: // Same as 1, without that rand bullcrap. Spawns a block, that's it.
								tile.TileType = 30; //Wood (and every wood block has a 1/10 chance not to be spawned)
								if (Framing.GetTileSafely(k, l + 1).TileType == TileID.Stone || Framing.GetTileSafely(k, l - 1).TileType == TileID.Stone) tile.TileType = TileID.Stone;
								if (Framing.GetTileSafely(k, l + 1).TileType == TileID.Granite || Framing.GetTileSafely(k, l - 1).TileType == TileID.Granite) tile.TileType = TileID.Granite;
								if (Framing.GetTileSafely(k, l + 1).TileType == TileID.Marble || Framing.GetTileSafely(k, l - 1).TileType == TileID.Marble) tile.TileType = TileID.Marble;
								if (Framing.GetTileSafely(k, l + 1).TileType == TileID.IceBlock || Framing.GetTileSafely(k, l - 1).TileType == TileID.IceBlock) tile.TileType = TileID.IceBlock;
								if (forceWood == 0) tile.TileType = 30;
								tile.HasTile = true;
								break;
							case 11:
								tile.TileType = 0; // Dirt
								tile.HasTile = true;
								break;
							case 12:
								tile.TileType = TileID.Stone;
								tile.HasTile = true;
								break;
							case 13:
								tile.TileType = 67; // Amethyst
								tile.HasTile = true;
								break;
							case 14:
								tile.TileType = 178; // Amethyst
								tile.HasTile = true;
								break;
							case 16:
								if (Main.rand.Next(3) > 1) WorldGen.PlaceObject(k, l - 1, 330); // Copper Coins
								else
								{
									if (Main.rand.Next(3) > 1) WorldGen.PlaceObject(k, l - 1, 331); // Silver Coins
									else WorldGen.PlaceObject(k, l - 1, 332); // Gold Coins
								}
								break;
							default:
								break;
						}

						switch (MSWall[y, x])
						{
							case 1:
								if (wallrand == 0) if (Main.rand.Next(5) != 0) tile.WallType = 27; //PlankedWall
								if (wallrand == 1) if (Main.rand.Next(2) != 0) tile.WallType = 2; //Dirt
								if (wallrand == 2) if (Main.rand.Next(2) != 0) tile.WallType = 1; //Stone
								if (wallrand == 3)
								{
									if (Main.rand.Next(5) >= 3) tile.WallType = 27; //PlankedWall + Stone
									else tile.WallType = 1;
								}
								if (wallrand == 4)
								{ // mash
									if (Main.rand.Next(3) == 0) tile.WallType = 27; //PlankedWall + Stone
									if (Main.rand.Next(3) == 0) tile.WallType = 2;
									if (Main.rand.Next(3) == 0) tile.WallType = 1;
								}
								break;
							default:
								break;
						}
					}
				}
			}
		}

		public void PlaceMSRoomFurnitures(int i, int j, int[,] MS, int[,] MSWall)
		{
			int barRand = Main.rand.Next(6);
			int wallrand = Main.rand.Next(5);
			for (int y = 0; y < MS.GetLength(0); y++)
			{
				for (int x = 0; x < MS.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (MS[y, x])
						{
							case 2:
								WorldGen.PlaceTile(k, l, 19); // Platform
								tile.HasTile = true;
								break;
							case 3:
								tile.TileType = TileID.Cobweb;
								tile.HasTile = true;
								break;
							case 5:
								tile.TileType = TileID.Chain;
								tile.HasTile = true;
								break;
							case 6:
								WorldGen.PlaceObject(k, l, 239, true, barRand); // Bars
								tile.HasTile = true;
								break;
							case 14:
								tile.TileType = 178; // Amethyst
								tile.HasTile = true;
								break;
							case 15:
								WorldGen.PlaceObject(k, l, 215); // campfire
								break;
							case 16:
								if (Main.rand.Next(3) > 1) WorldGen.PlaceObject(k, l - 1, 330); // Copper Coins
								else
								{
									if (Main.rand.Next(3) > 1) WorldGen.PlaceObject(k, l - 1, 331); // Silver Coins
									else WorldGen.PlaceObject(k, l - 1, 332); // Gold Coins
								}
								break;
							default:
								break;
						}
					}
				}
			}
		}

		public void PlaceMSFurnitures(int i, int j, int[,] MS, int[,] MSWall)
		{
			Tile tile;

			for (int y = 0; y < MS.GetLength(0); y++)
			{
				for (int x = 0; x < MS.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						switch (MS[y, x])
						{
							case 4:
								tile = Framing.GetTileSafely(k, l - 1);
								if (tile.HasTile == false)
								{
									tile.TileType = 30;
									tile.HasTile = true;
								}
								WorldGen.PlaceObject(k, l, 42, true, 6); // 33 = candle /42 = Lantern
								break;
							case 7:
								for (int w = 0; w < 3; w++)
								{
									for (int q = 0; q < 3; q++)
									{
										tile = Framing.GetTileSafely(k + w - 1, l + q - 1);
										tile.ClearEverything();
										tile.WallType = 27;
									}
								}
								WorldGen.PlaceObject(k, l, ModContent.TileType<Content.Items.Tools.MineshaftPickaxeTile>());
								break;
							case 17:
								for (int w = 0; w < 2; w++)
								{
									for (int q = 0; q < 2; q++)
									{
										tile = Framing.GetTileSafely(k - w, l - q);
										if (tile.TileType == 314)
										{
											break;
										}
										else
										{
											tile.ClearTile();
										}
									}
								}
								WorldGen.PlaceObject(k, l, ModContent.TileType<Tiles.Ambient.MineshaftCrate>());
								break;
							case 18:
								for (int w = 0; w < 2; w++)
								{
									for (int q = 0; q < 2; q++)
									{
										tile = Framing.GetTileSafely(k - w, l - q);
										if (tile.TileType == 314)
										{
											break;
										}
										else
										{
											tile.ClearTile();
										}
									}
								}
								WorldGen.PlaceTile(k, l + 1, 19);
								WorldGen.PlaceTile(k + 1, l + 1, 19);
								WorldGen.PlaceChest(k, l, (ushort)ModContent.TileType<Content.Items.Placeables.MinersLockboxTile>(), false, 0);
								break;
							case 19:
								WorldGen.PlaceTile(k, l, 83, true, true, -1, 2);
								/*
								tile = Framing.GetTileSafely(k, l);
								tile.TileType = 84;
								tile.HasTile = true;
								tile.
									*/
								break;
							case 20:
								for (int w = 0; w < 3; w++)
								{
									for (int q = 0; q < 3; q++)
									{
										tile = Framing.GetTileSafely(k + w - 1, l + q - 1);
										tile.ClearEverything();
										tile.WallType = 27;
									}
								}
								WorldGen.PlaceObject(k, l, ModContent.TileType<Tiles.Ambient.MineshaftHookTile>());
								break;
							case 21:
								for (int w = 0; w < 3; w++)
								{
									for (int q = 0; q < 2; q++)
									{
										tile = Framing.GetTileSafely(k - w + 1, l - q);
										tile.ClearTile();
									}
								}
								WorldGen.PlaceObject(k, l, ModContent.TileType<Tiles.Ambient.MineshaftToolboxTile>());
								break;
							case 22:
								for (int w = 0; w < 2; w++)
								{
									for (int q = 0; q < 2; q++)
									{
										tile = Framing.GetTileSafely(k - w, l - q);
										tile.ClearTile();
									}
								}
								WorldGen.PlaceObject(k, l, ModContent.TileType<Tiles.Ambient.MineshaftStaticTile>());
								break;
							default:
								break;
						}
					}
				}
			}
		}

		// private int[,] GenerateRArray(int RLenght, int RHeight) {
		// int [,] ruins = new int[RLenght,RHeight];
		// for (int i = 0; i < RLenght ; i++ ) { // FILL GRID WITH NORMAL ROOMS
		// for (int j = 0; j < RHeight ; j++ ) {
		// ruins[i,j] = 1;
		// }
		// }

		// for (int j = 0; j < RHeight ; j++ ) { // STAIR ROOMS
		// int rand = Main.rand.Next((RLenght - 2) + 1);
		// ruins[rand,j] = 2;
		// if (j > 0) ruins[rand, j-1] = 0;
		// }
		// return ruins;
		// }

		private int[,] GenerateSmallMSArray(int MSLenght, int MSHeight)
		{
			int[,] mineshaft = new int[MSLenght, MSHeight];
			int treasureNumber = 1; // Scale with size ? / CAN BE CHANGED
			int randL;
			int randH;
			bool placed;
			int tries;
			int sidesrand = 1;
			for (int i = 0; i < MSLenght; i++)
			{ // FILL GRID WITH NORMAL ROOMS
				for (int j = 0; j < MSHeight; j++)
				{
					mineshaft[i, j] = 1;
				}
			}

			for (int i = 0; i < MSHeight; i++)
			{ // DELETE A RANDOM AMOUNT OF ROOMS ON SIDES
				for (int j = 0; j < Main.rand.Next(sidesrand); j++)
				{
					mineshaft[j, i] = 0;
				}
				for (int j = MSLenght - 1; j > (MSLenght - Main.rand.Next(sidesrand) - 1); j--)
				{
					mineshaft[j, i] = 0;
				}
			}

			for (int i = 0; i < MSHeight - 1; i++)
			{ // PLACE STAIRS
				tries = 0;
				placed = false;
				while (placed == false && tries < 20)
				{
					randL = (Main.rand.Next(MSLenght - 2) + 1);
					tries++;
					if (mineshaft[randL, i] == 1 && mineshaft[randL, i + 1] == 1)
					{
						mineshaft[randL, i] = 2;
						mineshaft[randL, i + 1] = 0;
						placed = true;
					}
				}
			}

			for (int i = 0; i < MSHeight; i++)
			{  // REPLACE RIGHT MOST ROOM WITH "RIGHT END" ROOM
				placed = false;
				int j = MSLenght - 1;
				while (placed == false)
				{
					if (mineshaft[j, i] == 1)
					{
						mineshaft[j, i] = 4;
						placed = true;
					}
					if (mineshaft[j, i] == 2)
					{
						mineshaft[j + 1, i] = 4;
						placed = true;
					}
					j--;
				}
			}
			for (int i = 0; i < MSHeight; i++)
			{  // REPLACE LEFT MOST ROOM WITH "LEFT END" ROOM
				placed = false;
				int j = 0;
				while (placed == false)
				{
					if (mineshaft[j, i] == 1)
					{
						mineshaft[j, i] = 8;
						placed = true;
					}
					if (mineshaft[j, i] == 2)
					{
						mineshaft[j - 1, i] = 8;
						placed = true;
					}
					j++;
				}
			}

			while (treasureNumber > 0)
			{ // PLACE TREASURE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight - 1);
				if (mineshaft[randL, randH] == 1)
				{
					treasureNumber--;
					mineshaft[randL, randH] = 3;
				}
			}
			return mineshaft;
		}

		private int[,] GenerateMSArray(int MSLenght, int MSHeight)
		{
			int[,] mineshaft = new int[MSLenght, MSHeight];
			int treasureNumber = 3; // Scale with size ? / CAN BE CHANGED
			int campfireNumber = 2; // same / CAN BE CHANGED
			int bigStairsNumber = 2; // same ? DO NOT CHANGE FOR NOW
			int bossNumber = 1; // CAN BE CHANGED
			int gemNumber = 1; // CAN BE CHANGED - don't go over MSHeight-1
			int hookNumber = 1; // CAN BE CHANGED
			int staticNumber = 0; // CAN BE CHANGED
			int toolboxNumber = 2; // CAN BE CHANGED
			int randL;
			int randH;
			bool placed;
			int tries;
			int lockedvalue = -1;
			int sidesrand = 5;
			for (int i = 0; i < MSLenght; i++)
			{ // FILL GRID WITH NORMAL ROOMS
				for (int j = 0; j < MSHeight; j++)
				{
					mineshaft[i, j] = 1;
				}
			}
			for (int i = 0; i < MSHeight; i++)
			{ // DELETE A RANDOM AMOUNT OF ROOMS ON SIDES
				for (int j = 0; j < Main.rand.Next(sidesrand); j++)
				{
					mineshaft[j, i] = 0;
				}
				for (int j = MSLenght - 1; j > (MSLenght - Main.rand.Next(sidesrand) - 1); j--)
				{
					mineshaft[j, i] = 0;
				}
			}
			for (int i = 0; i < MSHeight - 1; i++)
			{ // PLACE STAIRS
				tries = 0;
				placed = false;
				while (placed == false && tries < 20)
				{
					randL = (Main.rand.Next(MSLenght - 4) + 2);
					tries++;
					if (mineshaft[randL, i] == 1 && mineshaft[randL, i + 1] == 1)
					{
						mineshaft[randL, i] = 2;
						mineshaft[randL, i + 1] = 0;
						placed = true;
					}
				}
			}
			for (int i = 0; i < MSHeight; i++)
			{  // REPLACE RIGHT MOST ROOM WITH "RIGHT END" ROOM
				placed = false;
				int j = MSLenght - 1;
				while (placed == false)
				{
					if (mineshaft[j, i] == 1)
					{
						mineshaft[j, i] = 4;
						placed = true;
					}
					if (mineshaft[j, i] == 2)
					{
						mineshaft[j + 1, i] = 4;
						placed = true;
					}
					j--;
				}
			}
			for (int i = 0; i < MSHeight; i++)
			{  // REPLACE LEFT MOST ROOM WITH "LEFT END" ROOM
				placed = false;
				int j = 0;
				while (placed == false)
				{
					if (mineshaft[j, i] == 1)
					{
						mineshaft[j, i] = 8;
						placed = true;
					}
					if (mineshaft[j, i] == 2)
					{
						mineshaft[j - 1, i] = 8;
						placed = true;
					}
					j++;
				}
			}
			for (int i = 0; i < bigStairsNumber; i++)
			{ // PLACE BIG STAIRS
				tries = 0;
				placed = false;
				while (placed == false && tries < 20)
				{
					randL = (Main.rand.Next(MSLenght - 4) + 2);
					randH = (Main.rand.Next(MSHeight - 4) + 1);
					while (randH == lockedvalue || randH == lockedvalue + 1) randH = (Main.rand.Next(MSHeight - 4) + 1);
					tries++;
					if (mineshaft[randL, randH] == 1 && mineshaft[randL, randH + 2] == 1)
					{
						mineshaft[randL, randH] = 5;
						mineshaft[randL, randH + 1] = 0;
						mineshaft[randL, randH + 2] = 0;
						lockedvalue = randH;
						placed = true;
					}
				}
			}
			while (campfireNumber > 0)
			{ // PLACE CAMPFIRE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL, randH] == 1 && mineshaft[randL + 1, randH] == 1)
				{
					campfireNumber--;
					mineshaft[randL, randH] = 6;
					mineshaft[randL + 1, randH] = 0;
				}
			}
			while (gemNumber > 0)
			{ // PLACE GEMSTONE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL, randH] == 4)
				{
					gemNumber--;
					mineshaft[randL, randH] = 7;
				}
			}
			while (bossNumber > 0)
			{ // PLACE BOSS ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL, randH] == 1 && mineshaft[randL + 1, randH] == 1 && mineshaft[randL + 2, randH] == 1)
				{
					bossNumber--;
					mineshaft[randL, randH] = 9;
					mineshaft[randL + 1, randH] = 0;
					mineshaft[randL + 2, randH] = 0;
				}
			}
			while (treasureNumber > 0)
			{ // PLACE TREASURE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight - 1);
				if (mineshaft[randL, randH] == 1)
				{
					treasureNumber--;
					mineshaft[randL, randH] = 3;
				}
			}
			while (hookNumber > 0)
			{ // PLACE HOOK ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight - 1);
				if (mineshaft[randL, randH] == 1)
				{
					hookNumber--;
					mineshaft[randL, randH] = 10;
				}
			}
			while (staticNumber > 0)
			{ // PLACE STATIC NODE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL, randH] == 1)
				{
					staticNumber--;
					mineshaft[randL, randH] = 12;
				}
			}
			while (toolboxNumber > 0)
			{ // PLACE TOOLBOX ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL, randH] == 1)
				{
					toolboxNumber -= 1 + Main.rand.Next(2);
					mineshaft[randL, randH] = 11;
				}
			}
			return mineshaft;
		}

		// public void PlaceRuins(int[,] ruins, int RLenght, int RHeight, int PosX, int MinPosY) {
		// int PosY = MinPosY; // same
		// for (int i = 0; i < RLenght ; i++ ) {
		// for (int j = 0; j < RHeight ; j++ ) {
		// if (ruins[i,j] == 1) {
		// PlaceRRoom(PosX, PosY, OrchidRarrays.Rshape1, OrchidRarrays.RWall1);
		// }
		// if (ruins[i,j] == 2) {
		// PlaceRRoom(PosX, PosY - 16, OrchidRarrays.REntrance, OrchidRarrays.RWall2);
		// }
		// PosY += 14;
		// }
		// PosY = MinPosY;
		// PosX += 15;
		// }
		// }

		public void PlaceFossil(int i, int j, int[,] fossil)
		{
			for (int y = 0; y < fossil.GetLength(0); y++)
			{
				for (int x = 0; x < fossil.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30) && fossil[y, x] == 1)
					{
						Tile tile = Framing.GetTileSafely(k, l);
						tile.ClearTile();
						WorldGen.PlaceTile(k, l, (ushort)TileType<AncientFossil>());
						tile.HasTile = true;
					}
				}
			}
		}

		public void PlaceMineshaft(int[,] mineshaft, int MSLenght, int MSHeight, int PosX, int MinPosY)
		{ //Used to spawn the majority of the mineshaft tiles, before worlgen smoothes things.
			int PosY = MinPosY; // same
			for (int i = 0; i < MSLenght; i++)
			{
				for (int j = 0; j < MSHeight; j++)
				{
					int[,] arrayShape = null;
					int[,] arrayWalls = null;
					int[,] arrayShape2 = null;
					int[,] arrayWalls2 = null;
					int[,] arrayShape3 = null;
					int[,] arrayWalls3 = null;
					int yOffSet = 0;
					if (mineshaft[i, j] == 1)
					{
						arrayWalls = OrchidMSarrays.mSWall1;
						switch (Main.rand.Next(11))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshape1;
								break;
							case 1:
								arrayShape = OrchidMSarrays.mSshape2;
								break;
							case 2:
								arrayShape = OrchidMSarrays.mSshape3;
								break;
							case 3:
								arrayShape = OrchidMSarrays.mSshape4;
								break;
							case 4:
								arrayShape = OrchidMSarrays.mSshape5;
								break;
							case 5:
								arrayShape = OrchidMSarrays.mSshape6;
								break;
							case 6:
								arrayShape = OrchidMSarrays.mSshape7;
								break;
							case 7:
								arrayShape = OrchidMSarrays.mSshape8;
								break;
							case 8:
								arrayShape = OrchidMSarrays.mSshape9;
								break;
							case 9:
								arrayShape = OrchidMSarrays.mSshape10;
								break;
							case 10:
								arrayShape = OrchidMSarrays.mSshape11;
								break;
						}
					}
					if (mineshaft[i, j] == 2)
					{
						arrayWalls = OrchidMSarrays.mSWallStairs1;
						switch (Main.rand.Next(3))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeStairs1;
								break;
							case 1:
								arrayShape = OrchidMSarrays.mSshapeStairs2;
								break;
							case 2:
								arrayShape = OrchidMSarrays.mSshapeStairs3;
								break;
						}
					}
					if (mineshaft[i, j] == 3)
					{
						arrayWalls = OrchidMSarrays.mSWallTreasure1;
						switch (Main.rand.Next(3))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeTreasure1;
								break;
							case 1:
								arrayShape = OrchidMSarrays.mSshapeTreasure2;
								break;
							case 2:
								arrayShape = OrchidMSarrays.mSshapeTreasure3;
								break;
						}
					}
					if (mineshaft[i, j] == 4)
					{
						arrayWalls = OrchidMSarrays.mSWall1;
						switch (Main.rand.Next(3))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeER1;
								break;
							case 1:
								arrayShape = OrchidMSarrays.mSshapeER2;
								arrayWalls = OrchidMSarrays.mSWallER1;
								break;
							case 2:
								arrayShape = OrchidMSarrays.mSshapeER3;
								break;
						}
					}
					if (mineshaft[i, j] == 5)
					{
						arrayWalls = OrchidMSarrays.mSWallBigStairs1;
						switch (Main.rand.Next(2))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeBigStairs1;
								break;
							case 1:
								arrayShape = OrchidMSarrays.mSshapeBigStairs2;
								break;
						}
					}
					if (mineshaft[i, j] == 6)
					{
						switch (Main.rand.Next(1))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeCampfireL;
								arrayWalls = OrchidMSarrays.mSWallCampfire;
								arrayShape2 = OrchidMSarrays.mSshapeCampfireR;
								arrayWalls2 = OrchidMSarrays.mSWallCampfire;
								break;
						}
					}
					if (mineshaft[i, j] == 7)
					{
						switch (Main.rand.Next(1))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeGems;
								arrayWalls = OrchidMSarrays.mSWallGems;
								break;
						}
					}
					if (mineshaft[i, j] == 8)
					{
						arrayWalls = OrchidMSarrays.mSWall1;
						switch (Main.rand.Next(3))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeEL1;
								arrayWalls = OrchidMSarrays.mSWallEL1;
								break;
							case 1:
								arrayShape = OrchidMSarrays.mSshapeEL2;
								break;
							case 2:
								arrayShape = OrchidMSarrays.mSshapeEL3;
								break;
						}
					}
					if (mineshaft[i, j] == 9)
					{
						switch (Main.rand.Next(1))
						{ // Update this when adding rooms.
							case 0:
								arrayShape = OrchidMSarrays.mSshapeBossL;
								arrayWalls = OrchidMSarrays.mSWallBoss;
								arrayShape2 = OrchidMSarrays.mSshapeBossM;
								arrayWalls2 = OrchidMSarrays.mSWallBoss;
								arrayShape3 = OrchidMSarrays.mSshapeBossR;
								arrayWalls3 = OrchidMSarrays.mSWallBoss;
								yOffSet = -4;
								break;
						}
					}
					if (mineshaft[i, j] == 10)
					{
						arrayWalls = OrchidMSarrays.mSWall1;
						arrayShape = OrchidMSarrays.mSshapeHook;
					}
					if (mineshaft[i, j] == 11)
					{
						arrayWalls = OrchidMSarrays.mSWall1;
						arrayShape = OrchidMSarrays.mSshapeToolbox;
					}
					if (mineshaft[i, j] == 12)
					{
						arrayWalls = OrchidMSarrays.mSWall1;
						arrayShape = OrchidMSarrays.mSshapeStatic;
					}
					if (arrayShape != null && arrayWalls != null)
					{
						PlaceMSRoomTiles(PosX, PosY + yOffSet, arrayShape, arrayWalls);
						PlaceMSRoomFurnitures(PosX, PosY + yOffSet, arrayShape, arrayWalls);
					}
					if (arrayShape2 != null && arrayWalls2 != null)
					{
						PlaceMSRoomTiles(PosX + 15, PosY + yOffSet, arrayShape2, arrayWalls2);
						PlaceMSRoomFurnitures(PosX + 15, PosY + yOffSet, arrayShape2, arrayWalls2);
					}
					if (arrayShape3 != null && arrayWalls3 != null)
					{
						PlaceMSRoomTiles(PosX + 30, PosY + yOffSet, arrayShape3, arrayWalls3);
						PlaceMSRoomFurnitures(PosX + 30, PosY + yOffSet, arrayShape3, arrayWalls3);
					}
					PosY += 14;
				}
				PosY = MinPosY;
				PosX += 15;
			}
		}

		public void EndMineshaft(int[,] mineshaft, int MSLenght, int MSHeight, int PosX, int MinPosY)
		{ // Used to spawn what we WANT to spawn (furniture, ...)
			int PosY = MinPosY;
			for (int i = 0; i < MSLenght; i++)
			{
				for (int j = 0; j < MSHeight; j++)
				{
					if (mineshaft[i, j] == 1)
					{
						switch (Main.rand.Next(11)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape1, OrchidMSarrays.mSWall1);
								break;
							case 1:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape2, OrchidMSarrays.mSWall1);
								break;
							case 2:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape3, OrchidMSarrays.mSWall1);
								break;
							case 3:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape4, OrchidMSarrays.mSWall1);
								break;
							case 4:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape5, OrchidMSarrays.mSWall1);
								break;
							case 5:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape6, OrchidMSarrays.mSWall1);
								break;
							case 6:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape7, OrchidMSarrays.mSWall1);
								break;
							case 7:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape8, OrchidMSarrays.mSWall1);
								break;
							case 8:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape9, OrchidMSarrays.mSWall1);
								break;
							case 9:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape10, OrchidMSarrays.mSWall1);
								break;
							case 10:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshape11, OrchidMSarrays.mSWall1);
								break;

						}
					}
					if (mineshaft[i, j] == 2)
					{
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeStairs1, OrchidMSarrays.mSWallStairs1);
								break;
							case 1:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeStairs2, OrchidMSarrays.mSWallStairs1);
								break;
							case 2:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeStairs3, OrchidMSarrays.mSWallStairs1);
								break;
						}
					}
					if (mineshaft[i, j] == 3)
					{
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeTreasure1, OrchidMSarrays.mSWallTreasure1);
								break;
							case 1:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeTreasure2, OrchidMSarrays.mSWallTreasure1);
								break;
							case 2:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeTreasure3, OrchidMSarrays.mSWallTreasure1);
								break;
						}
					}
					if (mineshaft[i, j] == 4)
					{
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeER1, OrchidMSarrays.mSWall1);
								break;
							case 1:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeER2, OrchidMSarrays.mSWallER1);
								break;
							case 2:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeER3, OrchidMSarrays.mSWall1);
								break;
						}
					}
					if (mineshaft[i, j] == 5)
					{
						switch (Main.rand.Next(2)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeBigStairs1, OrchidMSarrays.mSWallBigStairs1);
								break;
							case 1:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeBigStairs2, OrchidMSarrays.mSWallBigStairs1);
								break;
						}
					}
					if (mineshaft[i, j] == 6)
					{
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX + 15, PosY, OrchidMSarrays.mSshapeCampfireR, OrchidMSarrays.mSWallCampfire);
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeCampfireL, OrchidMSarrays.mSWallCampfire);
								break;
						}
					}
					if (mineshaft[i, j] == 7)
					{
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeGems, OrchidMSarrays.mSWallGems);
								break;
						}
					}
					if (mineshaft[i, j] == 8)
					{
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeEL1, OrchidMSarrays.mSWallEL1);
								break;
							case 1:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeEL2, OrchidMSarrays.mSWall1);
								break;
							case 2:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeEL3, OrchidMSarrays.mSWall1);
								break;
						}
					}
					if (mineshaft[i, j] == 9)
					{
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY - 4, OrchidMSarrays.mSshapeBossL, OrchidMSarrays.mSWallBoss);
								PlaceMSFurnitures(PosX + 15, PosY - 4, OrchidMSarrays.mSshapeBossM, OrchidMSarrays.mSWallBoss);
								PlaceMSFurnitures(PosX + 30, PosY - 4, OrchidMSarrays.mSshapeBossR, OrchidMSarrays.mSWallBoss);
								break;
						}
					}
					if (mineshaft[i, j] == 10)
					{
						PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeHook, OrchidMSarrays.mSWall1);
					}
					if (mineshaft[i, j] == 11)
					{
						PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeToolbox, OrchidMSarrays.mSWall1);
					}
					if (mineshaft[i, j] == 12)
					{
						PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeStatic, OrchidMSarrays.mSWall1);
					}
					PosY += 14;
				}
				PosY = MinPosY;
				PosX += 15;
			}
		}

		private void placeQuartz()
		{
			for (int k = 0; k < (int)((OrchidMSarrays.MSLenght * 15 * OrchidMSarrays.MSHeight * 8) / 6); k++)
			{
				int x = WorldGen.genRand.Next((int)(Main.maxTilesX / 2) - (int)((OrchidMSarrays.MSLenght * 15) / 2) - 50, (int)(Main.maxTilesX / 2) + (int)((OrchidMSarrays.MSLenght * 15) / 2) + 50);
				int y = WorldGen.genRand.Next((int)(Main.maxTilesY / 3) + 50, (int)(Main.maxTilesY / 3) + (OrchidMSarrays.MSHeight * 8) + 200);

				Tile tile = Framing.GetTileSafely(x, y);
				if (!tile.HasTile && ((WorldGen.SolidTile(x - 1, y)) || (WorldGen.SolidTile(x, y + 1)) || (WorldGen.SolidTile(x, y - 1)) || (WorldGen.SolidTile(x + 1, y))))
				{
					WorldGen.PlaceTile(x, y, TileType<Tiles.Ores.StaticQuartzGem>());
					tile.HasTile = true;
				}
			}
		}

		private void placeLilies()
		{
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.015); k++)
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
						WorldGen.PlaceTile(x, y, ModContent.TileType<Content.Items.Materials.JungleLilyTile>(), style: WorldGen.genRand.Next(4));
					}
				}
			}
		}

		public void placeFossils()
		{
			int fossilQuantity = WorldGen.genRand.Next(5, 5 + (int)(2f * (Main.maxTilesX / 800f)));
			for (int i = 0; i < fossilQuantity; i++)
			{

				int[,] fossilShape;
				switch (Main.rand.Next(4))
				{
					case 0:
						fossilShape = OrchidFossilsArrays.FossilShape1;
						break;
					case 1:
						fossilShape = OrchidFossilsArrays.FossilShape2;
						break;
					case 2:
						fossilShape = OrchidFossilsArrays.FossilShape3;
						break;
					case 3:
						fossilShape = OrchidFossilsArrays.FossilShape4;
						break;
					default:
						fossilShape = OrchidFossilsArrays.FossilShape1;
						break;
				}

				bool fossilPlaced = false;
				int attempts = 0;
				while (!fossilPlaced && attempts < 100000)
				{

					int x = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
					int y = WorldGen.genRand.Next((int)(Main.worldSurface + 150), Main.maxTilesY - 300);
					int validCount = 0;

					if (Framing.GetTileSafely(x, y).TileType == TileID.Dirt || Framing.GetTileSafely(x, y).TileType == TileID.Stone)
						validCount++;
					if (Framing.GetTileSafely(x + 34, y + 27).TileType == TileID.Dirt || Framing.GetTileSafely(x, y).TileType == TileID.Stone)
						validCount++;
					if (Framing.GetTileSafely(x, y + 27).TileType == TileID.Dirt || Framing.GetTileSafely(x, y).TileType == TileID.Stone)
						validCount++;
					if (Framing.GetTileSafely(x + 34, y).TileType == TileID.Dirt || Framing.GetTileSafely(x, y).TileType == TileID.Stone)
						validCount++;
					if (Framing.GetTileSafely(x + 17, y + 13).TileType == TileID.Dirt || Framing.GetTileSafely(x, y).TileType == TileID.Stone)
						validCount++;

					if (validCount > 2)
					{
						fossilPlaced = true;
						PlaceFossil(x, y, fossilShape);
					}
					attempts++;
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
					mineshaft = new int[OrchidMSarrays.MSLenght, OrchidMSarrays.MSHeight];
					minishaft1 = new int[5, 3];
					minishaft2 = new int[5, 3];

					ruins = new int[RLenght, RHeight];
					mineshaft = GenerateMSArray(OrchidMSarrays.MSLenght, OrchidMSarrays.MSHeight);
					// ruins = GenerateRArray(RLenght,RHeight);
					MSMinPosX = (Main.maxTilesX / 2) - ((OrchidMSarrays.MSLenght * 15) / 2);
					MSMinPosY = (Main.maxTilesY / 3 + 100);
					RMinPosX = (Main.maxTilesX / 2);
					RMinPosY = (Main.maxTilesY / 5);

					progress.Message = "Generating mineshaft";
					PlaceMineshaft(mineshaft, OrchidMSarrays.MSLenght, OrchidMSarrays.MSHeight, MSMinPosX, MSMinPosY);
					//PlaceRuins(ruins, RLenght, RHeight, RMinPosX, RMinPosY);

					progress.Message = "Generating minishafts";

					minishaft1X = (Main.maxTilesX / 2) - 750 + Main.rand.Next(300);
					minishaft1Y = (Main.maxTilesY / 3) + 200 - Main.rand.Next(300);

					while (!(Framing.GetTileSafely(minishaft1X, minishaft1Y).HasTile))
					{
						minishaft1X = (Main.maxTilesX / 2) - 750 + Main.rand.Next(300);
						minishaft1Y = (Main.maxTilesY / 3) + 200 - Main.rand.Next(300);
					}

					minishaft2X = (Main.maxTilesX / 2) + 750 - Main.rand.Next(300);
					minishaft2Y = (Main.maxTilesY / 3) + 200 - Main.rand.Next(300);

					while (!(Framing.GetTileSafely(minishaft2X, minishaft2Y).HasTile))
					{
						minishaft2X = (Main.maxTilesX / 2) + 750 - Main.rand.Next(300);
						minishaft2Y = (Main.maxTilesY / 3) + 200 - Main.rand.Next(300);
					}

					minishaft1 = GenerateSmallMSArray(5, 3);
					PlaceMineshaft(minishaft1, 5, 3, minishaft1X, minishaft1Y);
					minishaft2 = GenerateSmallMSArray(5, 3);
					PlaceMineshaft(minishaft2, 5, 3, minishaft2X, minishaft2Y);

					progress.Message = "Generating Static Quartz";
					this.placeQuartz();

					progress.Message = "Generating Jungle Lilies";
					this.placeLilies();

					//	placeFossils();
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
			EndMineshaft(mineshaft, OrchidMSarrays.MSLenght, OrchidMSarrays.MSHeight, MSMinPosX, MSMinPosY);
			EndMineshaft(minishaft1, 5, 3, minishaft1X, minishaft1Y);
			EndMineshaft(minishaft2, 5, 3, minishaft2X, minishaft2Y);

			List<ChestLoot> chestLoots = new List<ChestLoot>();
			List<Chest> usedChests = new List<Chest>();

			chestLoots.Add(new ChestLoot(ItemType<EmbersCard>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<AdornedBranch>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<EmberVial>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<Warhammer>(), ChestType.SurfaceWooden, 20));
			chestLoots.Add(new ChestLoot(ItemType<BubbleCard>(), ChestType.Water, 20));
			chestLoots.Add(new ChestLoot(ItemType<SeafoamVial>(), ChestType.Water, 20));
			chestLoots.Add(new ChestLoot(ItemType<SapCard>(), ChestType.LivingTree, 50));
			chestLoots.Add(new ChestLoot(ItemType<LivingSapVial>(), ChestType.LivingTree, 50));
			chestLoots.Add(new ChestLoot(ItemType<LivingRune>(), ChestType.LivingTree, 50));
			chestLoots.Add(new ChestLoot(ItemType<GoldChestCard>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<EnchantedScepter>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<EnchantedRune>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<EnchantedPavise>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<CloudInAVial>(), ChestType.Gold, 20));
			chestLoots.Add(new ChestLoot(ItemType<FartInAVial>(), ChestType.Gold, 1, needToPlace: 0, ignoreChestLimit: true));
			chestLoots.Add(new ChestLoot(ItemType<DeckEnchanted>(), ChestType.Gold, 5, needToPlace: 0, ignoreChestLimit: true));
			chestLoots.Add(new ChestLoot(ItemType<BlizzardInAVial>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<IceChestFlask>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<IceChestCard>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<AvalancheScepter>(), ChestType.Ice, 20));
			chestLoots.Add(new ChestLoot(ItemType<TiamatRelic>(), ChestType.DungeonLocked, 50, 1, 3, true));
			chestLoots.Add(new ChestLoot(ItemType<SpiritedWater>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<DungeonFlask>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<DungeonCatalyst>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<Rusalka>(), ChestType.DungeonLocked, 20));
			chestLoots.Add(new ChestLoot(ItemType<DeckBone>(), ChestType.DungeonLocked, 5, needToPlace: 0, ignoreChestLimit: true));
			chestLoots.Add(new ChestLoot(ItemType<FireBatScepter>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<ShadowChestFlask>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<KeystoneOfTheConvent>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<ImpDiceCup>(), ChestType.Shadow, 30));
			chestLoots.Add(new ChestLoot(ItemType<DeepForestCharm>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<IvyChestCard>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<BloomingBud>(), ChestType.Ivy, 20));
			chestLoots.Add(new ChestLoot(ItemType<BundleOfClovers>(), ChestType.Ivy, 20)); 
			chestLoots.Add(new ChestLoot(ItemType<DeckJungle>(), ChestType.Ivy, 5, needToPlace: 0, ignoreChestLimit:true));
			chestLoots.Add(new ChestLoot(ItemType<SunplateFlask>(), ChestType.SkyIsland, 30));
			chestLoots.Add(new ChestLoot(ItemType<RuneOfHorus>(), ChestType.Sandstone, 20));

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

			if (chest != null && Main.tile[chest.x, chest.y].TileType == (ushort)TileType<Content.Items.Placeables.MinersLockboxTile>())
			{
				int[] specialItemPoll = {49, 50, 53, 54, 55, 975, 997, 930, ItemType<Shaman.Weapons.EnchantedScepter>()
					, ItemType<Alchemist.Weapons.Air.CloudInAVial>(), ItemType<Gambler.Weapons.Cards.GoldChestCard>()};
				int rand = Main.rand.Next(specialItemPoll);

				chest.item[0].SetDefaults(Mod.Find<ModItem>("HauntedCandle").Type);
				placeInChest(chest, 71, Main.rand.Next(80, 99)); // Copper Coins
				placeInChest(chest, 72, Main.rand.Next(80, 99)); // Silver Coins
				placeInChest(chest, 28, Main.rand.Next(3, 8)); // Healing Pots
				placeInChest(chest, 2, Main.rand.Next(5, 15)); // Iron Bars
				placeInChest(chest, 166, Main.rand.Next(10, 20)); // Bombs
				placeInChest(chest, 965, Main.rand.Next(50, 100)); // Ropes
				placeInChest(chest, ItemType<Gambler.Weapons.Cards.DetonatorCard>(), 1); // Ropes

				if (rand == 930) placeInChest(chest, 931, Main.rand.Next(21) + 30); // Flares
				placeInChest(chest, rand, 1);
			}

			if (chest != null && Main.tile[chest.x, chest.y].TileType == (ushort)TileType<Content.Items.Placeables.ShamanBiomeChestTile>())
			{
				chest.item[0].SetDefaults(Mod.Find<ModItem>("ShroomiteScepter").Type);
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

	public class ChestLoot {
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
			this.placed = 0;
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