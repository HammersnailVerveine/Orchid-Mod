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
	public class UISystem : ModSystem
	{
		private static float uiScale = -1f;
		private static bool ignoreHotbarScroll = false;

		private static readonly List<OrchidUIState> uiStates = new();
		private static readonly List<UserInterface> userInterfaces = new();

		public static readonly RasterizerState OverflowHiddenRasterizerState = new()
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true
		};

		public static void RequestIgnoreHotbarScroll()
			=> ignoreHotbarScroll = true;

		public static T GetUIState<T>() where T : OrchidUIState
			=> uiStates.FirstOrDefault(i => i is T) as T;

		private static void OnResolutionChanged(Vector2 screenSize)
		{
			foreach (var uiState in uiStates)
			{
				uiState.OnResolutionChanged((int)screenSize.X, (int)screenSize.Y);
			}
		}

		private static void ModifyScrollHotbar(On.Terraria.Player.orig_ScrollHotbar orig, Player player, int offset)
		{
			if (ignoreHotbarScroll) return;

			orig(player, offset);
		}

		private static void ResetVariables(GameTime _)
		{
			ignoreHotbarScroll = false;
		}

		// ...

		public override void Load()
		{
			foreach (Type type in Mod.Code.GetTypes())
			{
				if (!type.IsSubclassOf(typeof(OrchidUIState))) continue;

				var uiState = (OrchidUIState)Activator.CreateInstance(type, null);
				uiStates.Add(uiState);
			}

			uiStates.Sort((x, y) => x.Priority.CompareTo(y.Priority));

			foreach (var uiState in uiStates)
			{
				uiState.Mod = Mod;
				uiState.Activate();

				var userInterface = new UserInterface();
				userInterface.SetState(uiState);
				userInterfaces.Add(userInterface);
			}

			On.Terraria.Player.ScrollHotbar += ModifyScrollHotbar;
			Main.OnResolutionChanged += OnResolutionChanged;
			Main.OnPreDraw += ResetVariables;
		}

		public override void Unload()
		{
			Main.OnPreDraw -= ResetVariables;
			Main.OnResolutionChanged -= OnResolutionChanged;
			On.Terraria.Player.ScrollHotbar -= ModifyScrollHotbar;

			foreach (var uiState in uiStates)
			{
				uiState.Deactivate();
				uiState.Unload();
			}

			uiStates.Clear();
			userInterfaces.Clear();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			for (int i = 0; i < userInterfaces.Count; i++)
			{
				var uiState = uiStates[i];
				var userInterface = userInterfaces[i];

				var index = uiState.InsertionIndex(layers);
				if (index < 0) continue;

				layers.Insert(index, new LegacyGameInterfaceLayer(
					name: $"{Mod.Name}: {uiState.Name}",
					drawMethod: () =>
					{
						if (uiState.Visible)
						{
							userInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
						return true;
					},
					scaleType: uiState.ScaleType)
				);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (uiScale != Main.UIScale)
			{
				uiScale = Main.UIScale;

				foreach (var uiState in uiStates)
				{
					uiState.OnUIScaleChanged();
				}
			}

			if (Main.mapFullscreen) return;

			foreach (var userInterface in userInterfaces)
			{
				userInterface.Update(gameTime);
			}
		}
	}
}