using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
using Terraria.GameContent.Tile_Entities;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ObjectData;
using Terraria.Utilities;
using Terraria.World.Generation;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod
{
	public class OrchidWorld : ModWorld
	{
		private static int MSLenght = 15;
		private static int MSHeight = 10;
		private static int MinPosX;
		private static int	MinPosY;
		private int [,]	mineshaft = new int[MSLenght,MSHeight]; 
		public void PlaceMSRoom(int i, int j, int[,] MS, int[,]MSWall) {
			int wallrand = Main.rand.Next(5);
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
						switch (MS[y, x])
						{
							case 1:
								if (Framing.GetTileSafely(k, l+1).active() == false) {
									if  (allowPlatforms == 0) WorldGen.PlaceTile(k, l, 19); // One room out of 3 will have a plaftorm above every air block instead of wood.
									if 	(allowPlatforms != 0 && allowPlatforms2 == 0) WorldGen.PlaceTile(k, l, 19); // Other 2 rooms, have a 1 in 3 chance to get every wood block above air to be a platform
								}
								else if (Main.rand.Next(10) != 0) tile.type = 30; //Wood (and every wood block has a 1/10 chance not to be spawned)
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceTile(k, l, 19); // Plaform
								tile.active(true);
								break;
							case 3:
								tile.type = TileID.Cobweb;
								tile.active(true);
								break;
							case 4: 
								break;
							case 5:
								tile.type = TileID.Chain;
								tile.active(true);
								break;
							case 6:
								tile.type = 239; //Copper Bar // Other bars are 239(2) ; 239(3) ... in code, idk how to spawn them
								tile.active(true);
								break;
							case 7: // Unused
								break;
							case 8:
								tile.type = 124; //Wooden Beam
								tile.active(true);
								break;
							case 9:
								tile.type = TileID.Rope;
								tile.active(true);
								break;
							case 10:
								tile.type = 30; // Wood /// USE THIS IF YOU WANT TO SPAWN A WOOD BLOCK 100% OF THE TIME
								tile.active(true);
								break;
							case 11:
								tile.type = 0; // Dirt
								tile.active(true);
								break;
							case 12:
								tile.type = TileID.Stone;
								tile.active(true);
								break;
							case 13:
								tile.type = 67; // Amethyst
								tile.active(true);
								break;
							case 14:
								tile.type = 178; // Amethyst
								tile.active(true);
								break;
							case 15: 
								WorldGen.PlaceObject(k, l-1, 215);
								WorldGen.PlaceObject(k, l, 215);
								WorldGen.PlaceObject(k, l+1, 215);
								WorldGen.PlaceObject(k-1, l, 215);
								WorldGen.PlaceObject(k+1, l, 215);
								break;
						}
						
						switch (MSWall[y, x]) // in case you want to spawn anything else than planked walls (don't ?)
						{
							case 1:
								if (wallrand == 0) if (Main.rand.Next(5) != 0) tile.wall = 27; //PlankedWall
								if (wallrand == 1) if (Main.rand.Next(2) != 0) tile.wall = 2; //Dirt
								if (wallrand == 2) if (Main.rand.Next(2) != 0) tile.wall = 1; //Stone
								if (wallrand == 3) {
									if (Main.rand.Next(5) >= 3) tile.wall = 27; //PlankedWall + Stone
									else tile.wall = 1;
								}
								if (wallrand == 4) { // mash
									if (Main.rand.Next(3) == 0) tile.wall = 27; //PlankedWall + Stone
									if (Main.rand.Next(3) == 0) tile.wall = 2;
									if (Main.rand.Next(3) == 0) tile.wall = 1;
								}
								break;
						}
					}
				}
			}
		}

		public void PlaceMSFurnitures(int i, int j, int[,] MS, int[,]MSWall) {
			Tile tile;
			for (int y = 0; y < MS.GetLength(0); y++)
			{
				for (int x = 0; x < MS.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					int allowPlatforms = Main.rand.Next(3);
					if (WorldGen.InWorld(k, l, 30))
					{/*
						switch (MSWall[y, x])
						{
							case 1:
								tile = Framing.GetTileSafely(k, l);
								tile.liquid = 0;
								break;
						}*/
						switch (MS[y, x])
						{
							case 4 :
								tile = Framing.GetTileSafely(k, l-1);
								if (tile.active() == false) {
									tile.type = 30;
									tile.active(true);
								}
								WorldGen.PlaceObject(k, l, 42, true, 6); // 33 = candle /42 = Lantern
								break;
							case 7:
								for (int w=3;w>-2; w--) {
									for (int q=3; q>-2; q--) {
										tile = Framing.GetTileSafely(k-w, l-q);
										tile.ClearTile();
										tile.wall = 27;
									}
								}
								WorldGen.PlaceObject(k, l, mod.TileType<Tiles.Ambient.MineshaftPickaxeTile>());
								break;
							case 16:
								for (int w=2;w>0; w--) {
									for (int q=2; q>0; q--) {
										tile = Framing.GetTileSafely(k-w, l-q);
										tile.ClearTile();
									}
								}
								WorldGen.PlaceObject(k, l, mod.TileType<Tiles.Ambient.MineshaftCrate>());
								break;
						}
					}
				}
			}
		}
		
		private int[,] GenerateMSArray(int MSLenght, int MSHeight) {
			int[,] mineshaft = new int[MSLenght,MSHeight];
			int treasureNumber = 3; // Scale with size ? / CAN BE CHANGED
			int campfireNumber = 2; // same / CAN BE CHANGED
			int bigStairsNumber = 2; // same ? DO NOT CHANGE FOR NOW
			int bossNumber = 1; // CAN BE CHANGED
			int gemNumber = 1; // CAN BE CHANGED - don't go over MSHeight-1
			int randL;
			int randH;
			bool placed;
			int tries;
			int lockedvalue = -1;
			int sidesrand = 5; // Blank rand on sides
			
			for (int i = 0; i < MSLenght ; i++ ) { // FILL GRID WITH NORMAL ROOMS
				for (int j = 0; j < MSHeight ; j++ ) {
					mineshaft[i,j] = 1;
				}
			}
			
			for (int i = 0; i < MSHeight ; i++ ) { // DELETE A RANDOM AMOUNT OF ROOMS ON SIDES
				for (int j = 0; j < Main.rand.Next(sidesrand) ; j++ ) {
					mineshaft[j,i] = 0;
				}
				for (int j = MSLenght-1 ; j > (MSLenght - Main.rand.Next(sidesrand) -1) ; j-- ) {
					mineshaft[j,i] = 0;
				}
			}
			
			for (int i = 0; i < bigStairsNumber ; i++ ) { // PLACE BIG STAIRS
				tries = 0;
				placed = false;
				while (placed == false && tries < 20) {
					randL = (Main.rand.Next(MSLenght - 4 ) + 2);
					randH = (Main.rand.Next(MSHeight - 4 ) + 1) ;
					while (randH == lockedvalue || randH == lockedvalue + 1) randH = (Main.rand.Next(MSHeight - 4 ) + 1) ;
					tries ++;
					if (mineshaft[randL,randH] == 1 && mineshaft[randL,randH + 2 ] == 1) {
						mineshaft[randL,randH] = 5;
						mineshaft[randL,randH + 1] = 0;
						mineshaft[randL,randH + 2] = 0;
						int j = 0;
						lockedvalue = randH;
						placed = true;
					}
				}
			}			
			
			for (int i = 0; i < MSHeight-1 ; i++ ) { // PLACE STAIRS
				tries = 0;
				placed = false;
				while (placed == false && tries < 20) {
					randL = (Main.rand.Next(MSLenght - 4 ) + 2);
					tries ++;
					if (mineshaft[randL,i] == 1 && mineshaft[randL,i+ 1 ] == 1) {
						mineshaft[randL,i] = 2;
						mineshaft[randL,i + 1 ] = 0;
						placed = true;
					}
				}
			}
			
			for (int i = 0; i < MSHeight ; i++ ) {  // REPLACE RIGHT MOST ROOM WITH "RIGHT END" ROOM
				placed = false;
				int j = MSLenght-1;
				while (placed == false) {
					if (mineshaft[j,i] == 1) {
						mineshaft[j,i] = 4;
						placed = true;
					}
					if (mineshaft[j,i] == 2) {
						mineshaft[j+1,i] = 4;
						placed = true;
					}
					j--;
				}
			}
			
			for (int i = 0; i < MSHeight ; i++ ) {  // REPLACE LEFT MOST ROOM WITH "LEFT END" ROOM
				placed = false;
				int j = 0;
				while (placed == false) {
					if (mineshaft[j,i] == 1) {
						mineshaft[j,i] = 8;
						placed = true;
					}
					if (mineshaft[j,i] == 2) {
						mineshaft[j-1,i] = 8;
						placed = true;
					}
					j++;
				}
			}
			
			while (treasureNumber > 0) { // PLACE TREASURE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight-1);
				if (mineshaft[randL,randH] == 1) {
					treasureNumber --;
					mineshaft[randL,randH] = 3;
				}
			}

			while (campfireNumber > 0) { // PLACE CAMPFIRE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL,randH] == 1 && mineshaft[randL+1,randH] == 1) {
					campfireNumber --;
					mineshaft[randL,randH] = 6;
					mineshaft[randL+1,randH] = 0;
				}
			}
			
			while (gemNumber > 0) { // PLACE GEMSTONE ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL,randH] == 4) {
					gemNumber --;
					mineshaft[randL,randH] = 7;
				}
			}
			
			while (bossNumber > 0) { // PLACE BOSS ROOMS
				randL = Main.rand.Next(MSLenght);
				randH = Main.rand.Next(MSHeight);
				if (mineshaft[randL,randH] == 1 && mineshaft[randL+1,randH] == 1 && mineshaft[randL+2,randH] == 1) {
					bossNumber --;
					mineshaft[randL,randH] = 9;
					mineshaft[randL+1,randH] = 0;
					mineshaft[randL+2,randH] = 0;
				}
			}
			
			/* //SPAWN TESTS
			mineshaft[0,0] = 6;
			mineshaft[1,0] = 0;
			*/
			
			return mineshaft;
		}
		
		public void PlaceMineshaft(int[,] mineshaft, int MSLenght, int MSHeight, int MinPosX, int MinPosY) {
			int PosX = MinPosX; // used in generation (locate rooms)
			int PosY = MinPosY; // same
			for (int i = 0; i < MSLenght ; i++ ) {
				for (int j = 0; j < MSHeight ; j++ ) {
					if (mineshaft[i,j] == 1) {
						switch (Main.rand.Next(8)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape1, OrchidMSarrays.mSWall1);
								break;
							case 1:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape2, OrchidMSarrays.mSWall1);
								break;
							case 2:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape3, OrchidMSarrays.mSWall1);
								break;
							case 3:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape4, OrchidMSarrays.mSWall1);
								break;
							case 4:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape5, OrchidMSarrays.mSWall1);
								break;
							case 5:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape6, OrchidMSarrays.mSWall1);
								break;
							case 6:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape7, OrchidMSarrays.mSWall1);
								break;
							case 7:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshape8, OrchidMSarrays.mSWall1);
								break;
						}
					}
					if (mineshaft[i,j] == 2) {
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeStairs1, OrchidMSarrays.mSWallStairs1);
								break;
							case 1:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeStairs2, OrchidMSarrays.mSWallStairs1);
								break;
							case 2:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeStairs3, OrchidMSarrays.mSWallStairs1);
								break;
						}
					}
					if (mineshaft[i,j] == 3) {
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeTreasure1, OrchidMSarrays.mSWallTreasure1);
								break;
							case 1:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeTreasure2, OrchidMSarrays.mSWallTreasure1);
								break;
							case 2:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeTreasure3, OrchidMSarrays.mSWallTreasure1);
								break;
						}
					}
					if (mineshaft[i,j] == 4) {
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeER1, OrchidMSarrays.mSWall1);
								break;
							case 1:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeER2, OrchidMSarrays.mSWallER1);
								break;
							case 2:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeER3, OrchidMSarrays.mSWall1);
								break;
						}
					}
					if (mineshaft[i,j] == 5) {
						switch (Main.rand.Next(2)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeBigStairs1, OrchidMSarrays.mSWallBigStairs1);
								break;
							case 1:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeBigStairs2, OrchidMSarrays.mSWallBigStairs1);
								break;
						}
					}
					if (mineshaft[i,j] == 6) {
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeCampfireL, OrchidMSarrays.mSWallCampfire);
								PlaceMSRoom(PosX+15, PosY, OrchidMSarrays.mSshapeCampfireR, OrchidMSarrays.mSWallCampfire);
								break;
						}
					}
					if (mineshaft[i,j] == 7) {
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeGems, OrchidMSarrays.mSWallGems);
								break;
						}
					}
					if (mineshaft[i,j] == 8) {
						switch (Main.rand.Next(3)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeEL1, OrchidMSarrays.mSWallEL1);
								break;
							case 1:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeEL2, OrchidMSarrays.mSWall1);
								break;
							case 2:
								PlaceMSRoom(PosX, PosY, OrchidMSarrays.mSshapeEL3, OrchidMSarrays.mSWall1);
								break;
						}
					}
					if (mineshaft[i,j] == 9) {
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSRoom(PosX, PosY-4, OrchidMSarrays.mSshapeBossL, OrchidMSarrays.mSWallBoss);
								PlaceMSRoom(PosX+15, PosY-4, OrchidMSarrays.mSshapeBossM, OrchidMSarrays.mSWallBoss);
								PlaceMSRoom(PosX+30, PosY-4, OrchidMSarrays.mSshapeBossR, OrchidMSarrays.mSWallBoss);
								break;
						}
					}
				PosY += 14;
				}
			PosY = MinPosY;
			PosX += 15;
			}
		}
		
		public void EndMineshaft(int[,] mineshaft, int MSLenght, int MSHeight, int MinPosX, int MinPosY){
			int PosX = MinPosX;
			int PosY = MinPosY; 
			for (int i = 0; i < MSLenght ; i++ ) {
				for (int j = 0; j < MSHeight ; j++ ) {
					if (mineshaft[i,j] == 1) {
						switch (Main.rand.Next(8)) // Update this when adding rooms.
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
						}
					}
					if (mineshaft[i,j] == 2) {
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
					if (mineshaft[i,j] == 3) {
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
					if (mineshaft[i,j] == 4) {
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
					if (mineshaft[i,j] == 5) {
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
					if (mineshaft[i,j] == 6) {
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX+15, PosY, OrchidMSarrays.mSshapeCampfireR, OrchidMSarrays.mSWallCampfire);
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeCampfireL, OrchidMSarrays.mSWallCampfire);
								break;
						}
					}
					if (mineshaft[i,j] == 7) {
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY, OrchidMSarrays.mSshapeGems, OrchidMSarrays.mSWallGems);
								break;
						}
					}
					if (mineshaft[i,j] == 8) {
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
					if (mineshaft[i,j] == 9) {
						switch (Main.rand.Next(1)) // Update this when adding rooms.
						{
							case 0:
								PlaceMSFurnitures(PosX, PosY-4, OrchidMSarrays.mSshapeBossL, OrchidMSarrays.mSWallBoss);
								PlaceMSFurnitures(PosX+15, PosY-4, OrchidMSarrays.mSshapeBossM, OrchidMSarrays.mSWallBoss);
								PlaceMSFurnitures(PosX+30, PosY-4, OrchidMSarrays.mSshapeBossR, OrchidMSarrays.mSWallBoss);
								break;
						}
					}
				PosY += 14;
				}
			PosY = MinPosY;
			PosX += 15;
			}
		}
		
		
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			mineshaft = new int[MSLenght,MSHeight]; 
			mineshaft = GenerateMSArray(MSLenght, MSHeight); 
			MinPosX = (Main.maxTilesX/2)-((MSLenght*15)/2);
			MinPosY = Main.maxTilesY/3 + Main.rand.Next(Main.maxTilesY/5);
			int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
			if (LivingTreesIndex != -1)
			{
				tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress)
				{
					progress.Message = "Generating mineshaft";
					PlaceMineshaft(mineshaft, MSLenght, MSHeight, MinPosX, MinPosY);
				}));
			}
		}
		public override void PostWorldGen()
        {
			EndMineshaft(mineshaft, MSLenght, MSHeight, MinPosX, MinPosY);
		}
	}
}