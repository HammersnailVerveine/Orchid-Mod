using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Gambler.Accessories;
using OrchidMod.Shaman.Accessories;
using OrchidMod.Shaman.Misc;
using OrchidMod.Shaman.Weapons;
using OrchidMod.Shaman.Weapons.Thorium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.Globals.NPCs
{
	public class NPCLootAndShop : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			switch (type)
			{
				case NPCID.WitchDoctor:
					{
						AddItemToShop<ShamanRod>(shop, ref nextSlot);

						if (Main.hardMode) AddItemToShop<RitualScepter>(shop, ref nextSlot);
					}
					break;
				case NPCID.Demolitionist:
					AddItemToShop<GunpowderFlask>(shop, ref nextSlot);
					break;
				case NPCID.Dryad:
					AddItemToShop<DryadsGift>(shop, ref nextSlot);
					break;
			}

			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod == null) return;

			if (thoriumMod.TryFind("ConfusedZombie", out ModNPC modNPC) && type == modNPC.Type)
			{
				AddItemToShop<PatchWerkScepter>(shop, ref nextSlot);
			}
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			AddItemToShop<PileOfChips>(shop, ref nextSlot, 3);
		}

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			/*if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom && Main.hardMode && Main.rand.Next(100) == 0)
			{
				Item.NewItem(npc.GetSource_DropAsItem() npc.getRect(), ModContent.ItemType<ShroomKey>());
			}
			if (this.alchemistHit && Main.rand.Next(4) == 0)
			{
				Item.NewItem(npc.getRect(), ModContent.ItemType<Potency>());
			}
			if (this.gamblerHit && Main.rand.Next(4) == 0)
			{
				Item.NewItem(npc.getRect(), ModContent.ItemType<Chip>());
			}*/
		}

		public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
		{
			/*if (npc.type == 21 || npc.type == -46 || npc.type == -47 || npc.type == 201 || npc.type == -48 || npc.type == -49 || npc.type == 202 || npc.type == -50 || npc.type == -51 || npc.type == 203 || npc.type == -52 || npc.type == -53 || npc.type == 167)
			{ // Skeletons & vikings in mineshaft
				Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
				int MSMinPosX = (Main.maxTilesX / 2) - ((OrchidMSarrays.MSLenght * 15) / 2);
				int MSMinPosY = (Main.maxTilesY / 3 + 100);
				Rectangle rect = new Rectangle(MSMinPosX, MSMinPosY, (OrchidMSarrays.MSLenght * 15), (OrchidMSarrays.MSHeight * 14));
				if (rect.Contains(new Point((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f))))
				{
					Item.NewItem((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), Main.rand.Next(3) + 1, false, 0, false, false);
				}
			}

			if ((npc.type == NPCID.Hornet) || (npc.type == NPCID.HornetFatty) || (npc.type == NPCID.HornetHoney) || (npc.type == NPCID.HornetLeafy) || (npc.type == NPCID.HornetSpikey) || (npc.type == NPCID.HornetStingy))
			{
				if (Main.rand.Next(30) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.PoisonSigil>());
				}

				if (Main.rand.Next(20) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Nature.PoisonVial>());
				}
			}

			if (npc.type == 26 || npc.type == 27 || npc.type == 28 || npc.type == 29 || npc.type == 111) // Goblins
			{
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>());
				}
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.GoblinArmyCard>());
				}
			}

			if (npc.type == 490 || npc.type == 489) // Drippler / Blood Zombie
			{
				if (Main.rand.Next(40) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.BloodMoonFlask>());
				}
			}

			if (npc.type == 285 || npc.type == 286) // Diabolists
			{
				if (Main.rand.Next(20) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.DiabolistRune>());
				}
			}

			if (npc.type == 204) // Spiked Jungle Slime
			{
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.JungleSlimeCard>());
				}
			}

			if (npc.type == 59) // Lava Slime
			{
				if (Main.rand.Next(25) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.LavaSlimeCard>());
				}
			}

			if (npc.type == 1 || npc.type == -3 || npc.type == -8 || npc.type == -9 || npc.type == -6 || npc.type == 147 || npc.type == -10) // Most Surface Slimes
			{
				if (Main.rand.Next(!OrchidWorld.foundSlimeCard ? 5 : 1000) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.SlimeCard>());
					OrchidWorld.foundSlimeCard = true;
				}
			}

			if (npc.type == 87) // Wyvern Head
			{
				if (Main.rand.Next(15) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.WyvernTailFeather>());
				}
			}

			if (npc.type == 167) // Undead Viking
			{
				if (Main.rand.Next(30) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FrostburnSigil>());
				}
			}

			if (npc.type == 62) // Demon
			{
				if (Main.rand.Next(30) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FurnaceSigil>());
				}

				if (Main.rand.Next(20) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Accessories.DemonicPocketMirror>());
				}
			}

			if ((npc.type == NPCID.DarkCaster))
			{
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Blum>());
				}

				if (Main.rand.Next(33) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.DungeonCard>());
				}
			}

			if ((npc.type == NPCID.FireImp))
			{
				if (Main.rand.Next(20) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.MeltedRing>());
				}
			}

			if ((npc.type == NPCID.Mimic))
			{
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.HeavyBracer>());
				}
			}

			if (npc.type == 395 || npc.type == 392) // MARTIAN SAUCER
			{
				if (Main.rand.Next(4) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.MartianBeamer>());
				}
			}

			if ((npc.type == NPCID.IceQueen))
			{
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.IceFlakeCone>());
				}
			}

			if ((npc.type == NPCID.RuneWizard))
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.RuneScepter>());
			}

			if ((npc.type == NPCID.PirateShip))
			{
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.PiratesGlory>());
				}
				if (Main.rand.Next(10) == 0)
				{
					if (Main.rand.Next(20) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Decks.DeckDog>());
					}
					else
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Decks.DeckPirate>());
					}
				}
			}

			if ((npc.type == NPCID.GoblinSummoner))
			{
				if (Main.rand.Next(3) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.GoblinStick>());
				}
			}

			if ((npc.type == NPCID.Lihzahrd || npc.type == 199)) // Lihzahrds
			{
				if (Main.rand.Next(4) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Misc.LihzahrdSilk>());
				}
				if (Main.rand.Next(100) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.SunPriestTorch>());
				}
				if (Main.rand.Next(300) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.SunPriestBelt>());
				}
			}

			if (npc.type == NPCID.MourningWood)
			{
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.MourningTorch>());
				}
			}

			if (npc.type == 346) // SANTANK
			{
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FragilePresent>());
				}
			}

			if (npc.type == NPCID.UndeadMiner)
			{
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.TreasuredBaubles>());
				}
			}

			if ((npc.type == NPCID.BlackRecluse || npc.type == NPCID.BlackRecluseWall)) // Black Recluse (ground/wall)
			{
				if (Main.rand.Next(40) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.VenomSigil>());
				}
			}

			if (npc.type == NPCID.Mimic && npc.frame.Y >= 828 && npc.frame.Y <= 1104) //Ice Mimic
			{
				if (Main.rand.Next(3) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.IceMimicScepter>());
				}
			}

			if (NPC.downedBoss1 == true)
			{
				if ((npc.type == NPCID.Harpy))
				{
					if (Main.rand.Next(5) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Misc.HarpyTalon>());
					}
				}
			}

			if ((npc.type == 347)) // Elf Copter
			{
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<General.Items.Misc.RCRemote>());
				}
			}

			if ((npc.type == NPCID.CultistBoss))
			{
				int rand;
				if (Main.expertMode)
					rand = Main.rand.Next(73) + 18;
				else
					rand = Main.rand.Next(49) + 12;
				Item.NewItem((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Shaman.Misc.AbyssFragment>(), rand, false, 0, false, false);
			}
			/*if (npc.aiStyle == 94) // Celestial Pillar AI 
			{
				float numberOfPillars = 4;
				int quantity = (int)(Main.rand.Next(25, 41) / 2 / numberOfPillars);
				if (Main.expertMode) quantity = (int)(quantity * 1.5f);

				for (int i = 0; i < quantity; i++)
				{
					Item.NewItem((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ModContent.ItemType<Shaman.Misc.AbyssFragment>(), Main.rand.Next(1, 4), false, 0, false, false);
				}
			}*//*

			// BOSSES
			if ((npc.type == NPCID.QueenBee))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(2) == 0)
					{
						if (Main.rand.Next(2) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.QueenBeeCard>());
						}
						else
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Dice.HoneyDie>());
						}
					}
					if (Main.rand.Next(2) == 0)
					{
						int rand = Main.rand.Next(3);
						if (rand == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.BeeSeeker>());
						}
						else if (rand == 1)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.WaxyVial>());
						}
						else
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Air.QueenBeeFlask>());
						}
					}
				}
				if (alchemistHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			if ((npc.type == NPCID.MoonLordCore))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(5) == 0)
					{
						if (Main.rand.Next(2) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.Nirvana>());
						}
						else
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.TheCore>());
						}
					}
				}
			}

			if ((npc.type == NPCID.WallofFlesh))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(4) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.ShamanEmblem>());
					}
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<General.Items.Misc.OrchidEmblem>());
				}
			}

			if (npc.type == 50) // King Slime
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.KingSlimeFlask>());
					}
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.KingSlimeCard>());
					}
				}
				if (alchemistHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if ((npc.type == NPCID.Plantera))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(3) == 0)
					{
						if (Main.rand.Next(2) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.BulbScepter>());
						}
						else
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FloralStinger>());
						}
					}
				}
			}

			if ((npc.type == NPCID.Golem))
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(7) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.SunRay>());
					}
				}
			}

			if ((npc.type == 4))  // Eye of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.EyeCard>());
					}
				}
				if (alchemistHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}

			if ((npc.type == 35))  // Skeletron
			{
				if (!Main.expertMode)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.SkeletronCard>());
				}
				if (alchemistHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier3>());
				}
			}

			if ((npc.type == 266))  // Brain of Chtulhu
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Accessories.PreservedCrimson>());
					}
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.BrainCard>());
					}
				}
				if (alchemistHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			if (Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1 && npc.boss)
			{
				if (!Main.expertMode)
				{
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Accessories.PreservedCorruption>());
					}
					if (Main.rand.Next(3) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.EaterCard>());
					}
				}
				if (alchemistHit)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}

			//THORIUM

			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				if ((npc.type == thoriumMod.Find<ModNPC>("TheGrandThunderBirdv2").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(4) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.ThunderScepter>());
						}
					}
					if (alchemistHit)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("QueenJelly").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.QueenJellyfishScepter>());
						}
					}
					if (alchemistHit)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("GranityEnergyStorm").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.GraniteEnergyScepter>());
						}
					}
					if (alchemistHit)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("Viscount").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(7) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.ViscountScepter>());
						}
						Item.NewItem((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Shaman.Misc.Thorium.ViscountMaterial>(), 30, false, 0, false, false);
					}
					if (alchemistHit)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("ThePrimeScouter").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(6) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.StarScouterScepter>());
						}
					}
					if (alchemistHit)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("FallenDeathBeholder").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.CoznixScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("BoreanStriderPopped").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("Lich").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(7) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.LichScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("Abyssion").Type) || (npc.type == thoriumMod.Find<ModNPC>("AbyssionCracked").Type) || (npc.type == thoriumMod.Find<ModNPC>("AbyssionReleased").Type))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(6) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>());
						}
					}
				}

				if ((npc.type == thoriumMod.Find<ModNPC>("PatchWerk").Type))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.PatchWerkScepter>());
				}
			}
			else
			{
				if ((npc.type == NPCID.Mothron))
				{
					if (Main.rand.Next(4) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Misc.BrokenHeroScepter>());
					}
				}

				if ((npc.type == NPCID.Vulture))
				{
					int rand = Main.rand.Next(2) + 1;
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Misc.VultureTalon>(), rand);
				}
			}*/
		}

		// ...

		private static void AddItemToShop<T>(Chest shop, ref int nextSlot, int chanceDenominator = 1) where T : ModItem
			=> AddItemToShop(shop, ref nextSlot, ModContent.ItemType<T>(), chanceDenominator);

		private static void AddItemToShop(Chest shop, ref int nextSlot, int type, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;

			shop.item[nextSlot].SetDefaults(type);
			nextSlot++;
		}

		private static void AddItemToShop<T>(int[] shop, ref int nextSlot, int chanceDenominator = 1) where T : ModItem
			=> AddItemToShop(shop, ref nextSlot, ModContent.ItemType<T>(), chanceDenominator);

		private static void AddItemToShop(int[] shop, ref int nextSlot, int type, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;

			shop[nextSlot] = type;
			nextSlot++;
		}
	}
}