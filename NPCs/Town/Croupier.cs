using Microsoft.Xna.Framework;
using OrchidMod.Gambler;
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

		public override bool Autoload(ref string name)
		{
			name = "Croupier";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			DisplayName.SetDefault("Croupier");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 28;
			npc.height = 44;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
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

					OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
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

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(11))
			{
				case 0:
					return "Capone";
				case 1:
					return "Cadillac";
				case 2:
					return "Tannenbaum";
				case 3:
					return "Alderman";
				case 4:
					return "Accardo";
				case 5:
					return "Angelini";
				case 6:
					return "Bonanno";
				case 7:
					return "Ambrosino";
				case 8:
					return "D'Amico";
				case 9:
					return "Attanasio";
				default:
					return "Manocchio";
			}
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
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (!OrchidMod.Instance.croupierUI.Visible)
			{
				button = Language.GetTextValue("LegacyInterface.28");

				string deckBuilding = $"[c/{Colors.AlphaDarken(new Color(255, 200, 0)).Hex3()}:Deck Building]";
				button2 = OrchidModGamblerHelper.hasGamblerDeck(player) ? deckBuilding : "Get a New Deck";
			}
			else
			{
				button = "Return";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (firstButton)
			{
				if (OrchidMod.Instance.croupierUI.Visible)
				{
					OrchidMod.Instance.croupierUI.Visible = false;

					Main.npcShop = 0;
					Main.npcChatCornerItem = 0;
					Recipe.FindRecipes();
					Main.npcChatText = Main.npc[player.talkNPC].GetChat();
				}
				else shop = true;
			}
			else
			{
				if (OrchidModGamblerHelper.hasGamblerDeck(player))
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

					OrchidMod.Instance.croupierUI.UpdateOnChatButtonClicked();
					OrchidMod.Instance.croupierUI.Visible = true;
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
							player.QuickSpawnItem(gamblerDeck, 1);
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
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			shop.item[nextSlot].SetDefaults(ItemType<Gambler.GamblerDummy>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Chips.GamblingChip>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Dice.GamblingDie>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Gambler.Weapons.Cards.ShuffleCard>());
			nextSlot++;

			if (!player.ZoneBeach && !player.ZoneCorrupt && !player.ZoneSkyHeight && !player.ZoneCrimson && !player.ZoneHoly && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneDesert && !player.ZoneGlowshroom && player.ZoneOverworldHeight)
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

			if (OrchidModGamblerHelper.checkSetCardsInDeck(modPlayer, "Slime") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.SlimyLollipop>());
				nextSlot++;
			}
			if (OrchidModGamblerHelper.checkSetCardsInDeck(modPlayer, "Biome") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.LuckySprout>());
				nextSlot++;
			}
			if (OrchidModGamblerHelper.checkSetCardsInDeck(modPlayer, "Boss") > 2)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Gambler.Accessories.ConquerorsPennant>());
				nextSlot++;
			}
			if (OrchidModGamblerHelper.checkSetCardsInDeck(modPlayer, "Elemental") > 2)
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
