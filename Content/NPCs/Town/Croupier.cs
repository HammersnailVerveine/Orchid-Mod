using Microsoft.Xna.Framework;
using OrchidMod.Gambler;
using OrchidMod.Gambler.Accessories;
using OrchidMod.Gambler.Decks;
using OrchidMod.Gambler.Weapons.Cards;
using OrchidMod.Gambler.Weapons.Chips;
using OrchidMod.Gambler.Weapons.Dice;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.NPCs.Town
{
	[AutoloadHead]
	public class Croupier : ModNPC
	{
		public override string Texture => OrchidAssets.NPCsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Croupier");

			Main.npcFrameCount[NPC.type] = 26;

			NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
			NPCID.Sets.AttackFrameCount[NPC.type] = 4;
			NPCID.Sets.DangerDetectRange[NPC.type] = 700;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;

			// They were chosen randomly, so it's better to choose them yourself
			var happiness = NPC.Happiness;
			happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Like);
			happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
			happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike);
			happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
			happiness.SetNPCAffection(NPCID.Nurse, AffectionLevel.Love);
			happiness.SetNPCAffection<Chemist>(AffectionLevel.Like);
			happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike);
			happiness.SetNPCAffection(NPCID.Clothier, AffectionLevel.Hate);

			var drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			void CreateMoodTranslation(string key, string text)
			{
				var tr = LocalizationLoader.CreateTranslation(Mod, "TownNPCMood.Croupier." + key);
				tr.SetDefault(text);
				LocalizationLoader.AddTranslation(tr);
			}

			// Text is taken from Example Mod
			CreateMoodTranslation("Content", "I feel pretty fine right now.");
			CreateMoodTranslation("NoHome", "I would very much like a house, all the colorful monsters scare me.");
			CreateMoodTranslation("LoveSpace", "I love how there is so much space here to code tModLoader mods!");
			CreateMoodTranslation("FarFromHome", "Could you please get me back to my house?");
			CreateMoodTranslation("DislikeCrowded", "There are too many people around, it makes it hard for me to focus on mod making.");
			CreateMoodTranslation("HateCrowded", "I can't test my mod with so many people around!");
			CreateMoodTranslation("LikeBiome", "{BiomeName} is a very nice place to test mods in.");
			CreateMoodTranslation("LoveBiome", "I love {BiomeName}.");
			CreateMoodTranslation("DislikeBiome", "It's way too cold in {BiomeName}, I'm freezing!");
			CreateMoodTranslation("HateBiome", "Its kind of hard to mod while being attacked by monsters in {BiomeName}.");
			CreateMoodTranslation("LikeNPC", "I can respect {NPCName} as a fellow guide and educator!");
			CreateMoodTranslation("LoveNPC", "Do you think {NPCName} notices me?");
			CreateMoodTranslation("DislikeNPC", "{NPCName} keeps rambling on about ...");
			CreateMoodTranslation("HateNPC", "I hate all the loud noises caused by {NPCName} and his explosives! I just want peace and quiet.");
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

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				// Sets your NPC's flavor text in the bestiary
				new FlavorTextBestiaryInfoElement("Kokomi C6 WHEN???")
			});
		}

		public override ITownNPCProfile TownNPCProfile()
			=> new CroupierProfile();

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

		public override string GetChat()
		{
			return Main.rand.Next(7) switch
			{
				0 => "Cards turnin' your way?",
				1 => "I like them odds, chief. Always like them odds.",
				2 => "Got you covered on cards, chief. We got poker, solitaire... more poker...",
				3 => "Ey, chief, best bud! Since we're such great pals, could ya help with a little debt?",
				4 => "I'm your rear hand man.",
				5 => "Fate's face down. How do you wanna be dealt?",
				_ => "Pick a number between 1 and 86!",
			};
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidGambler>();
			var ui = CroupierUI.Instance;

			if (!ui.Visible)
			{
				string deckBuilding = $"[c/{Colors.AlphaDarken(new Color(255, 200, 0)).Hex3()}:Deck Building]";

				button2 = modPlayer.HasGamblerDeck() ? deckBuilding : "Get a New Deck";
				button = Language.GetTextValue("LegacyInterface.28");
			}
			else
			{
				button = "Return";
			}
		}

		public void ClickFirstButton(ref bool shop)
		{
			var ui = CroupierUI.Instance;

			if (ui.Visible) ui.Deactivate(NPC.whoAmI);
			else shop = true;
		}

		public void ClickSecondButton()
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidGambler>();
			var ui = CroupierUI.Instance;

			if (modPlayer.HasGamblerDeck())
			{
				Main.npcChatText = $"Not too fond of your odds, eh? Aight, go on.";
				Main.npcChatText += "\n\n\n\n\n\n\n";

				ui.Activate();
				return;
			}

			for (int i = 0; i < 50; i++)
			{
				var item = Main.LocalPlayer.inventory[i];

				if (item.type.Equals(ItemID.None))
				{
					Main.npcChatText = $"You lost it already? Here chief, take your new deck.";
					int gamblerDeck = ModContent.ItemType<GamblerAttack>();

					player.QuickSpawnItem(NPC.GetSource_FromThis(), gamblerDeck, 1);
					return;
				}
			}

			Main.npcChatText = $"My man, your pockets are full. You wouldn't let a brand new deck sitting on the ground, would ya?";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				ClickFirstButton(ref shop);
				return;
			}

			ClickSecondButton();
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidGambler>();

			NPCUtils.AddItemToShop<GamblerDummy>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<GamblingChip>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<GamblingDie>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<ShuffleCard>(shop, ref nextSlot);

			if (player.ZoneForest)
			{
				NPCUtils.AddItemToShop<ForestCard>(shop, ref nextSlot);
			}

			if (player.ZoneSnow)
			{
				NPCUtils.AddItemToShop<SnowCard>(shop, ref nextSlot);
			}

			if (player.ZoneDesert)
			{
				NPCUtils.AddItemToShop<DesertCard>(shop, ref nextSlot);
			}

			if (player.ZoneBeach)
			{
				NPCUtils.AddItemToShop<OceanCard>(shop, ref nextSlot);
			}

			if (player.ZoneJungle)
			{
				NPCUtils.AddItemToShop<JungleCard>(shop, ref nextSlot);
			}

			if (player.ZoneGlowshroom)
			{
				NPCUtils.AddItemToShop<MushroomCard>(shop, ref nextSlot);
			}

			if (player.ZoneUnderworldHeight)
			{
				NPCUtils.AddItemToShop<HellCard>(shop, ref nextSlot);
			}

			if (player.ZoneSkyHeight)
			{
				NPCUtils.AddItemToShop<SkyCard>(shop, ref nextSlot);
			}

			if (Main.slimeRain)
			{
				NPCUtils.AddItemToShop<SlimeRainCard>(shop, ref nextSlot);
			}

			if (modPlayer.CheckSetCardsInDeck("Slime") > 2)
			{
				NPCUtils.AddItemToShop<SlimyLollipop>(shop, ref nextSlot);
			}

			if (modPlayer.CheckSetCardsInDeck("Biome") > 2)
			{
				NPCUtils.AddItemToShop<LuckySprout>(shop, ref nextSlot);
			}

			if (modPlayer.CheckSetCardsInDeck("Boss") > 2)
			{
				NPCUtils.AddItemToShop<ConquerorsPennant>(shop, ref nextSlot);
			}

			if (modPlayer.CheckSetCardsInDeck("Elemental") > 2)
			{
				NPCUtils.AddItemToShop<ElementalLens>(shop, ref nextSlot);
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
			=> true;

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
			projType = ModContent.ProjectileType<CroupierProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}
