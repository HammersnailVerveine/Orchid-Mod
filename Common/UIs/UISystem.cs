using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.UI;
using OrchidMod.Gambler.UI;
using OrchidMod.Guardian.UI;
using OrchidMod.Shaman.UI;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Common.UIs
{
	[Autoload(Side = ModSide.Client)]
	// TODO: Auto-loading pls...
	public class UISystem : ModSystem
	{
		public static bool reloadShamanUI;
		public static Texture2D[] coatingTextures;

		internal UserInterface orchidModShamanInterface;
		internal UserInterface orchidModShamanCharacterInterface;
		internal UserInterface orchidModAlchemistInterface;
		internal UserInterface orchidModAlchemistSelectInterface;
		internal UserInterface orchidModAlchemistSelectKeysInterface;
		internal UserInterface orchidModAlchemistBookInterface;
		internal UserInterface orchidModGamblerInterface;
		internal UserInterface orchidModGuardianInterface;
		internal ShamanUIState shamanUIState;
		internal ShamanCharacterUIState shamanCharacterUIState;
		internal AlchemistUIState alchemistUIState;
		internal AlchemistSelectUIState alchemistSelectUIState;
		internal AlchemistSelectKeysUIState alchemistSelectKeysUIState;
		internal AlchemistBookUIState alchemistBookUIState;
		internal GamblerUIState gamblerUIState;
		internal GuardianUIState guardianUIState;

		public override void Load()
		{
			shamanUIState = new ShamanUIState();
			shamanCharacterUIState = new ShamanCharacterUIState();
			alchemistUIState = new AlchemistUIState();
			alchemistSelectUIState = new AlchemistSelectUIState();
			alchemistSelectKeysUIState = new AlchemistSelectKeysUIState();
			alchemistBookUIState = new AlchemistBookUIState();
			gamblerUIState = new GamblerUIState();
			guardianUIState = new GuardianUIState();

			orchidModShamanInterface = new UserInterface();
			orchidModShamanCharacterInterface = new UserInterface();
			orchidModAlchemistInterface = new UserInterface();
			orchidModAlchemistSelectInterface = new UserInterface();
			orchidModAlchemistSelectKeysInterface = new UserInterface();
			orchidModAlchemistBookInterface = new UserInterface();
			orchidModGamblerInterface = new UserInterface();
			orchidModGuardianInterface = new UserInterface();

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

			guardianUIState.Activate();
			orchidModGuardianInterface.SetState(guardianUIState);

			coatingTextures = new Texture2D[6];
			coatingTextures[0] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingFire", AssetRequestMode.ImmediateLoad).Value;
			coatingTextures[1] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingWater", AssetRequestMode.ImmediateLoad).Value;
			coatingTextures[2] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingNature", AssetRequestMode.ImmediateLoad).Value;
			coatingTextures[3] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingAir", AssetRequestMode.ImmediateLoad).Value;
			coatingTextures[4] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingLight", AssetRequestMode.ImmediateLoad).Value;
			coatingTextures[5] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingDark", AssetRequestMode.ImmediateLoad).Value;
		}

		public override void Unload()
		{
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
				GamblerUIFrame.chipDetonatorMain = null;
				GamblerUIFrame.chipDetonatorBar = null;
				GamblerUIFrame.chipDetonatorBarEnd = null;

				GuardianUIFrame.textureBlockOn = null;
				GuardianUIFrame.textureBlockOff = null;
				GuardianUIFrame.textureSlamOn = null;
				GuardianUIFrame.textureSlamOff = null;

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
			guardianUIState = null;
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
						orchidModGuardianInterface.Draw(Main.spriteBatch, new GameTime());
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

				guardianUIState = new GuardianUIState();
				guardianUIState.Activate();
				orchidModGuardianInterface.SetState(guardianUIState);

				reloadShamanUI = false;
			}
		}
	}
}
