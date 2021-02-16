using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using OrchidMod.Shaman.UI;
using OrchidMod.Alchemist.UI;
using OrchidMod.Gambler.UI;
using OrchidMod;
using OrchidMod.Shaman;
using OrchidMod.Alchemist;


namespace OrchidMod
{
	public class OrchidMod : Mod
	{
		public static List<AlchemistHiddenReactionRecipe> alchemistReactionRecipes;
		public static ModHotKey AlchemistReactionHotKey;
		public static ModHotKey AlchemistCatalystHotKey;
		public static ModHotKey ShamanBondHotKey;
		internal static OrchidMod Instance;
		
		internal UserInterface orchidModShamanInterface;
		internal UserInterface orchidModShamanCharacterInterface;
		internal UserInterface orchidModAlchemistInterface;
		internal UserInterface orchidModAlchemistSelectInterface;
		internal UserInterface orchidModAlchemistBookInterface;
		internal UserInterface orchidModGamblerInterface;
		internal ShamanUIState shamanUIState;
		internal ShamanCharacterUIState shamanCharacterUIState;
		internal AlchemistUIState alchemistUIState;
		internal AlchemistSelectUIState alchemistSelectUIState;
		internal AlchemistBookUIState alchemistBookUIState;
		internal GamblerUIState gamblerUIState;
		public static bool reloadShamanUI;
			
		public OrchidMod()
		{
			Instance = this;
		}
		
		public override void PostSetupContent() {
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			Mod censusMod = ModLoader.GetMod("Census");
			if(censusMod != null)
			{
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.Town.Croupier>(), $"Have a gamber card ([i:{ModContent.ItemType<Gambler.Weapons.Cards.SlimeCard>()}][i:{ModContent.ItemType<Gambler.Weapons.Cards.EmbersCard>()}] etc.) in your deck");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.Town.Chemist>(), "Find in the main mineshaft, in the center of your world");
			}
			
			if (bossChecklist != null) {
				
				// Bosses -- Vanilla
				
				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"QueenBee",
					new List<int> {ModContent.ItemType<Gambler.Weapons.Cards.QueenBeeCard>(), ModContent.ItemType<Gambler.Weapons.Dice.HoneyDie>(), ModContent.ItemType<Shaman.Weapons.BeeSeeker>(), ModContent.ItemType<Shaman.Accessories.WaxyVial>(), ModContent.ItemType<Alchemist.Weapons.Air.QueenBeeFlask>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"MoonLord",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.Nirvana>(), ModContent.ItemType<Shaman.Weapons.Hardmode.TheCore>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"WallofFlesh",
					new List<int> {ModContent.ItemType<Shaman.Accessories.ShamanEmblem>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Plantera",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.BulbScepter>(), ModContent.ItemType<Shaman.Accessories.FloralStinger>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Golem",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.SunRay>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"KingSlime",
					new List<int> {ModContent.ItemType<Alchemist.Weapons.Water.KingSlimeFlask>(), ModContent.ItemType<Gambler.Weapons.Cards.KingSlimeCard>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"EaterofWorldsHead",
					new List<int> {ModContent.ItemType<Alchemist.Accessories.PreservedCorruption>(), ModContent.ItemType<Gambler.Weapons.Cards.EaterCard>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"BrainofCthulhu",
					new List<int> {ModContent.ItemType<Alchemist.Accessories.PreservedCrimson>(), ModContent.ItemType<Gambler.Weapons.Cards.BrainCard>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"EyeofCthulhu",
					new List<int> {ModContent.ItemType<Gambler.Weapons.Cards.EyeCard>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"SkeletronHead",
					new List<int> {ModContent.ItemType<Gambler.Weapons.Cards.SkeletronCard>()});
					
					// Minibosses and Events -- Vanilla

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Goblin Army",
					new List<int> {ModContent.ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Blood Moon",
					new List<int> {ModContent.ItemType<Alchemist.Weapons.Water.BloodMoonFlask>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Pirate Invasion",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.PiratesGlory>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"PirateShip",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.PiratesGlory>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Frost Moon",
					new List<int> {ModContent.ItemType<Shaman.Accessories.FragilePresent>(), ModContent.ItemType<Shaman.Weapons.Hardmode.IceFlakeCone>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"SantaNK1",
					new List<int> {ModContent.ItemType<Shaman.Accessories.FragilePresent>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"IceQueen",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.IceFlakeCone>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Pumpkin Moon",
					new List<int> {ModContent.ItemType<Shaman.Accessories.MourningTorch>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"MourningWood",
					new List<int> {ModContent.ItemType<Shaman.Accessories.MourningTorch>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Martian Madness",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.MartianBeamer>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"MartianSaucer",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Hardmode.MartianBeamer>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"CultistBoss",
					new List<int> {ModContent.ItemType<Shaman.Misc.AbyssFragment>()});
					
					// Bosses -- Thorium

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"The Grand Thunder Bird",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.ThunderScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"The Queen Jellyfish",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.QueenJellyfishScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Granite Energy Storm",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.GraniteEnergyScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Viscount",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.ViscountScepter>(), ModContent.ItemType<Shaman.Misc.Thorium.ViscountMaterial>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Star Scouter",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.StarScouterScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Coznix, the Fallen Beholder",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.CoznixScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Borean Strider",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"The Lich",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.LichScepter>()});

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Abyssion, the Forgotten One",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>()});
					
					// Minibosses and Events -- Thorium

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Patch Werk",
					new List<int> {ModContent.ItemType<Shaman.Weapons.Thorium.PatchWerkScepter>()});
			}
		}
		
		public override void Load()
		{
			AlchemistReactionHotKey = RegisterHotKey("Alchemist Hidden Reaction", "Mouse3");
			AlchemistCatalystHotKey = RegisterHotKey("Alchemist Catalyst Tool Shortcut", "Z");
			ShamanBondHotKey = RegisterHotKey("Shaman Bond Abilities", "Mouse3");
			alchemistReactionRecipes = AlchemistHiddenReactionHelper.ListReactions();
			
			if (!Main.dedServ)
			{
				shamanUIState = new ShamanUIState();
				shamanCharacterUIState = new ShamanCharacterUIState();
				alchemistUIState = new AlchemistUIState();
				alchemistSelectUIState = new AlchemistSelectUIState();
				alchemistBookUIState = new AlchemistBookUIState();
				gamblerUIState = new GamblerUIState();
				
				orchidModShamanInterface = new UserInterface();
				orchidModShamanCharacterInterface = new UserInterface();
				orchidModAlchemistInterface = new UserInterface();
				orchidModAlchemistSelectInterface = new UserInterface();
				orchidModAlchemistBookInterface = new UserInterface();
				orchidModGamblerInterface = new UserInterface();
				
				shamanUIState.Activate();
				orchidModShamanInterface.SetState(shamanUIState);
				
				shamanCharacterUIState.Activate();
				orchidModShamanCharacterInterface.SetState(shamanCharacterUIState);
				
				alchemistUIState.Activate();
				orchidModAlchemistInterface.SetState(alchemistUIState);
				
				alchemistSelectUIState.Activate();
				orchidModAlchemistSelectInterface.SetState(alchemistSelectUIState);
				
				alchemistBookUIState.Activate();
				orchidModAlchemistBookInterface.SetState(alchemistBookUIState);
				
				gamblerUIState.Activate();
				orchidModGamblerInterface.SetState(gamblerUIState);
			}
		}
		
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"OrchidMod: UI",
					delegate {
						orchidModShamanInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModShamanCharacterInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistSelectInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistBookInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModGamblerInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			if (reloadShamanUI) {
				shamanUIState = new ShamanUIState();
				shamanUIState.Activate();
				orchidModShamanInterface.SetState(shamanUIState);
				
				shamanCharacterUIState = new ShamanCharacterUIState();
				shamanCharacterUIState.Activate();
				orchidModShamanCharacterInterface.SetState(shamanCharacterUIState);
				
				alchemistUIState = new AlchemistUIState();
				alchemistUIState.Activate();
				orchidModAlchemistInterface.SetState(alchemistUIState);
				
				alchemistSelectUIState = new AlchemistSelectUIState();
				alchemistSelectUIState.Activate();
				orchidModAlchemistSelectInterface.SetState(alchemistSelectUIState);
				
				alchemistBookUIState = new AlchemistBookUIState();
				alchemistBookUIState.Activate();
				orchidModAlchemistBookInterface.SetState(alchemistBookUIState);
				
				gamblerUIState = new GamblerUIState();
				gamblerUIState.Activate();
				orchidModGamblerInterface.SetState(gamblerUIState);
				
				reloadShamanUI = false;
			}
		}
		
		public override void Unload() {
			if (!Main.dedServ)
            {
				AlchemistUIFrame.ressourceBottom = null;
				AlchemistUIFrame.ressourceTop = null;
				AlchemistUIFrame.ressourceFull = null;
				AlchemistUIFrame.ressourceFullTop = null;
				AlchemistUIFrame.ressourceFullBorder = null;
				AlchemistUIFrame.ressourceEmpty = null;
				AlchemistUIFrame.reactionCooldown = null;
				AlchemistUIFrame.reactionCooldownLiquid = null;
				AlchemistUIFrame.symbolWater = null;
				AlchemistUIFrame.symbolFire = null;
				AlchemistUIFrame.symbolNature = null;
				AlchemistUIFrame.symbolAir = null;
				AlchemistUIFrame.symbolLight = null;
				AlchemistUIFrame.symbolDark = null;
				
				AlchemistSelectUIFrame.resourceItem = null;
				AlchemistSelectUIFrame.resourceBack = null;
				AlchemistSelectUIFrame.resourceCross = null;
				AlchemistSelectUIFrame.resourceSelected = null;
				AlchemistSelectUIFrame.resourceBorder = null;
				
				AlchemistBookUIFrame.ressourceBookPage = null;
				AlchemistBookUIFrame.ressourceBookSlot = null;
				
				ShamanUIFrame.shamanUIMainFrame = null;
				ShamanUIFrame.shamanUILevel = null;
				ShamanUIFrame.resourceDuration = null;
				ShamanUIFrame.resourceDurationEnd = null;
				ShamanUIFrame.resourceFire = null;
				ShamanUIFrame.resourceFireEnd = null;
				ShamanUIFrame.resourceWater = null;
				ShamanUIFrame.resourceWaterEnd = null;
				ShamanUIFrame.resourceAir = null;
				ShamanUIFrame.resourceAirEnd = null;
				ShamanUIFrame.resourceEarth = null;
				ShamanUIFrame.resourceEarthEnd = null;
				ShamanUIFrame.resourceSpirit = null;
				ShamanUIFrame.resourceSpiritEnd = null;
				ShamanUIFrame.FireSymbolBasic = null;
				ShamanUIFrame.WaterSymbolBasic = null;
				ShamanUIFrame.AirSymbolBasic = null;
				ShamanUIFrame.EarthSymbolBasic = null;
				ShamanUIFrame.SpiritSymbolBasic = null;
				ShamanUIFrame.Bonus1Symbol = null;
				ShamanUIFrame.Bonus2Symbol = null;
				ShamanUIFrame.Bonus3Symbol = null;
				ShamanUIFrame.Bonus4Symbol = null;
				ShamanUIFrame.Bonus5Symbol = null;
				ShamanUIFrame.Bonus6Symbol = null;
				ShamanUIFrame.Bonus7Symbol = null;
				ShamanUIFrame.Bonus8Symbol = null;
				ShamanUIFrame.Bonus9Symbol = null;
				ShamanUIFrame.Bonus10Symbol = null;
				ShamanUIFrame.Bonus11Symbol = null;
				ShamanUIFrame.Bonus12Symbol = null;
				ShamanUIFrame.Bonus13Symbol = null;
				ShamanUIFrame.Bonus14Symbol = null;
				ShamanUIFrame.Bonus15Symbol = null;
				ShamanUIFrame.Level1Symbol = null;
				ShamanUIFrame.Level2Symbol = null;
				ShamanUIFrame.Level3Symbol = null;
				ShamanUIFrame.Level4Symbol = null;
				ShamanUIFrame.Level5Symbol = null;
				ShamanUIFrame.SymbolFire = null;
				ShamanUIFrame.SymbolIce = null;
				ShamanUIFrame.SymbolPoison = null;
				ShamanUIFrame.SymbolVenom = null;
				ShamanUIFrame.SymbolDemonite = null;
				ShamanUIFrame.SymbolHeavy = null;
				ShamanUIFrame.SymbolForest = null;
				ShamanUIFrame.SymbolDiabolist = null;
				ShamanUIFrame.SymbolSkull = null;
				ShamanUIFrame.SymbolWaterHoney = null;
				ShamanUIFrame.SymbolDestroyer = null;
				ShamanUIFrame.SymbolBee = null;
				ShamanUIFrame.SymbolAmber = null;
				ShamanUIFrame.SymbolSmite = null;
				ShamanUIFrame.SymbolCrimtane = null;
				ShamanUIFrame.SymbolRage = null;
				ShamanUIFrame.SymbolLava = null;
				ShamanUIFrame.SymbolFeather = null;
				ShamanUIFrame.SymbolAnklet = null;
				ShamanUIFrame.SymbolWyvern = null;
				
				ShamanCharacterUIFrame.symbolAttack = null;
				ShamanCharacterUIFrame.symbolDefense = null;
				ShamanCharacterUIFrame.symbolCritical = null;
				ShamanCharacterUIFrame.symbolRegeneration = null;
				ShamanCharacterUIFrame.symbolSpeed = null;
				ShamanCharacterUIFrame.fireLoaded = null;
				ShamanCharacterUIFrame.waterLoaded = null;
				ShamanCharacterUIFrame.airLoaded = null;
				ShamanCharacterUIFrame.earthLoaded = null;
				ShamanCharacterUIFrame.spiritLoaded = null;
				
				GamblerUIFrame.ressourceBar = null;
				GamblerUIFrame.ressourceBarFull = null;
				GamblerUIFrame.ressourceBarTop = null;
				GamblerUIFrame.ressourceBarDiceFull = null;
				GamblerUIFrame.ressourceBarDiceTop = null;
				GamblerUIFrame.chip1 = null;
				GamblerUIFrame.chip2 = null;
				GamblerUIFrame.chip3 = null;
				GamblerUIFrame.chip4 = null;
				GamblerUIFrame.chip5 = null;
				GamblerUIFrame.dice1 = null;
				GamblerUIFrame.dice2 = null;
				GamblerUIFrame.dice3 = null;
				GamblerUIFrame.dice4 = null;
				GamblerUIFrame.dice5 = null;
				GamblerUIFrame.dice6 = null;
				GamblerUIFrame.UIDeck = null;
				GamblerUIFrame.UICard = null;
				GamblerUIFrame.UICardNext1 = null;
				GamblerUIFrame.UICardNext2 = null;
				GamblerUIFrame.UICardNext3 = null;
				GamblerUIFrame.UIRedraw = null;
				GamblerUIFrame.UIDeckbuilding = null;
				GamblerUIFrame.UIDeckbuildingBlock = null;
			}
			
			orchidModShamanInterface = null;
			orchidModShamanCharacterInterface = null;
			orchidModAlchemistInterface = null;
			orchidModAlchemistSelectInterface = null;
			orchidModAlchemistBookInterface = null;
			orchidModGamblerInterface = null;
			shamanUIState = null;
			shamanCharacterUIState = null;
			alchemistUIState = null;
			alchemistSelectUIState = null;
			alchemistBookUIState = null;
			gamblerUIState = null;
			Instance = null;
			AlchemistReactionHotKey = null;
			AlchemistCatalystHotKey = null;
			ShamanBondHotKey = null;
			alchemistReactionRecipes = null;
		}
		
		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			OrchidModMessageType msgType = (OrchidModMessageType)reader.ReadByte();
			switch (msgType) {
				case OrchidModMessageType.ORCHIDPLAYERSYNCPLAYER:
					byte playernumber = reader.ReadByte();
					OrchidModPlayer modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					
					// Orbs
					byte readSmallOrb = reader.ReadByte();
					modPlayer.shamanOrbSmall = (ShamanOrbSmall)readSmallOrb;

					byte readBigOrb = reader.ReadByte();
					modPlayer.shamanOrbBig = (ShamanOrbBig)readBigOrb;
					
					byte readLargeOrb = reader.ReadByte();
					modPlayer.shamanOrbLarge = (ShamanOrbLarge)readLargeOrb;
					
					byte readUniqueOrb = reader.ReadByte();
					modPlayer.shamanOrbUnique = (ShamanOrbUnique)readUniqueOrb;
					
					byte readCircleOrb = reader.ReadByte();
					modPlayer.shamanOrbCircle = (ShamanOrbCircle)readCircleOrb;
					
					//Counts
					int countSmall = reader.ReadInt32();
					modPlayer.orbCountSmall = countSmall;
					
					int countBig = reader.ReadInt32();
					modPlayer.orbCountBig = countBig;
					
					int countLarge = reader.ReadInt32();
					modPlayer.orbCountLarge = countLarge;
					
					int countUnique = reader.ReadInt32();
					modPlayer.orbCountUnique = countUnique;
					
					int countCircle = reader.ReadInt32();
					modPlayer.orbCountCircle = countCircle;
					
					// Buff Levels
					int attackBuff = reader.ReadInt32();
					modPlayer.shamanFireBuff = attackBuff;
					
					int armorBuff = reader.ReadInt32();
					modPlayer.shamanWaterBuff = armorBuff;
					
					int criticalBuff = reader.ReadInt32();
					modPlayer.shamanAirBuff = criticalBuff;
					
					int regenerationBuff = reader.ReadInt32();
					modPlayer.shamanEarthBuff = regenerationBuff;
					
					int speedBuff = reader.ReadInt32();
					modPlayer.shamanSpiritBuff = speedBuff;
					
					// Buff Timers
					int attackTimer = reader.ReadInt32();
					modPlayer.shamanFireTimer = attackTimer;
					
					int armorTimer = reader.ReadInt32();
					modPlayer.shamanWaterTimer = armorTimer;
					
					int criticalTimer = reader.ReadInt32();
					modPlayer.shamanAirTimer = criticalTimer;
					
					int regenerationTimer = reader.ReadInt32();
					modPlayer.shamanEarthTimer = regenerationTimer;
					
					int speedTimer = reader.ReadInt32();
					modPlayer.shamanSpiritTimer = speedTimer;
					
					//Gambler Card in Deck
					bool cardInDeck = reader.ReadBoolean();
					modPlayer.gamblerHasCardInDeck = cardInDeck;
					
					
					// Alchemist Misc
					// bool alchElement0 = reader.ReadBoolean();
					// modPlayer.alchemistElements[0] = alchElement0;
					
					// bool alchElement1 = reader.ReadBoolean();
					// modPlayer.alchemistElements[1] = alchElement1;
					
					// bool alchElement2 = reader.ReadBoolean();
					// modPlayer.alchemistElements[2] = alchElement2;
					
					// bool alchElement3 = reader.ReadBoolean();
					// modPlayer.alchemistElements[3] = alchElement3;
					
					// bool alchElement4 = reader.ReadBoolean();
					// modPlayer.alchemistElements[4] = alchElement4;
					
					// bool alchElement5 = reader.ReadBoolean();
					// modPlayer.alchemistElements[5] = alchElement5;
					
					// int alchFlask0 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[0] = alchFlask0;
					
					// int alchFlask1 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[1] = alchFlask1;
					
					// int alchFlask2 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[2] = alchFlask2;
					
					// int alchFlask3 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[3] = alchFlask3;
					
					// int alchFlask4 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[4] = alchFlask4;
					
					// int alchFlask5 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[5] = alchFlask5;
					
					// int alchDust0 = reader.ReadInt32();
					// modPlayer.alchemistDusts[0] = alchDust0;
					
					// int alchDust1 = reader.ReadInt32();
					// modPlayer.alchemistDusts[1] = alchDust1;
					
					// int alchDust2 = reader.ReadInt32();
					// modPlayer.alchemistDusts[2] = alchDust2;
					
					// int alchDust3 = reader.ReadInt32();
					// modPlayer.alchemistDusts[3] = alchDust3;
					
					// int alchDust4 = reader.ReadInt32();
					// modPlayer.alchemistDusts[4] = alchDust4;
					
					// int alchDust5 = reader.ReadInt32();
					// modPlayer.alchemistDusts[5] = alchDust5;
					
					// int alchColorR = reader.ReadInt32();
					// modPlayer.alchemistColorR = alchColorR;
					
					// int alchColorG = reader.ReadInt32();
					// modPlayer.alchemistColorG = alchColorG;
					
					// int alchColorB = reader.ReadInt32();
					// modPlayer.alchemistColorB = alchColorB;
					break;
				case OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					readSmallOrb = reader.ReadByte();
					modPlayer.shamanOrbSmall = (ShamanOrbSmall)readSmallOrb;

					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbSmall);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBTYPECHANGEDBIG:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					readBigOrb = reader.ReadByte();
					modPlayer.shamanOrbBig = (ShamanOrbBig)readBigOrb;

					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDBIG);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbBig);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					readLargeOrb = reader.ReadByte();
					modPlayer.shamanOrbLarge = (ShamanOrbLarge)readLargeOrb;

					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbLarge);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					readUniqueOrb = reader.ReadByte();
					modPlayer.shamanOrbUnique = (ShamanOrbUnique)readUniqueOrb;

					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbUnique);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					readCircleOrb = reader.ReadByte();
					modPlayer.shamanOrbCircle = (ShamanOrbCircle)readCircleOrb;

					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbCircle);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					countSmall = reader.ReadInt32();
					modPlayer.orbCountSmall = countSmall;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountSmall);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					countBig = reader.ReadInt32();
					modPlayer.orbCountBig = countBig;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountBig);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					countLarge = reader.ReadInt32();
					modPlayer.orbCountLarge = countLarge;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountLarge);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					countUnique = reader.ReadInt32();
					modPlayer.orbCountUnique = countUnique;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountUnique);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					countCircle = reader.ReadInt32();
					modPlayer.orbCountCircle = countCircle;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountCircle);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFCHANGEDATTACK:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					attackBuff = reader.ReadInt32();
					modPlayer.shamanFireBuff = attackBuff;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDATTACK);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanFireBuff);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFCHANGEDARMOR:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					armorBuff = reader.ReadInt32();
					modPlayer.shamanWaterBuff = armorBuff;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDARMOR);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanWaterBuff);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFCHANGEDCRITICAL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					criticalBuff = reader.ReadInt32();
					modPlayer.shamanAirBuff = criticalBuff;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDCRITICAL);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanAirBuff);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFCHANGEDREGENERATION:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					regenerationBuff = reader.ReadInt32();
					modPlayer.shamanEarthBuff = regenerationBuff;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDCRITICAL);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanEarthBuff);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFCHANGEDSPEED:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					speedBuff = reader.ReadInt32();
					modPlayer.shamanSpiritBuff = speedBuff;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFCHANGEDSPEED);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanSpiritBuff);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					attackTimer = reader.ReadInt32();
					modPlayer.shamanFireTimer = attackTimer;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanFireTimer);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					armorTimer = reader.ReadInt32();
					modPlayer.shamanWaterTimer = armorTimer;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanWaterTimer);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					criticalTimer = reader.ReadInt32();
					modPlayer.shamanAirTimer = criticalTimer;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanAirTimer);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					regenerationTimer = reader.ReadInt32();
					modPlayer.shamanEarthTimer = regenerationTimer;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanEarthTimer);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					speedTimer = reader.ReadInt32();
					modPlayer.shamanSpiritTimer = speedTimer;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanSpiritTimer);
						packet.Send(-1, playernumber);
					}
					break;
					
				case OrchidModMessageType.GAMBLERCARDINDECKCHANGED:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					cardInDeck = reader.ReadBoolean();
					modPlayer.gamblerHasCardInDeck = cardInDeck;
					if (Main.netMode == NetmodeID.Server) {
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
						packet.Write(playernumber);
						packet.Write(modPlayer.gamblerHasCardInDeck);
						packet.Send(-1, playernumber);
					}
					break;
					
				// case OrchidModMessageType.ALCHEMISTELEMENTCHANGED0:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchElement0 = reader.ReadBoolean();
					// modPlayer.alchemistElements[0] = alchElement0;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED0);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistElements[0]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTELEMENTCHANGED1:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchElement1 = reader.ReadBoolean();
					// modPlayer.alchemistElements[1] = alchElement1;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED1);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistElements[1]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTELEMENTCHANGED2:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchElement2 = reader.ReadBoolean();
					// modPlayer.alchemistElements[2] = alchElement2;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED2);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistElements[2]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTELEMENTCHANGED3:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchElement3 = reader.ReadBoolean();
					// modPlayer.alchemistElements[3] = alchElement3;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED3);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistElements[3]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTELEMENTCHANGED4:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchElement4 = reader.ReadBoolean();
					// modPlayer.alchemistElements[4] = alchElement4;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED4);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistElements[4]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTELEMENTCHANGED5:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchElement5 = reader.ReadBoolean();
					// modPlayer.alchemistElements[5] = alchElement5;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTELEMENTCHANGED5);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistElements[5]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTFLASKCHANGED0:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchFlask0 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[0] = alchFlask0;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED0);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistFlasks[0]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTFLASKCHANGED1:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchFlask1 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[1] = alchFlask1;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED1);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistFlasks[1]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTFLASKCHANGED2:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchFlask2 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[2] = alchFlask2;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED2);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistFlasks[2]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTFLASKCHANGED3:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchFlask3 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[3] = alchFlask3;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED3);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistFlasks[3]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTFLASKCHANGED4:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchFlask4 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[4] = alchFlask4;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED4);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistFlasks[4]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTFLASKCHANGED5:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchFlask5 = reader.ReadInt32();
					// modPlayer.alchemistFlasks[5] = alchFlask5;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTFLASKCHANGED5);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistFlasks[5]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTDUSTCHANGED0:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchDust0 = reader.ReadInt32();
					// modPlayer.alchemistDusts[0] = alchDust0;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED0);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistDusts[0]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTDUSTCHANGED1:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchDust1 = reader.ReadInt32();
					// modPlayer.alchemistDusts[1] = alchDust1;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED1);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistDusts[1]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTDUSTCHANGED2:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchDust2 = reader.ReadInt32();
					// modPlayer.alchemistDusts[2] = alchDust2;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED2);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistDusts[2]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTDUSTCHANGED3:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchDust3 = reader.ReadInt32();
					// modPlayer.alchemistDusts[3] = alchDust3;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED3);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistDusts[3]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTDUSTCHANGED4:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchDust4 = reader.ReadInt32();
					// modPlayer.alchemistDusts[4] = alchDust4;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED4);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistDusts[4]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTDUSTCHANGED5:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchDust5 = reader.ReadInt32();
					// modPlayer.alchemistDusts[5] = alchDust5;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTDUSTCHANGED5);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistDusts[5]);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTCOLORCHANGEDR:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchColorR = reader.ReadInt32();
					// modPlayer.alchemistColorR = alchColorR;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTCOLORCHANGEDR);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistColorR);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTCOLORCHANGEDG:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchColorG = reader.ReadInt32();
					// modPlayer.alchemistColorG = alchColorG;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTCOLORCHANGEDG);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistColorG);
						// packet.Send(-1, playernumber);
					// }
					// break;
					
				// case OrchidModMessageType.ALCHEMISTCOLORCHANGEDB:
					// playernumber = reader.ReadByte();
					// modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					// alchColorB = reader.ReadInt32();
					// modPlayer.alchemistColorB = alchColorB;
					// if (Main.netMode == NetmodeID.Server) {
						// var packet = GetPacket();
						// packet.Write((byte)OrchidModMessageType.ALCHEMISTCOLORCHANGEDB);
						// packet.Write(playernumber);
						// packet.Write(modPlayer.alchemistColorB);
						// packet.Send(-1, playernumber);
					// }
				
				default:
					Logger.WarnFormat("OrchidMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
}
