using OrchidMod.Alchemist;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Town
{
	[AutoloadHead]
	public class Chemist : ModNPC
	{
		public override string Texture => OrchidAssets.NPCsPath + Name;
		// public override string[] AltTextures => new[] { OrchidAssets.NPCsPath + Name + "_Alt_1" };

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

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return OrchidWorld.foundChemist;
		}

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
			Player player = Main.player[Main.myPlayer];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = checkUniqueHints(player, modPlayer, false) == "" ? "Reaction Hint" : "Special Hint!";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			Player player = Main.player[Main.myPlayer];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			if (firstButton)
			{
				shop = true;
			}
			else
			{
				string uniqueStr = checkUniqueHints(player, modPlayer, true);
				if (uniqueStr == "")
				{
					if (!modPlayer.alchemistDailyHint)
					{
						if (this.getDailyHint(player, modPlayer))
						{
							modPlayer.alchemistDailyHint = true;
							switch (Main.rand.Next(6))
							{
								case 0:
									Main.npcChatText = $"Here's your daily recipe... note that it itself is not an ingredient.";
									break;
								case 1:
									Main.npcChatText = $"There you go, try not to lose a hand with this one.";
									break;
								case 2:
									Main.npcChatText = $"I didn't take the time to try that one. If I don't see you tomorrow, I'll assume it doesn't work.";
									break;
								case 3:
									Main.npcChatText = $"And one perfectly fine recipe, completely free of charge! Or is it?";
									break;
								case 4:
									Main.npcChatText = $"That one should work. Maybe. Possibly. I wouldn't put you in danger, would I?";
									break;
								default:
									Main.npcChatText = $"If it's free, you're probably the product, you say? Naaah... please, just don't try it in the vicinity.";
									break;
							}
						}
						else
						{
							Main.npcChatText = $"I need time to think about more recipes. Maybe can you go and kill a few baddies, meanwhile?";
						}
					}
					else
					{
						switch (Main.rand.Next(3))
						{
							case 0:
								Main.npcChatText = $"Over-exposure to my brilliant ideas may be dangerous for a trainee.";
								break;
							case 1:
								Main.npcChatText = $"I already gave you one today, why don't you try and come up with your own recipes?";
								break;
							default:
								Main.npcChatText = $"There's nothing I need you to test for me right now. Wait, what did I just say?";
								break;
						}
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
			Player player = Main.player[Main.myPlayer];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.UIItem>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.UIItemKeys>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.ReactionItem>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.PotionBagSimple>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.EmptyFlask>());
			nextSlot++;
			if (NPC.downedQueenBee)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.AlchemicStabilizer>());
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Accessories.WeightedBottles>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Weapons.Catalysts.IronCatalyst>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>());
			nextSlot++;
			if (modPlayer.alchemistKnownReactions.Count > 4)
			{
				shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Accessories.ReactiveVials>());
				nextSlot++;
			}
		}

		// public override void NPCLoot() {
		// Item.NewItem(npc.getRect(), ItemType<Items.Armor.ExampleCostume>());
		// }

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toQueenStatue)
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
			projType = ProjectileType<NPCs.Town.Projectiles.ChemistProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}

		public bool getDailyHint(Player player, OrchidAlchemist modPlayer)
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
					int scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier1>();
					switch (progression)
					{
						case 2:
							scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier2>();
							break;
						case 3:
							scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier3>();
							break;
						case 4:
							scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier4>();
							break;
						case 5:
							scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier5>();
							break;
						case 6:
							scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier6>();
							break;
						default:
							scrollType = ItemType<Alchemist.Misc.Scrolls.ScrollTier1>();
							break;
					}
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

		public string checkUniqueHints(Player player, OrchidAlchemist modPlayer, bool buttonClicked)
		{
			List<string> reactions = new List<string>(modPlayer.alchemistKnownReactions);
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
			&& (player.HasItem(183) || player.HasItem(ModContent.ItemType<Alchemist.Weapons.Nature.GlowingMushroomVial>())))
			{
				if (buttonClicked)
				{
					AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, -2, false);
				}
				return "I see you found some glowing mushrooms. Did you know that you can make a pretty good thread with them? Here's the recipe.";
			}

			if (!reactions.Contains("RecipeJungleLilyPurification")
			&& (player.HasItem(ModContent.ItemType<Content.Items.Materials.JungleLily>()) || player.HasItem(ModContent.ItemType<Alchemist.Weapons.Nature.JungleLilyFlask>())))
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
