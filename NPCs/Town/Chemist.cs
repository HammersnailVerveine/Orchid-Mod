using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Town
{
	// [AutoloadHead] and npc.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Chemist : ModNPC
	{
		public override string Texture => "OrchidMod/NPCs/Town/Chemist";

		public override string[] AltTextures => new[] { "OrchidMod/NPCs/Town/Chemist_Alt_1" };

		public override bool Autoload(ref string name) {
			name = "Chemist";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults() {
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			DisplayName.SetDefault("Chemist");
			Main.npcFrameCount[npc.type] = 23;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults() {
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
			animationType = NPCID.Mechanic;
		}

		// public override void HitEffect(int hitDirection, double damage) {
			// int num = npc.life > 0 ? 1 : 5;
			// for (int k = 0; k < num; k++) {
				// Dust.NewDust(npc.position, npc.width, npc.height, 6);
			// }
		// }

		public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
			return OrchidWorld.foundChemist;
		}
		
		// public override void AI() {
			// if (!OrchidWorld.foundChemist) {
				// OrchidWorld.foundChemist = true;
			// }
		// }

		public override string TownNPCName() {
			switch (WorldGen.genRand.Next(11)) {
				case 0:
					return "Elodie";
				case 1:
					return "Annick";
				case 2:
					return "Samantha";
				case 3:
					return "Ambre";
				case 4:
					return "Maeva";
				case 5:
					return "Sophie";
				case 6:
					return "Florianne";
				case 7:
					return "Juliette";
				case 8:
					return "Chloe";
				case 9:
					return "Amandine";
				default:
					return "Alicia";
			}
		}

		public override void FindFrame(int frameHeight) {
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

		public override string GetChat() {
			if (Main.bloodMoon) {
				switch (Main.rand.Next(4)) {
					case 0:
						return "Want your blood donated to science? No? Then leave.";
					case 1:
						return "Of course you can't use normal bottles for alchemy! Give me money!";
					case 2:
						return "Ugh, what, you want to blow yourself up or something?";
					default:
						return "DON'T get stains on anything! I don't care if I already did!";
				}
			} else {
				switch (Main.rand.Next(8 + (Main.dayTime ? 1 : 2))) {
					case 0:
						return "So you're interested in alchemy AND haven't exploded yet? Impressive.";
					case 1:
						return "Always wear your gloves! By the way, have you seen my gloves?";
					case 2:
						return "You know, you can use a centrifuge for stain removal! Kind of.";
					case 3:
						return "You can be a catalyst for science! Or a pile of ash. Try to keep priorities.";
					case 4:
						return "Alchemy makes the world go around... current scientific theories purport.";
					case 5:
						return "Safety warning: do not drink alchemical compounds. Unless I can take notes.";
					case 6:
						return "Some people call me a reactionary.";
					case 7:
						return "Alchemy is just sublime!";
					case 8:
						return Main.dayTime ? "Life is just one big reaction!" : "Undeath is just one big reaction! I don't know why the moon does that.";
					case 9:
						return "Do you think the moon... is made of attractite?!";
					default:
						return "The scientific method is... try until it doesn't explode!";
				}
			}
		}

		public override void SetChatButtons(ref string button, ref string button2) {
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = modPlayer.alchemistChemistFirstInteraction ? "Reaction Hint" : "Alchemy Basics";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop) {
			Player player = Main.player[Main.myPlayer];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (firstButton) {
				shop = true;
			}
			else {
				if (!modPlayer.alchemistChemistFirstInteraction) {
					Main.npcChatText = $"Since it's your first time talking to me, I will give you a reminder to check your modded keybinds, the hidden reaction system is really important for an alchemist!  Talk to me again if you want hints to find some of them based on your progression.";
					modPlayer.alchemistChemistFirstInteraction = true;
				} else {
					switch (Main.rand.Next(5)) {
						case 0:
							Main.npcChatText = $"If you happen to need some corrosion-resistant lab equipment, glowing mushroom extract makes an excellent plastic filament when mixed with bioluminescent root. Don't expect it to sell for much, though. That mushroom smell never washes off.";
							break;
						case 1:
							Main.npcChatText = $"What keeps clouds in the sky? Who knows! How could you get one up there? Gunpowder, probably. Got any clouds to try it with?";
							break;
						case 2:
							Main.npcChatText = $"You know, brewing something drinkable isn't exactly rocket science. You could probably do it on the fly with the right ingredients.";
							break;
						case 3:
							Main.npcChatText = $"Attractite has some interesting chemistry with plant matter, seeming to function as some form of hormonal accelerant. Just keep it outside of my room. Getting all of those spores out of my hair was a nightmare...";
							break;
						default:
							Main.npcChatText = $"Clouds are pretty useful for bubbling up a reaction quicker! Or a bath. I could use a nice bath.";
							break;
					}
				}
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot) {
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.UIItem>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Misc.ReactionItem>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("EmptyFlask"));
			nextSlot++;
			if (NPC.downedQueenBee) {
			shop.item[nextSlot].SetDefaults(mod.ItemType("AlchemicStabilizer"));
			nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Accessories.WeightedBottles>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("AttractiteFlask"));
			nextSlot++;
		}

		// public override void NPCLoot() {
			// Item.NewItem(npc.getRect(), ItemType<Items.Armor.ExampleCostume>());
		// }

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toQueenStatue) {
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

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileType<NPCs.Town.Projectiles.ChemistProjectile>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}
