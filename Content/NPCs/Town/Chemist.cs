using OrchidMod.Alchemist;
using OrchidMod.Alchemist.Accessories;
using OrchidMod.Alchemist.Bag;
using OrchidMod.Alchemist.Misc;
using OrchidMod.Alchemist.Misc.Scrolls;
using OrchidMod.Alchemist.Weapons.Catalysts;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Content.Items.Materials;
using OrchidMod.NPCs.Town;
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
	public class Chemist : ModNPC
	{
		public override string Texture => OrchidAssets.NPCsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chemist");

			Main.npcFrameCount[Type] = 23;

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4;

			// They were chosen randomly, so it's better to choose them yourself
			var happiness = NPC.Happiness;
			happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Like);
			happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
			happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike);
			happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
			happiness.SetNPCAffection(NPCID.Nurse, AffectionLevel.Love);
			happiness.SetNPCAffection<Croupier>(AffectionLevel.Like);
			happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike);
			happiness.SetNPCAffection(NPCID.Clothier, AffectionLevel.Hate);

			var drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			void CreateMoodTranslation(string key, string text)
			{
				var tr = LocalizationLoader.CreateTranslation(Mod, "TownNPCMood.Chemist." + key);
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

			AnimationType = NPCID.Mechanic;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				// Sets your NPC's flavor text in the bestiary
				new FlavorTextBestiaryInfoElement("Eula C6 WHEN???")
			});
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
			=> OrchidWorld.foundChemist;

		public override ITownNPCProfile TownNPCProfile()
			=> new ChemistProfile();

		public override List<string> SetNPCNameList()
		{
			return new()
			{
				"Elodie",
				"Annick",
				"Samantha",
				"Ambre",
				"Maeva",
				"Sophie",
				"Florianne",
				"Juliette",
				"Chloe",
				"Amandine",
				"Alicia"
			};
		}

		public override string GetChat()
		{
			if (Main.bloodMoon)
			{
				return Main.rand.Next(4) switch
				{
					0 => "Want your blood donated to science? No? Then leave.",
					1 => "Of course you can't use normal bottles for alchemy! Give me money!",
					2 => "Ugh, what, you want to blow yourself up or something?",
					_ => "DON'T get stains on anything! I don't care if I already did!",
				};
			}

			return Main.rand.Next(8 + (Main.dayTime ? 1 : 2)) switch
			{
				0 => "So you're interested in alchemy AND haven't exploded yet? Impressive.",
				1 => "Always wear your gloves! By the way, have you seen my gloves?",
				2 => "You know, you can use a centrifuge for stain removal! Kind of.",
				3 => "You can be a catalyst for science! Or a pile of ash. Try to keep priorities.",
				4 => "Alchemy makes the world go around... current scientific theories purport.",
				5 => "Safety warning: do not drink alchemical compounds. Unless I can take notes.",
				6 => "Some people call me a reactionary.",
				7 => "Alchemy is just sublime!",
				8 => Main.dayTime ? "Life is just one big reaction!" : "Undeath is just one big reaction! I don't know why the moon does that.",
				9 => "Do you think the moon... is made of attractite?!",
				_ => "The scientific method is... try until it doesn't explode!",
			};
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidAlchemist>();

			button = Language.GetTextValue("LegacyInterface.28");
			button2 = CheckUniqueHints(player, modPlayer, false) == "" ? "Reaction Hint" : "Special Hint!";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			var player = Main.player[Main.myPlayer];
			var modPlayer = player.GetModPlayer<OrchidAlchemist>();

			if (firstButton)
			{
				shop = true;
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
								0 => $"Here's your daily recipe... note that it itself is not an ingredient.",
								1 => $"There you go, try not to lose a hand with this one.",
								2 => $"I didn't take the time to try that one. If I don't see you tomorrow, I'll assume it doesn't work.",
								3 => $"And one perfectly fine recipe, completely free of charge! Or is it?",
								4 => $"That one should work. Maybe. Possibly. I wouldn't put you in danger, would I?",
								_ => $"If it's free, you're probably the product, you say? Naaah... please, just don't try it in the vicinity.",
							};
						}
						else
						{
							Main.npcChatText = $"I need time to think about more recipes. Maybe can you go and kill a few baddies, meanwhile?";
						}
					}
					else
					{
						Main.npcChatText = Main.rand.Next(3) switch
						{
							0 => $"Over-exposure to my brilliant ideas may be dangerous for a trainee.",
							1 => $"I already gave you one today, why don't you try and come up with your own recipes?",
							_ => $"There's nothing I need you to test for me right now. Wait, what did I just say?",
						};
					}
				}
				else
				{
					Main.npcChatText = uniqueStr;
				}
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			var player = Main.LocalPlayer;
			var modPlayer = player.GetModPlayer<OrchidAlchemist>();

			NPCUtils.AddItemToShop<UIItem>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<UIItemKeys>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<ReactionItem>(shop, ref nextSlot);

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<PotionBag>());
			(shop.item[nextSlot].ModItem as PotionBag).InShop = true;
			nextSlot++;

			NPCUtils.AddItemToShop<EmptyFlask>(shop, ref nextSlot);

			if (NPC.downedQueenBee)
			{
				NPCUtils.AddItemToShop<AlchemicStabilizer>(shop, ref nextSlot);
			}

			NPCUtils.AddItemToShop<WeightedBottles>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<IronCatalyst>(shop, ref nextSlot);
			NPCUtils.AddItemToShop<AttractiteFlask>(shop, ref nextSlot);

			if (modPlayer.alchemistKnownReactions.Count > 4)
			{
				NPCUtils.AddItemToShop<ReactiveVials>(shop, ref nextSlot);
			}
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

				foreach (AlchemistHiddenReactionRecipe recipe in OrchidMod.alchemistReactionRecipes)
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
				}
				return "Since this is your first time asking, here are three hints for the price of one! I'd highly advise getting your hands on a hidden reactions codex, now...";
			}

			if (!reactions.Contains("RecipeMushroomThread")
			&& (player.HasItem(183) || player.HasItem(ModContent.ItemType<GlowingMushroomVial>())))
			{
				if (buttonClicked)
				{
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -2, false);
				}
				return "I see you found some glowing mushrooms. Did you know that you can make a pretty good thread with them? Here's the recipe.";
			}

			if (!reactions.Contains("RecipeJungleLilyPurification")
			&& (player.HasItem(ModContent.ItemType<JungleLily>()) || player.HasItem(ModContent.ItemType<JungleLilyFlask>())))
			{
				if (buttonClicked)
				{
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -4, false);
				}
				return "Jungle lilies, I love these flowers! I bet you're wondering how to make them bloom ain't you? Here's the solution.";
			}

			return "";
		}
	}
}