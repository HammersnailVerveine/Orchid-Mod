using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist;
using OrchidMod.Alchemist.UI;
using OrchidMod.Common;
using OrchidMod.Common.Hooks;
using OrchidMod.Effects;
using OrchidMod.Gambler.UI;
using OrchidMod.Shaman;
using OrchidMod.Shaman.UI;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod
{
	public class OrchidMod : Mod
	{
		public static OrchidMod Instance { get; private set; }
		public static Mod ThoriumMod { get; private set; }

		internal CroupierUI croupierUI;

		public static Texture2D[] coatingTextures;
		public static List<AlchemistHiddenReactionRecipe> alchemistReactionRecipes;
		public static ModHotKey AlchemistReactionHotKey;
		public static ModHotKey AlchemistCatalystHotKey;

		internal UserInterface orchidModShamanInterface;
		internal UserInterface orchidModShamanCharacterInterface;
		internal UserInterface orchidModAlchemistInterface;
		internal UserInterface orchidModAlchemistSelectInterface;
		internal UserInterface orchidModAlchemistSelectKeysInterface;
		internal UserInterface orchidModAlchemistBookInterface;
		internal UserInterface orchidModGamblerInterface;
		internal ShamanUIState shamanUIState;
		internal ShamanCharacterUIState shamanCharacterUIState;
		internal AlchemistUIState alchemistUIState;
		internal AlchemistSelectUIState alchemistSelectUIState;
		internal AlchemistSelectKeysUIState alchemistSelectKeysUIState;
		internal AlchemistBookUIState alchemistBookUIState;
		internal GamblerUIState gamblerUIState;

		public static bool reloadShamanUI;

		public OrchidMod() => Instance = this;

		public override void Load()
		{
			ThoriumMod = ModLoader.GetMod("ThoriumMod");

			EffectsManager.Load(mod: this);
			HookLoader.Load(mod: this);
			PrimitiveTrailSystem.Trail.Load();

			// ...

			AlchemistReactionHotKey = RegisterHotKey("Alchemist Hidden Reaction", "Mouse3");
			AlchemistCatalystHotKey = RegisterHotKey("Alchemist Catalyst Tool Shortcut", "Z");
			alchemistReactionRecipes = AlchemistHiddenReactionHelper.ListReactions();

			if (!Main.dedServ)
			{
				croupierUI = new CroupierUI();
				shamanUIState = new ShamanUIState();
				shamanCharacterUIState = new ShamanCharacterUIState();
				alchemistUIState = new AlchemistUIState();
				alchemistSelectUIState = new AlchemistSelectUIState();
				alchemistSelectKeysUIState = new AlchemistSelectKeysUIState();
				alchemistBookUIState = new AlchemistBookUIState();
				gamblerUIState = new GamblerUIState();

				orchidModShamanInterface = new UserInterface();
				orchidModShamanCharacterInterface = new UserInterface();
				orchidModAlchemistInterface = new UserInterface();
				orchidModAlchemistSelectInterface = new UserInterface();
				orchidModAlchemistSelectKeysInterface = new UserInterface();
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

				alchemistSelectKeysUIState.Activate();
				orchidModAlchemistSelectKeysInterface.SetState(alchemistSelectKeysUIState);

				alchemistBookUIState.Activate();
				orchidModAlchemistBookInterface.SetState(alchemistBookUIState);

				gamblerUIState.Activate();
				orchidModGamblerInterface.SetState(gamblerUIState);

				coatingTextures = new Texture2D[6];
				coatingTextures[0] = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingFire");
				coatingTextures[1] = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingWater");
				coatingTextures[2] = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingNature");
				coatingTextures[3] = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingAir");
				coatingTextures[4] = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingLight");
				coatingTextures[5] = ModContent.GetTexture("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingDark");
			}
		}

		public override void Unload()
		{
			PrimitiveTrailSystem.Trail.Unload();

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

				AlchemistSelectKeysUIFrame.emptyTexture = null;

				AlchemistBookUIFrame.ressourceBookPage = null;
				AlchemistBookUIFrame.ressourceBookSlot = null;
				AlchemistBookUIFrame.ressourceBookSlotEmpty = null;
				AlchemistBookUIFrame.ressourceBookPopup = null;

				ShamanUIFrame.shamanUIMainFrame = null;
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

				ShamanUIFrame.SymbolAmethyst = null;
				ShamanUIFrame.SymbolTopaz = null;
				ShamanUIFrame.SymbolSapphire = null;
				ShamanUIFrame.SymbolEmerald = null;
				ShamanUIFrame.SymbolRuby = null;

				ShamanCharacterUIFrame.symbolAttack = null;
				ShamanCharacterUIFrame.symbolDefense = null;
				ShamanCharacterUIFrame.symbolCritical = null;
				ShamanCharacterUIFrame.symbolRegeneration = null;
				ShamanCharacterUIFrame.symbolSpeed = null;
				ShamanCharacterUIFrame.symbolWeak = null;
				ShamanCharacterUIFrame.fireLoaded = null;
				ShamanCharacterUIFrame.waterLoaded = null;
				ShamanCharacterUIFrame.airLoaded = null;
				ShamanCharacterUIFrame.earthLoaded = null;
				ShamanCharacterUIFrame.spiritLoaded = null;
				ShamanCharacterUIFrame.resource = null;
				ShamanCharacterUIFrame.resourceEnd = null;
				ShamanCharacterUIFrame.resourceBar = null;

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
				GamblerUIFrame.chipDirection = null;

				coatingTextures = null;
			}

			orchidModShamanInterface = null;
			orchidModShamanCharacterInterface = null;
			orchidModAlchemistInterface = null;
			orchidModAlchemistSelectInterface = null;
			orchidModAlchemistSelectKeysInterface = null;
			orchidModAlchemistBookInterface = null;
			orchidModGamblerInterface = null;
			shamanUIState = null;
			shamanCharacterUIState = null;
			alchemistUIState = null;
			alchemistSelectUIState = null;
			alchemistSelectKeysUIState = null;
			alchemistBookUIState = null;
			gamblerUIState = null;
			AlchemistReactionHotKey = null;
			AlchemistCatalystHotKey = null;
			alchemistReactionRecipes = null;

			// ...

			EffectsManager.Unload();
			HookLoader.Unload();

			croupierUI = null;

			ThoriumMod = null;
			Instance = null;
		}

		public override void PostSetupContent()
		{
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			Mod censusMod = ModLoader.GetMod("Census");
			if (censusMod != null)
			{
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.Town.Croupier>(), $"Have a gamber card ([i:{ModContent.ItemType<Gambler.Weapons.Cards.SlimeCard>()}][i:{ModContent.ItemType<Gambler.Weapons.Cards.EmbersCard>()}] etc.) in your deck");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.Town.Chemist>(), "Find in the main mineshaft, in the center of your world");
			}

			if (bossChecklist != null)
			{

				// Bosses -- Vanilla

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"QueenBee",
					new List<int> { ModContent.ItemType<Gambler.Weapons.Cards.QueenBeeCard>(), ModContent.ItemType<Gambler.Weapons.Dice.HoneyDie>(), ModContent.ItemType<Shaman.Weapons.BeeSeeker>(), ModContent.ItemType<Shaman.Accessories.WaxyVial>(), ModContent.ItemType<Alchemist.Weapons.Air.QueenBeeFlask>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"MoonLord",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.Nirvana>(), ModContent.ItemType<Shaman.Weapons.Hardmode.TheCore>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"WallofFlesh",
					new List<int> { ModContent.ItemType<Shaman.Accessories.ShamanEmblem>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Plantera",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.BulbScepter>(), ModContent.ItemType<Shaman.Accessories.FloralStinger>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Golem",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.SunRay>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"KingSlime",
					new List<int> { ModContent.ItemType<Alchemist.Weapons.Water.KingSlimeFlask>(), ModContent.ItemType<Gambler.Weapons.Cards.KingSlimeCard>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"EaterofWorldsHead",
					new List<int> { ModContent.ItemType<Alchemist.Accessories.PreservedCorruption>(), ModContent.ItemType<Gambler.Weapons.Cards.EaterCard>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"BrainofCthulhu",
					new List<int> { ModContent.ItemType<Alchemist.Accessories.PreservedCrimson>(), ModContent.ItemType<Gambler.Weapons.Cards.BrainCard>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"EyeofCthulhu",
					new List<int> { ModContent.ItemType<Gambler.Weapons.Cards.EyeCard>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"SkeletronHead",
					new List<int> { ModContent.ItemType<Gambler.Weapons.Cards.SkeletronCard>() });

				// Minibosses and Events -- Vanilla

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Goblin Army",
					new List<int> { ModContent.ItemType<Alchemist.Weapons.Water.GoblinArmyFlask>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Blood Moon",
					new List<int> { ModContent.ItemType<Alchemist.Weapons.Water.BloodMoonFlask>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Pirate Invasion",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.PiratesGlory>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"PirateShip",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.PiratesGlory>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Frost Moon",
					new List<int> { ModContent.ItemType<Shaman.Accessories.FragilePresent>(), ModContent.ItemType<Shaman.Weapons.Hardmode.IceFlakeCone>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"SantaNK1",
					new List<int> { ModContent.ItemType<Shaman.Accessories.FragilePresent>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"IceQueen",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.IceFlakeCone>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Pumpkin Moon",
					new List<int> { ModContent.ItemType<Shaman.Accessories.MourningTorch>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"MourningWood",
					new List<int> { ModContent.ItemType<Shaman.Accessories.MourningTorch>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"Martian Madness",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.MartianBeamer>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"MartianSaucer",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Hardmode.MartianBeamer>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"Terraria",
					"CultistBoss",
					new List<int> { ModContent.ItemType<Shaman.Misc.AbyssFragment>() });

				// Bosses -- Thorium

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"The Grand Thunder Bird",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.ThunderScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"The Queen Jellyfish",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.QueenJellyfishScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Granite Energy Storm",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.GraniteEnergyScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Viscount",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.ViscountScepter>(), ModContent.ItemType<Shaman.Misc.Thorium.ViscountMaterial>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Star Scouter",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.StarScouterScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Coznix, the Fallen Beholder",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.CoznixScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Borean Strider",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"The Lich",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.LichScepter>() });

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Abyssion, the Forgotten One",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>() });

				// Minibosses and Events -- Thorium

				bossChecklist.Call(
					"AddToBossLoot",
					"ThoriumMod",
					"Patch Werk",
					new List<int> { ModContent.ItemType<Shaman.Weapons.Thorium.PatchWerkScepter>() });
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"OrchidMod: UI",
					delegate
					{
						orchidModShamanInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModShamanCharacterInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistSelectInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistSelectKeysInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModAlchemistBookInterface.Draw(Main.spriteBatch, new GameTime());
						orchidModGamblerInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			if (reloadShamanUI)
			{
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

				alchemistSelectKeysUIState = new AlchemistSelectKeysUIState();
				alchemistSelectKeysUIState.Activate();
				orchidModAlchemistSelectKeysInterface.SetState(alchemistSelectKeysUIState);

				alchemistBookUIState = new AlchemistBookUIState();
				alchemistBookUIState.Activate();
				orchidModAlchemistBookInterface.SetState(alchemistBookUIState);

				gamblerUIState = new GamblerUIState();
				gamblerUIState.Activate();
				orchidModGamblerInterface.SetState(gamblerUIState);

				reloadShamanUI = false;
			}
		}

		public override void PostUpdateEverything()
		{
			PrimitiveTrailSystem.PostUpdateEverything();
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			OrchidModMessageType msgType = (OrchidModMessageType)reader.ReadByte();
			switch (msgType)
			{
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
					break;
				case OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					readSmallOrb = reader.ReadByte();
					modPlayer.shamanOrbSmall = (ShamanOrbSmall)readSmallOrb;

					if (Main.netMode == NetmodeID.Server)
					{
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

					if (Main.netMode == NetmodeID.Server)
					{
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

					if (Main.netMode == NetmodeID.Server)
					{
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

					if (Main.netMode == NetmodeID.Server)
					{
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

					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountCircle);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					attackTimer = reader.ReadInt32();
					modPlayer.shamanFireTimer = attackTimer;
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
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
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
						packet.Write(playernumber);
						packet.Write(modPlayer.gamblerHasCardInDeck);
						packet.Send(-1, playernumber);
					}
					break;

				default:
					Logger.WarnFormat("OrchidMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
}
