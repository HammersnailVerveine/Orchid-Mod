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

		private static readonly Dictionary<string, OrchidUIState> uiStates = new();
		private static readonly Dictionary<string, UserInterface> userInterfaces = new();

		public static readonly RasterizerState OverflowHiddenRasterizerState = new()
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true
		};

		public static T GetUIState<T>() where T : OrchidUIState
			=> uiStates.FirstOrDefault(i => i.Value is T) as T;

		public static OrchidUIState GetUIState(string name)
			=> uiStates[name];

		private static void OnResolutionChanged(Vector2 screenSize)
		{
			foreach (var (_, uiState) in uiStates)
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
				uiState.Activate();

				var userInterface = new UserInterface();
				userInterface.SetState(uiState);

				var name = type.Name;
				uiStates.Add(name, uiState);
				userInterfaces.Add(name, userInterface);
			}

			Main.OnResolutionChanged += OnResolutionChanged;
		}

		public override void Unload()
		{
			Main.OnResolutionChanged -= OnResolutionChanged;

			foreach (var (_, uiState) in uiStates)
			{
				uiState.Deactivate();
				uiState.Unload();
			}

			uiStates.Clear();
			userInterfaces.Clear();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			foreach (var (uiName, uiState) in uiStates)
			{
				var index = uiState.InsertionIndex(layers);
				if (index < 0) continue;

				layers.Insert(index, new LegacyGameInterfaceLayer(
					name: $"{Mod.Name}: {uiName}",
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
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (uiScale != Main.UIScale)
			{
				uiScale = Main.UIScale;

				foreach (var (_, uiState) in uiStates)
				{
					uiState.OnUIScaleChanged();
				}
			}

			foreach (var (_, uiState) in uiStates)
			{
				uiState.Update(gameTime);
			}
		}
	}
}