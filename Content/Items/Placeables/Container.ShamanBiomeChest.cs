using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OrchidMod.Content.Items.Placeables
{
	public class ShamanBiomeChest : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.createTile = ModContent.TileType<ShamanBiomeChestTile>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroom Chest");
		}
	}

	public class ShroomKey : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroom Key");
			Tooltip.SetDefault("Unlocks a Shroom Chest in the dungeon");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 28;
			Item.maxStack = 1;
			Item.useStyle = ItemUseStyleID.None;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 0, 0, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var tooltip = tooltips.Find(i => i.Mod == "Terraria" && i.Name.StartsWith("Tooltip"));

			if (tooltip != null && !NPC.downedPlantBoss) tooltip.Text = Language.GetTextValue("LegacyTooltip.59");
		}
	}

	public class ShamanBiomeChestTile : ModTile
	{
		public override string Texture => OrchidAssets.TilesPath + Name;

		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileOreFinderPriority[Type] = 500;

			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.BasicChest[Type] = true;
			TileID.Sets.IsAContainer[Type] = true;
			TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
			TileID.Sets.GeneralPlacementTiles[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.AvoidedByNPCs[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Shroom Chest");
			AddMapEntry(new Color(174, 129, 92), name, MapChestName);

			name = CreateMapEntryName("_Locked" + Name);
			name.SetDefault("Locked Shroom Chest");
			AddMapEntry(new Color(174, 129, 92), name, MapChestName);

			DustType = 239;
			AdjTiles = new int[] { TileID.Containers };
			ChestDrop = ModContent.ItemType<ShamanBiomeChest>();

			ContainerName.SetDefault("Shroom Chest");
		}

		public static string MapChestName(string name, int i, int j)
		{
			if (i < 0 || i >= Main.maxTilesX || j < 0 || j >= Main.maxTilesY) return name;

			Tile tile = Main.tile[i, j];
			if (tile == null) return name;

			int left = i;
			int top = j;

			if (tile.TileFrameX % 36 != 0) left--;
			if (tile.TileFrameY != 0) top--;

			int chest = Chest.FindChest(left, top);
			return name + ((Main.chest[chest].name != "") ? (": " + Main.chest[chest].name) : "");
		}

		public override ushort GetMapOption(int i, int j)
			=> (ushort)(Main.tile[i, j].TileFrameX / 36);

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
			=> true;

		public override bool IsLockedChest(int i, int j)
			=> Main.tile[i, j].TileFrameX / 36 == 1;

		public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
		{
			if (!NPC.downedPlantBoss) return false;

			AchievementsHelper.NotifyProgressionEvent(20);
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ChestDrop);
			Chest.DestroyChest(i, j);
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];

			Main.mouseRightRelease = false;

			int left = i;
			int top = j;

			if (tile.TileFrameX % 36 != 0) left--;
			if (tile.TileFrameY != 0) top--;

			if (player.sign >= 0)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);

				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = "";
			}

			if (Main.editChest)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);

				Main.editChest = false;
				Main.npcChatText = "";
			}

			if (player.editedChestName)
			{
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);

				player.editedChestName = false;
			}

			bool isLocked = IsLockedChest(left, top);

			if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
			{
				if (left == player.chestX && top == player.chestY && player.chest >= 0)
				{
					player.chest = -1;

					Recipe.FindRecipes();
					SoundEngine.PlaySound(SoundID.MenuClose);
				}
				else
				{
					NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
					Main.stackSplit = 600;
				}
			}
			else
			{
				if (isLocked)
				{
					if (!NPC.downedPlantBoss) return true;

					int key = ModContent.ItemType<ShroomKey>();

					if (player.ConsumeItem(key) && Chest.Unlock(left, top))
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.Unlock, -1, -1, null, player.whoAmI, 1f, (float)left, (float)top);
						}
					}
				}
				else
				{
					int chest = Chest.FindChest(left, top);

					if (chest >= 0)
					{
						Main.stackSplit = 600;

						if (chest == player.chest)
						{
							player.chest = -1;
							SoundEngine.PlaySound(SoundID.MenuClose);
						}
						else
						{
							player.chest = chest;
							player.chestX = left;
							player.chestY = top;
							Main.playerInventory = true;
							Main.recBigList = false;

							SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
						}
						Recipe.FindRecipes();
					}
				}
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = -1;

			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;

			if (tile.TileFrameX % 36 != 0) left--;
			if (tile.TileFrameY != 0) top--;

			int chest = Chest.FindChest(left, top);

			if (chest < 0)
			{
				player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
			}
			else
			{
				var name = Main.chest[chest].name;
				player.cursorItemIconText = name.Length > 0 ? name : "Shroom Chest";

				if (player.cursorItemIconText == "Shroom Chest")
				{
					player.cursorItemIconID = ModContent.ItemType<ShamanBiomeChest>();
					player.cursorItemIconText = "";

					if (Main.tile[left, top].TileFrameX / 36 == 1) player.cursorItemIconID = ModContent.ItemType<ShroomKey>();
				}
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);

			Player player = Main.LocalPlayer;

			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}