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
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Bestiary"))
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
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.1"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.2"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.3"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.4"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.5"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.6"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.7"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.8"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.9"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.10"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Names.11")
			};
		}

		public override string GetChat()
		{
			return Main.rand.Next(7) switch
			{
				0 => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.1"),
				1 => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.2"),
				2 => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.3"),
				3 => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.4"),
				4 => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.5"),
				5 => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.6"),
				_ => Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.7"),
			};
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidGambler>();
			var ui = CroupierUI.Instance;

			if (!ui.Visible)
			{
				string deckBuilding = $"[c/{Colors.AlphaDarken(new Color(255, 200, 0)).Hex3()}:" + Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.UI.DeckBuilding") + "]";

				button2 = modPlayer.HasGamblerDeck() ? deckBuilding : Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.UI.NewDeck");
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
			else shop = Language.GetTextValue("LegacyInterface.28");
		}

		public void ClickSecondButton()
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidGambler>();
			var ui = CroupierUI.Instance;

			if (modPlayer.HasGamblerDeck())
			{
				Main.npcChatText = Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.8");
				Main.npcChatText += "\n\n\n\n\n\n\n";

				ui.Activate();
				return;
			}

			for (int i = 0; i < 50; i++)
			{
				var item = Main.LocalPlayer.inventory[i];

				if (item.type.Equals(ItemID.None))
				{
					Main.npcChatText = Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.9");
					int gamblerDeck = ModContent.ItemType<GamblerAttack>();

					player.QuickSpawnItem(NPC.GetSource_FromThis(), gamblerDeck, 1);
					return;
				}
			}

			Main.npcChatText = Language.GetTextValue("Mods.OrchidMod.NPCs.Croupier.Dialogues.10");
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
			var npcShop = new NPCShop(Type, Language.GetTextValue("LegacyInterface.28"));
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
			npcShop.Add<SkyCard>([Condition.InSkyHeight]);
			npcShop.Add<SlimeRainCard>([OrchidConditions.SlimeRain]);
			npcShop.Add<LuckySprout>([OrchidConditions.GamblerSetBiome]);
			npcShop.Add<ConquerorsPennant>([OrchidConditions.GamblerSetBoss]);
			npcShop.Add<ElementalLens>([OrchidConditions.GamblerSetElemental]);
			npcShop.Add<SlimyLollipop>([OrchidConditions.GamblerSetSlime]);
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
