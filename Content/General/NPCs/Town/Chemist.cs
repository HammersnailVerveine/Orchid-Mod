using OrchidMod.Assets;
using OrchidMod.Common;
using OrchidMod.Common.ModSystems;
using OrchidMod.Content.Alchemist;
using OrchidMod.Content.Alchemist.Accessories;
using OrchidMod.Content.Alchemist.Bag;
using OrchidMod.Content.Alchemist.Misc;
using OrchidMod.Content.Alchemist.Misc.Scrolls;
using OrchidMod.Content.Alchemist.Weapons.Catalysts;
using OrchidMod.Content.Alchemist.Weapons.Nature;
using OrchidMod.Content.General.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.NPCs.Town
{
	[AutoloadHead]
	public class Chemist : ModNPC
	{
		public override string Texture => OrchidAssets.NPCsPath + Name;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chemist");

			Main.npcFrameCount[Type] = 23;

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4;

			var happiness = NPC.Happiness;
			happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
			happiness.SetBiomeAffection<JungleBiome>(AffectionLevel.Love);
			happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike);
			happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
			happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love);
			happiness.SetNPCAffection<Croupier>(AffectionLevel.Like);
			happiness.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike);
			happiness.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate);

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

			AnimationType = NPCID.Mechanic;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				// Sets your NPC's flavor text in the bestiary
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Bestiary"))
			});
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
			=> OrchidWorld.foundChemist;

		public override ITownNPCProfile TownNPCProfile()
			=> new ChemistProfile();

		public override List<string> SetNPCNameList()
		{
			return new()
			{
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.1"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.2"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.3"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.4"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.5"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.6"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.7"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.8"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.9"),
				Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Names.10")
			};
		}

		public override string GetChat()
		{
			if (Main.bloodMoon)
			{
				return Main.rand.Next(4) switch
				{
					0 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.2"),
					1 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.3"),
					2 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.4"),
					_ => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.5"),
				};
			}

			return Main.rand.Next(8 + (Main.dayTime ? 1 : 2)) switch
			{
				0 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.6"),
				1 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.7"),
				2 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.8"),
				3 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.9"),
				4 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.10"),
				5 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.11"),
				6 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.12"),
				7 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.13"),
				8 => Main.dayTime ? Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.14") : Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.15"),
				9 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.16"),
				_ => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.17"),
			};
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidAlchemist>();

			button = Language.GetTextValue("LegacyInterface.28");
			button2 = CheckUniqueHints(player, modPlayer, false) == "" ? Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.UI.RHint") : Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.UI.SHint");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidAlchemist>();

			if (firstButton)
			{
				shopName = Language.GetTextValue("LegacyInterface.28");
			}
			else
			{
				string uniqueStr = CheckUniqueHints(player, modPlayer, true);

				if (uniqueStr == "")
				{
					if (!modPlayer.alchemistDailyHint)
					{
						if (GetDailyHint(player, modPlayer))
						{
							modPlayer.alchemistDailyHint = true;

							Main.npcChatText = Main.rand.Next(6) switch
							{
								0 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.18"),
								1 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.19"),
								2 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.20"),
								3 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.21"),
								4 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.22"),
								_ => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.23"),
							};
						}
						else
						{
							Main.npcChatText = Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.24");
						}
					}
					else
					{
						Main.npcChatText = Main.rand.Next(3) switch
						{
							0 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.25"),
							1 => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.26"),
							_ => Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.27"),
						};
					}
				}
				else
				{
					Main.npcChatText = uniqueStr;
				}
			}
		}

		public override void ModifyActiveShop(string shopName, Item[] items)
		{
			foreach (Item item in items)
			{
				if (item == null || item.type == ItemID.None)
				{
					continue;
				}

				if (item.type == ModContent.ItemType<PotionBag>())
				{
					(item.ModItem as PotionBag).InShop = true;
				}
			}
		}

		public override void AddShops()
		{
			NPCShop npcShop = new NPCShop(Type, Language.GetTextValue("LegacyInterface.28"));
			npcShop.Add<UIItem>();
			npcShop.Add<UIItemKeys>();
			npcShop.Add<ReactionItem>();
			npcShop.Add<PotionBag>();
			npcShop.Add<EmptyFlask>();
			npcShop.Add<WeightedBottles>();
			npcShop.Add<IronCatalyst>();
			npcShop.Add<AttractiteFlask>();
			npcShop.Add<AlchemicStabilizer>([Condition.DownedQueenBee]);
			npcShop.Add<ReactiveVials>([OrchidConditions.AlchemistKnownReactions4]);

			npcShop.Register();
		}

		public override bool CanGoToStatue(bool toQueenStatue)
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
			projType = ModContent.ProjectileType<ChemistProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}

		public bool GetDailyHint(Player player, OrchidAlchemist modPlayer)
		{
			int progression = modPlayer.GetProgressLevel();

			while (progression > 0)
			{
				bool validHint = false;

				foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.AlchemistReactionRecipes)
				{
					if (recipe.level == progression)
					{
						if (!(modPlayer.alchemistKnownReactions.Contains(recipe.typeName) || modPlayer.alchemistKnownHints.Contains(recipe.typeName)))
						{
							validHint = true;
							break;
						}
					}
				}

				if (validHint)
				{
					var scrollType = progression switch
					{
						1 => ModContent.ItemType<ScrollTier1>(),
						2 => ModContent.ItemType<ScrollTier2>(),
						3 => ModContent.ItemType<ScrollTier3>(),
						4 => ModContent.ItemType<ScrollTier4>(),
						5 => ModContent.ItemType<ScrollTier5>(),
						6 => ModContent.ItemType<ScrollTier6>(),
						_ => ModContent.ItemType<ScrollTier1>(),
					};

					player.QuickSpawnItem(NPC.GetSource_FromThis(), scrollType, 1);

					return true;
				}
				else
				{
					progression--;
				}
			}

			return false;
		}

		public string CheckUniqueHints(Player player, OrchidAlchemist modPlayer, bool buttonClicked)
		{
			List<string> reactions = new(modPlayer.alchemistKnownReactions);
			reactions.AddRange(modPlayer.alchemistKnownHints);

			if (!reactions.Contains("RecipeFireSpores")
			|| !reactions.Contains("RecipeWaterSpores")
			|| !reactions.Contains("RecipeAirSpores"))
			{
				if (buttonClicked)
				{
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -3, false);
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -3, false);
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -3, false);
					int flask = ModContent.ItemType<MushroomFlask>();
					player.QuickSpawnItem(NPC.GetSource_FromThis(), flask, 1);
				}
				return Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.28");
			}

			if (!reactions.Contains("RecipeMushroomThread")
			&& (player.HasItem(183) || player.HasItem(ModContent.ItemType<GlowingMushroomVial>())))
			{
				if (buttonClicked)
				{
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -2, false);
				}
				return Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.29");
			}

			if (!reactions.Contains("RecipeJungleLilyPurification")
			&& (player.HasItem(ModContent.ItemType<JungleLily>()) || player.HasItem(ModContent.ItemType<JungleLilyFlask>())))
			{
				if (buttonClicked)
				{
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -4, false);
				}
				return Language.GetTextValue("Mods.OrchidMod.NPCs.Chemist.Dialogues.30");
			}

			return "";
		}
	}
}