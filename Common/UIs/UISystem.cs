using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Common.UIs
{
	[Autoload(Side = ModSide.Client)]
	// TODO: Auto-loading pls...
	public class UISystem : ModSystem
	{
		/*
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
		internal GuardianUIState guardianUIState;*/

		// ...

		private static readonly Dictionary<OrchidUIState, UserInterface> uis = new();
		private static float uiScale = -1f;

		public static readonly RasterizerState OverflowHiddenRasterizerState = new()
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true
		};

		public static void AddUIState(OrchidUIState state, UserInterface userInterface)
		{
			if (state == null || userInterface == null) return;

			uis.Add(state, userInterface);
		}

		public static T GetUIState<T>() where T : OrchidUIState
		{
			return uis.Keys.FirstOrDefault(i => i is T) as T;
		}

		private static void OnResolutionChanged(Vector2 screenSize)
		{
			foreach (var (uiState, _) in uis)
			{
				uiState.OnResolutionChanged((int)screenSize.X, (int)screenSize.Y);
			}
		}

		// ...

		public override void Load()
		{
			foreach (Type type in Mod.Code.GetTypes())
			{
				if (!type.IsSubclassOf(typeof(OrchidUIState))) continue;

				var uiState = (OrchidUIState)Activator.CreateInstance(type, null);
				uiState.Mod = Mod;
				uiState.Load();
				uiState.Activate();

				var userInterface = new UserInterface();
				userInterface.SetState(uiState);

				uis.Add(uiState, userInterface);
			}

			Main.OnResolutionChanged += OnResolutionChanged;

			// ...

			/*shamanUIState = new ShamanUIState();
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
			coatingTextures[5] = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistCoatingDark", AssetRequestMode.ImmediateLoad).Value;*/
		}

		public override void PostSetupContent()
		{
			foreach (var (uiState, _) in uis)
			{
				uiState.PostSetupContent();
			}
		}

		public override void Unload()
		{
			Main.OnResolutionChanged -= OnResolutionChanged;

			foreach (var (uiState, _) in uis)
			{
				uiState.Deactivate();
				uiState.Unload();
			}

			uis.Clear();

			/*AlchemistUIFrame.ressourceBottom = null;
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
			guardianUIState = null;*/
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			foreach (var (uiState, userInterface) in uis)
			{
				var index = uiState.InsertionIndex(layers);
				if (index < 0) continue;

				layers.Insert(index, new LegacyGameInterfaceLayer(
					name: $"{Mod.Name}: {uiState.GetType().Name}",
					drawMethod: () =>
					{
						if (uiState.Visible)
						{
							uiState.Draw(Main.spriteBatch);
						}
						return true;
					},
					scaleType: uiState.ScaleType)
				);
			}

			/*
			 * int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
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
			 */
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (uiScale != Main.UIScale)
			{
				uiScale = Main.UIScale;

				foreach (var (uiState, _) in uis)
				{
					if (uiState.Visible)
					{
						uiState.OnUIScaleChanged();
					}
				}
			}

			foreach (var elem in uis.Values)
			{
				elem.Update(gameTime);
			}
		}
	}
}