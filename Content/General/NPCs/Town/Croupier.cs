using Microsoft.Xna.Framework;
using OrchidMod.Assets;
using OrchidMod.Common;
using OrchidMod.Content.Gambler;
using OrchidMod.Content.Gambler.Accessories;
using OrchidMod.Content.Gambler.Decks;
using OrchidMod.Content.Gambler.Weapons.Cards;
using OrchidMod.Content.Gambler.Weapons.Chips;
using OrchidMod.Content.Gambler.Weapons.Dice;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.NPCs.Town
{
	[AutoloadHead]
	public class Croupier : ModNPC
	{
		public override string Texture => OrchidAssets.NPCsPath + Name;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Croupier");

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
			happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
			happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Love);
			happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike);
			happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Hate);
			happiness.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Love);
			happiness.SetNPCAffection<Chemist>(AffectionLevel.Like);
			happiness.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Dislike);
			happiness.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate);

			var drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				// Sets your NPC's flavor text in the bestiary
				new FlavorTextBestiaryInfoElement("The Croupier helps gamblers sustaining their unique way of life. He is purported so lucky, he once won by rolling a 7 on a six-sided die.")
			});
		}

		public override ITownNPCProfile TownNPCProfile()
			=> new CroupierProfile();

		public override bool CanTownNPCSpawn(int numTownNPCs)
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

		public void ClickFirstButton(ref string shop)
		{
			var ui = CroupierUI.Instance;

			if (ui.Visible) ui.Deactivate(NPC.whoAmI);
			else shop = "Shop";
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

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			if (firstButton)
			{
				ClickFirstButton(ref shopName);
				return;
			}

			ClickSecondButton();
		}

		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, "Shop");
			npcShop.Add<GamblerDummy>();
			npcShop.Add<GamblingChip>();
			npcShop.Add<GamblingDie>();
			npcShop.Add<ShuffleCard>();
			npcShop.Add<ForestCard>([Condition.InShoppingZoneForest]);
			npcShop.Add<SnowCard>([Condition.InSnow]);
			npcShop.Add<DesertCard>([Condition.InDesert]);
			npcShop.Add<OceanCard>([Condition.InBeach]);
			npcShop.Add<JungleCard>([Condition.InJungle]);
			npcShop.Add<MushroomCard>([Condition.InGlowshroom]);
			npcShop.Add<SlimeRainCard>([OrchidConditions.SlimeRain]);
			npcShop.Add<LuckySprout>([OrchidConditions.GamblerSetBiome]);
			npcShop.Add<ConquerorsPennant>([OrchidConditions.GamblerSetBoss]);
			npcShop.Add<ElementalLens>([OrchidConditions.GamblerSetElemental]);
			npcShop.Register();
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
