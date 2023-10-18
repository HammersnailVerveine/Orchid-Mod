using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Alchemist;
using OrchidMod.Content.Dancer;
using OrchidMod.Content.Gambler;
using OrchidMod.Content.Shaman;
using OrchidMod.Content.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Common;

namespace OrchidMod
{
	public class OrchidPlayer : ModPlayer
	{
		public OrchidShaman modPlayerShaman;
		public OrchidAlchemist modPlayerAlchemist;
		public OrchidGambler modPlayerGambler;
		public OrchidDancer modPlayerDancer;
		public OrchidGuardian modPlayerGuardian;

		public bool hauntedCandle = false;
		public bool remoteCopterPet = false;
		public bool spawnedGhost = false;
		public int originalSelectedItem;
		public bool autoRevertSelectedItem = false;

		public int timer120 = 0;
		public int doubleTap = 0;
		public int doubleTapCooldown = 0;
		public bool doubleTapLock = false;
		public int keepSelected = -1;

		/*General*/

		public bool generalTools = false;
		public bool generalStatic = false;

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			if (Player.ZoneSkyHeight && !attempt.inLava && !attempt.inHoney && Main.rand.NextBool(10) && Main.hardMode && attempt.rare)
			{
				itemDrop = ModContent.ItemType<Content.Shaman.Weapons.Hardmode.WyvernMoray>();
			}
		}

		public override void Initialize()
		{
			modPlayerShaman = Player.GetModPlayer<OrchidShaman>();
			modPlayerAlchemist = Player.GetModPlayer<OrchidAlchemist>();
			modPlayerGambler = Player.GetModPlayer<OrchidGambler>();
			modPlayerDancer = Player.GetModPlayer<OrchidDancer>();
			modPlayerGuardian = Player.GetModPlayer<OrchidGuardian>();
		}

		public override void PreUpdate()
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				if (autoRevertSelectedItem)
				{
					if (Player.itemTime == 0 && Player.itemAnimation == 0)
					{
						Player.selectedItem = originalSelectedItem;
						autoRevertSelectedItem = false;
					}
				}
			}
		}

		public override void PostUpdateEquips()
		{
			//this.updateBuffEffects();
			//this.updateItemEffects();
			this.CheckWoodBreak(Player);
		}

		public override void ResetEffects()
		{
			timer120++;
			if (timer120 == 120)
				timer120 = 0;

			remoteCopterPet = false;

			generalTools = false;
			generalStatic = false;

			if (this.keepSelected != -1)
			{
				Player.selectedItem = keepSelected;
				this.keepSelected = -1;
			}

			hauntedCandle = false;
			spawnedGhost = false;
		}

		public void CheckWoodBreak(Player player)
		{ // From Vanilla Source
			if (player.velocity.Y <= 1f || this.generalTools)
				return;
			Vector2 vector2 = player.position + player.velocity;
			int num1 = (int)(vector2.X / 16.0);
			int num2 = (int)((vector2.X + (double)player.width) / 16.0);
			int num3 = (int)((player.position.Y + (double)player.height + 1.0) / 16.0);
			for (int i = num1; i <= num2; ++i)
			{
				for (int j = num3; j <= num3 + 1; ++j)
				{
					if (Main.tile[i, j].HasUnactuatedTile && (int)Main.tile[i, j].TileType == TileType<Content.Tiles.Ambient.FragileWood>() && !WorldGen.SolidTile(i, j - 1))
					{
						WorldGen.KillTile(i, j, false, false, false);
						// if (Main.netMode == 1)
						// NetMessage.SendData(17, -1, -1, (NetworkText) null, 0, (float) i, (float) j, 0.0f, 0, 0, 0);
					}
				}
			}
		}
	}
}
