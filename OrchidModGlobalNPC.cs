using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.WorldgenArrays;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
    public class OrchidModGlobalNPC : GlobalNPC
    {
		public int shamanBomb = 0;
		public int shamanShroom = 0;
		public int shamanSpearDamage = 0;
		public int gamblerDungeonCardCount = 0;
		public bool shamanWater = false;
		public bool shamanWind = false;
		public bool alchemistHit = false;
		public bool gamblerHit = false;

		public override bool InstancePerEntity => true;

		public override void GetChat(NPC npc, ref string chat)
		{
			OrchidMod.Instance.croupierUI.Visible = false;
		}

		public override void NPCLoot(NPC npc)
        {
			if (npc.lifeMax > 5 && npc.value > 0f) {
				if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneGlowshroom && Main.hardMode && Main.rand.Next(100) == 0) {
					Item.NewItem(npc.getRect(), ItemType<General.Items.Misc.ShroomKey>());
				}
				if (this.alchemistHit && Main.rand.Next(4) == 0) {
					Item.NewItem(npc.getRect(), ItemType<Alchemist.Misc.Potency>());
				}
				if (this.gamblerHit && Main.rand.Next(4) == 0) {
					Item.NewItem(npc.getRect(), ItemType<Gambler.Misc.Chip>());
				}
			}
			
			if (npc.type == 21 || npc.type == -46 || npc.type == -47 || npc.type == 201 || npc.type == -48 || npc.type == -49 || npc.type == 202 || npc.type == -50 || npc.type == -51 || npc.type == 203 || npc.type == -52 || npc.type == -53 || npc.type == 167) { // Skeletons & vikings in mineshaft
				Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
				int MSMinPosX = (Main.maxTilesX / 2) - ((OrchidMSarrays.MSLenght * 15) / 2);
				int MSMinPosY = (Main.maxTilesY/3 + 100);
				Rectangle rect = new Rectangle(MSMinPosX, MSMinPosY, (OrchidMSarrays.MSLenght * 15), (OrchidMSarrays.MSHeight * 14));
				if (rect.Contains(new Point((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f)))) {
					Item.NewItem((int) npc.position.X + Main.rand.Next(npc.width), (int) npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), Main.rand.Next(3) + 1, false, 0, false, false);
				}
			}
			
			if ((npc.type == NPCID.Hornet) || (npc.type == NPCID.HornetFatty) || (npc.type == NPCID.HornetHoney) || (npc.type == NPCID.HornetLeafy) || (npc.type == NPCID.HornetSpikey) || (npc.type == NPCID.HornetStingy))
            {
                if (Main.rand.Next(30) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.PoisonSigil>());
				}
				
				if (Main.rand.Next(20) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Nature.PoisonVial>());
				}
			}
			
			if (npc.type == 26 || npc.type == 27 || npc.type == 28 || npc.type == 29 || npc.type == 111) // Goblins
			{
				if (Main.rand.Next(50) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>());
				}
				if (Main.rand.Next(50) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.GoblinArmyCard>());
				}
			}
			
			if (npc.type == 490 || npc.type == 489) // Drippler / Blood Zombie
			{
				if (Main.rand.Next(40) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.BloodMoonFlask>());
				}
			}
			
			if (npc.type == 285 || npc.type == 286) // Diabolists
			{
				if (Main.rand.Next(20) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.DiabolistRune>());
				}
			}
			
			if (npc.type == 204) // Spiked Jungle Slime
			{
				if (Main.rand.Next(25) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.JungleSlimeCard>());
				}
			}
			
			if (npc.type == 59) // Lava Slime
			{
				if (Main.rand.Next(25) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.LavaSlimeCard>());
				}
			}
			
			if (npc.type == 1 || npc.type == -3 || npc.type == -8 || npc.type == -9 || npc.type == -6 || npc.type == 147 || npc.type == -10) // Most Surface Slimes
			{
				if (Main.rand.Next(!OrchidWorld.foundSlimeCard ? 5 : 1000) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.SlimeCard>());
                    OrchidWorld.foundSlimeCard = true;
				}
			}
			
			if (npc.type == 87) // Wyvern Head
			{
				if (Main.rand.Next(15) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.WyvernTailFeather>());
				}
			}
			
			if (npc.type == 167) // Undead Viking
			{
                if (Main.rand.Next(30) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FrostburnSigil>());
				}
			}
			
			if (npc.type == 62) // Demon
			{
				if (Main.rand.Next(30) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FurnaceSigil>());
				}
			}
			
			if ((npc.type == NPCID.DarkCaster))
			{
				if (Main.rand.Next(50) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Blum>());
				}
				
				if (Main.rand.Next(33) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.DungeonCard>());
				}
			}
			
			if ((npc.type == NPCID.FireImp))
			{
				if (Main.rand.Next(20) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.MeltedRing>());
				}
			}
			
			if ((npc.type == NPCID.Mimic))
            {
                if (Main.rand.Next(10) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.HeavyBracer>());
				}
			}
			
			if (npc.type == 395 || npc.type == 392) // MARTIAN SAUCER
            {
                if (Main.rand.Next(4) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.MartianBeamer>());
				}
			}
			
			if ((npc.type == NPCID.IceQueen)) {
                if (Main.rand.Next(10) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.IceFlakeCone>());
				}
			}
			
			if ((npc.type == NPCID.RuneWizard)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.RuneScepter>());
			}
			
			if ((npc.type == NPCID.PirateShip)) {
                if (Main.rand.Next(5) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.PiratesGlory>());
				}
                if (Main.rand.Next(10) == 0) {
					 if (Main.rand.Next(20) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Decks.DeckDog>());
					 } else {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Decks.DeckPirate>());
					 }
				}
            }
			
			if ((npc.type == NPCID.GoblinSummoner)) {
                if (Main.rand.Next(3) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.GoblinStick>());
				}
			}
			
			if ((npc.type == NPCID.Lihzahrd || npc.type == 199)) // Lihzahrds
            {
                if (Main.rand.Next(4) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Misc.LihzahrdSilk>());
				}
				if (Main.rand.Next(100) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.SunPriestTorch>());
				}
				if (Main.rand.Next(300) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.SunPriestBelt>());
				}
			}
			
			if (npc.type == NPCID.MourningWood) {
                if (Main.rand.Next(10) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.MourningTorch>());
				}
			}
			
			if (npc.type == 346) // SANTANK
			{
                if (Main.rand.Next(10) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FragilePresent>());
				}
			}
			
			if (npc.type == NPCID.UndeadMiner) {
                if (Main.rand.Next(5) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.TreasuredBaubles>());
				}
			}
			
			if ((npc.type == NPCID.BlackRecluse || npc.type == NPCID.BlackRecluseWall)) // Black Recluse (ground/wall)
            {
                if (Main.rand.Next(40) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.VenomSigil>());
				}
			}
			
			if (npc.type == NPCID.Mimic && npc.frame.Y >= 828 && npc.frame.Y <= 1104) //Ice Mimic
			{
                if (Main.rand.Next(3) == 0) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.IceMimicScepter>());
				}
			}
			
			if (NPC.downedBoss1 == true) {
				if ((npc.type == NPCID.Harpy)) {
                    if (Main.rand.Next(6) == 0) {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Misc.HarpyTalon>());
				    }
				}
            }
			
			if ((npc.type == 347)) // Elf Copter
			{
				if (Main.rand.Next(50) == 0) {
                       Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<General.Items.Misc.RCRemote>());
				}
			}
			
			if ((npc.type == NPCID.CultistBoss)) {
                int rand;
                if (Main.expertMode)
                    rand = Main.rand.Next(73) + 18;
				else 
					rand = Main.rand.Next(49) + 12;
                Item.NewItem((int) npc.position.X + Main.rand.Next(npc.width), (int) npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Shaman.Misc.AbyssFragment>(), rand, false, 0, false, false);
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
			}*/
			
			// BOSSES
			if ((npc.type == NPCID.QueenBee))
            {
				if (!Main.expertMode) {
					if (Main.rand.Next(2) == 0) {
						if (Main.rand.Next(2) == 0) {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.QueenBeeCard>());
						} else {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Dice.HoneyDie>());
						}
					}
                    if (Main.rand.Next(2) == 0) {
						int rand = Main.rand.Next(3);
						if (rand == 0) {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.BeeSeeker>());
						} else if (rand == 1) {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.WaxyVial>());
						} else {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Air.QueenBeeFlask>());
						}
				    }
				}
				if (alchemistHit) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}
			
			if ((npc.type == NPCID.MoonLordCore))  {
				if (!Main.expertMode) {
                    if (Main.rand.Next(5) == 0) {
						if (Main.rand.Next(2) == 0) {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.Nirvana>());
						}
						else {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.TheCore>());
						}
				    }
				}
			}
			
			if ((npc.type == NPCID.WallofFlesh)) {
				if (!Main.expertMode) {
                    if (Main.rand.Next(4) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.ShamanEmblem>());
				    }
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<General.Items.Misc.OrchidEmblem>());
				}
			}
			
			if (npc.type == 50) // King Slime
			{
				if (!Main.expertMode) {
                    if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Weapons.Water.KingSlimeFlask>());
				    }
					if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.KingSlimeCard>());
					}
				}
				if (alchemistHit) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}
			
			if ((npc.type == NPCID.Plantera))
            {
				if (!Main.expertMode) {
                    if (Main.rand.Next(3) == 0) {
						if (Main.rand.Next(2) == 0) {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.BulbScepter>());
						}
						else {
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Accessories.FloralStinger>());
						}
				    }
				}
			}
			
			if ((npc.type == NPCID.Golem))
            {
				if (!Main.expertMode) {
                    if (Main.rand.Next(7) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Hardmode.SunRay>());
				    }
				}
			}
			
			if ((npc.type == 4))  // Eye of Chtulhu
            {
				if (!Main.expertMode) {
					if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.EyeCard>());
					}
				}
				if (alchemistHit) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
				}
			}
			
			if ((npc.type == 35))  // Skeletron
            {
				if (!Main.expertMode) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.SkeletronCard>());
				}
				if (alchemistHit) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier3>());
				}
			}
			
			if ((npc.type == 266))  // Brain of Chtulhu
            {
				if (!Main.expertMode) {
                    if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Accessories.PreservedCrimson>());
				    }
					if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.BrainCard>());
					}
				}
				if (alchemistHit) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}
			
			if (Array.IndexOf(new int[]{ NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1 && npc.boss) {
				if (!Main.expertMode) {
					if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Accessories.PreservedCorruption>());
					}
					if (Main.rand.Next(3) == 0) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Gambler.Weapons.Cards.EaterCard>());
					}
				}
				if (alchemistHit) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
				}
			}
			
			//THORIUM
			
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				if ((npc.type == thoriumMod.NPCType("TheGrandThunderBirdv2"))) {
					if (!Main.expertMode) {
						if (Main.rand.Next(4) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.ThunderScepter>());
						}
					}
					if (alchemistHit) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier1>());
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("QueenJelly")))
				{
					if (!Main.expertMode) {
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.QueenJellyfishScepter>());
						}
					}
					if (alchemistHit) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("GranityEnergyStorm")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.GraniteEnergyScepter>());
						}
					}
					if (alchemistHit) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("Viscount")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(7) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.ViscountScepter>());
						}
						Item.NewItem((int) npc.position.X + Main.rand.Next(npc.width), (int) npc.position.Y + Main.rand.Next(npc.height), 2, 2, ItemType<Shaman.Misc.Thorium.ViscountMaterial>(), 30, false, 0, false, false);
					}
					if (alchemistHit) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("ThePrimeScouter")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(6) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.StarScouterScepter>());
						}
					}
					if (alchemistHit) {
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Alchemist.Misc.Scrolls.ScrollTier2>());
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("FallenDeathBeholder")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.CoznixScepter>());
						}
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("BoreanStriderPopped")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(5) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>());
						}
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("Lich")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(7) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.LichScepter>());
						}
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("Abyssion")) || (npc.type == thoriumMod.NPCType("AbyssionCracked")) || (npc.type == thoriumMod.NPCType("AbyssionReleased")))
				{
					if (!Main.expertMode)
					{
						if (Main.rand.Next(6) == 0)
						{
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>());
						}
					}
				}
				
				if ((npc.type == thoriumMod.NPCType("PatchWerk")))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Shaman.Weapons.Thorium.PatchWerkScepter>());
				}
			} else {
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
			}
        }
    
		public override void SetupShop(int type, Chest shop, ref int nextSlot) {
			if (type == NPCID.WitchDoctor) {
				shop.item[nextSlot].SetDefaults(ItemType<Shaman.Weapons.ShamanRod>());
				nextSlot++;
				if (Main.hardMode) {
					shop.item[nextSlot].SetDefaults(ItemType<Shaman.Misc.RitualScepter>());
					nextSlot++;
				}
			}
			
			if (type == NPCID.Demolitionist) {
				shop.item[nextSlot].SetDefaults(ItemType<Alchemist.Weapons.Fire.GunpowderFlask>());
				nextSlot++;
			}
			
			if (type == NPCID.Dryad) {
				shop.item[nextSlot].SetDefaults(ItemType<Shaman.Accessories.DryadsGift>());
				nextSlot++;
			}
			
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				if (type == thoriumMod.NPCType("ConfusedZombie")) {
					shop.item[nextSlot].SetDefaults(ItemType<Shaman.Weapons.Thorium.PatchWerkScepter>());
					nextSlot++;
				}
			}
		}
		
		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit) {
			if (this.shamanWater) {
				damage = (double)(damage * 1.05f);
			}
			
			if (this.shamanShroom > 0) {
				damage = (double)(damage * 1.1f);
			}
			
			return true;
		}
		
		public override void DrawEffects(NPC npc, ref Color drawColor) {
			if (this.shamanBomb > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 6, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4)) {
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				Lighting.AddLight(npc.position, 0.5f, 0.2f, 0f);
				
				if (this.shamanBomb == 1) {
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45);
					npc.StrikeNPCNoInteraction(500, 0f, 0);
					for (int i = 0 ; i < 15 ; i ++) {
						int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 6, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 5f;
						Main.dust[dust].scale *= 1.5f;
					}
				}
			}
			
			if (this.shamanShroom > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 176, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1f;
					Main.dust[dust].scale *= 0.5f;
				}
			}
		}
		
		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (this.shamanWind) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 16;
				if (damage < 20) {
					damage = 20;
				}
			}
		}
		
		public override void ResetEffects(NPC npc) {
			this.shamanBomb -= this.shamanBomb > 0 ? 1 : 0;
			this.shamanShroom -= this.shamanShroom > 0 ? 1 : 0;
			this.shamanWind = false;
			this.shamanSpearDamage -= this.shamanSpearDamage > 0 ? 1 : 0;
		}
	}
}  