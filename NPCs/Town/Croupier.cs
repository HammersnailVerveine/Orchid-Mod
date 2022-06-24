using Microsoft.Xna.Framework;
using OrchidMod.Gambler;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Town
{
	// [AutoloadHead] and npc.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Croupier : ModNPC
	{
		public override string Texture => "OrchidMod/NPCs/Town/Croupier";

		// public override bool IsLoadingEnabled(Mod mod)/* tModPorter Suggestion: If you return false for the purposes of manual loading, use the [Autoload(false)] attribute on your class instead */
		// {
		// 	name = "Croupier";
		// 	return Mod.Properties/* tModPorter Note: Removed. Instead, assign the properties directly (ContentAutoloadingEnabled, GoreAutoloadingEnabled, MusicAutoloadingEnabled, and BackgroundAutoloadingEnabled) */.Autoload;
		// }


		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			DisplayName.SetDefault("Croupier");
			Main.npcFrameCount[NPC.type] = 26;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 28;
			NPC.height = 44;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
		}

		// public override void HitEffect(int hitDirection, double damage) {
		// int num = npc.life > 0 ? 1 : 5;
		// for (int k = 0; k < num; k++) {
		// Dust.NewDust(npc.position, npc.width, npc.height, 6);
		// }
		// }

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				for (int k = 0; k < 255; k++)
				{
					Player player = Main.player[k];
					if (!player.active)
					{
						continue;
					}

					OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
					if (modPlayer.gamblerHasCardInDeck)
					{
						return true;
					}
				}
			}
			else
			{
				return true;
			}
			return false;
		}

		public override List<string> SetNPCNameList()
		{
			return new()
			{
				"Capone",
				"Cadillac",
				"Tannenbaum",
				"Alderman",
				"Accardo",
				"Angelini",
				"Bonanno",
				"Ambrosino",
				"D'Amico",
				"Attanasio",
				"Manocchio"
			};
		}

		public override void FindFrame(int frameHeight)
		{
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat()
		{
			switch (Main.rand.Next(7))
			{
				case 0:
					return "Cards turnin' your way?";
				case 1:
					return "I like them odds, chief. Always like them odds.";
				case 2:
					return "Got you covered on cards, chief. We got poker, solitaire... more poker...";
				case 3:
					return "Ey, chief, best bud! Since we're such great pals, could ya help with a little debt?";
				case 4:
					return "I'm your rear hand man.";
				case 5:
					return "Fate's face down. How do you wanna be dealt?";
				default:
					return "Pick a number between 1 and 86!";
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			Player player = Main.player[Main.myPlayer];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();

			// [SP]

			/*if (!OrchidMod.Instance.croupierUI.Visible)
			{
				button = Language.GetTextValue("LegacyInterface.28");

				string deckBuilding = $"[c/{Colors.AlphaDarken(new Color(255, 200, 0)).Hex3()}:Deck Building]";
				button2 = OrchidModGamblerHelper.hasGamblerDeck(player) ? deckBuilding : "Get a New Deck";
			}
			else
			{
				button = "Return";
			}*/
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			Player player = Main.player[Main.myPlayer];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();

			if (firstButton)
			{
				// [SP]

				/*if (OrchidMod.Instance.croupierUI.Visible)
				{
					OrchidMod.Instance.croupierUI.Visible = false;

					//Main.npcShop = 0;
					Main.npcChatCornerItem = 0;
					Recipe.FindRecipes();
					Main.npcChatText = Main.npc[player.talkNPC].GetChat();
				}
				else shop = true;*/
			}
			else
			{
				if (modPlayer.HasGamblerDeck())
				{
					Main.npcChatText = $"Not too fond of your odds, eh? Aight, go on.";

					/*switch (Main.rand.Next(0, 1))
					{
						case 0:
							Main.npcChatText = $"Not too fond of your odds, eh? Aight, go on.\n" +
								$"Calamity - best mod...";
							break;
					}*/

					Main.npcChatText += "\n\n\n\n\n";
					Main.npcChatFocus2 = false;
					Main.npcChatFocus3 = false;

					// [SP]

					//OrchidMod.Instance.croupierUI.UpdateOnChatButtonClicked();
					//OrchidMod.Instance.croupierUI.Visible = true;
				}
				else
				{
					for (int i = 0; i < 50; i++)
					{
						Item item = Main.LocalPlayer.inventory[i];
						if (item.type == 0)
						{
							Main.npcChatText = $"You lost it already? Here chief, take your new deck.";
							int gamblerDeck = ItemType<Gambler.Decks.GamblerAttack>();
							player.QuickSpawnItem(NPC.GetSource_FromThis(), gamblerDeck, 1);
							return;
						}
					}
					Main.npcChatText = $"My man, your pockets are full. You wouldn't let a brand new deck sitting on the ground, would ya?";
				}
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			Player player = Main.player[Main.myPlayer];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();

			shop.item[nextSlot].SetDefaults(ItemType<Gambler.GamblerDummy>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Chips.GamblingChip>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Dice.GamblingDie>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.ShuffleCard>());
			nextSlot++;

			if (!player.ZoneBeach && !player.ZoneCorrupt && !player.ZoneSkyHeight && !player.ZoneCrimson && !player.ZoneHallow && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneDesert && !player.ZoneGlowshroom && player.ZoneOverworldHeight)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.ForestCard>());
				nextSlot++;
			}
			if (player.ZoneSnow)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.SnowCard>());
				nextSlot++;
			}
			if (player.ZoneDesert)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.DesertCard>());
				nextSlot++;
			}
			if (player.ZoneBeach)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.OceanCard>());
				nextSlot++;
			}
			if (player.ZoneJungle)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.JungleCard>());
				nextSlot++;
			}
			if (player.ZoneGlowshroom)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.MushroomCard>());
				nextSlot++;
			}
			if (player.ZoneUnderworldHeight)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.HellCard>());
				nextSlot++;
			}
			if (player.ZoneSkyHeight)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.SkyCard>());
				nextSlot++;
			}
			if (Main.slimeRain)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.SlimeRainCard>());
				nextSlot++;
			}

			if (modPlayer.CheckSetCardsInDeck("Slime") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.SlimyLollipop>());
				nextSlot++;
			}
			if (modPlayer.CheckSetCardsInDeck("Biome") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.LuckySprout>());
				nextSlot++;
			}
			if (modPlayer.CheckSetCardsInDeck("Boss") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.ConquerorsPennant>());
				nextSlot++;
			}
			if (modPlayer.CheckSetCardsInDeck("Elemental") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.ElementalLens>());
				nextSlot++;
			}
		}

		// public override void NPCLoot() {
		// Item.NewItem(npc.getRect(), ItemType<Items.Armor.ExampleCostume>());
		// }

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true;
		}

		// Make something happen when the npc teleports to a statue. Since this method only runs server side, any visual effects like dusts or gores have to be synced across all clients manually.
		// public override void OnGoToStatue(bool toQueenStatue) {
		// if (Main.netMode == NetmodeID.Server) {
		// ModPacket packet = mod.GetPacket();
		// packet.Write((byte)ExampleModMessageType.ExampleTeleportToStatue);
		// packet.Write((byte)npc.whoAmI);
		// packet.Send();
		// }
		// }

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileType<NPCs.Town.Projectiles.CroupierProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}
