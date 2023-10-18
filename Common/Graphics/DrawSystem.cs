using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common.Graphics
{
	[Autoload(Side = ModSide.Client)]
	public class DrawSystem : ILoadable
	{
		public static Matrix TransformMatrix { get; private set; }

		private Dictionary<DrawLayers, List<IDrawData>> alphaBlendDataDict;
		private Dictionary<DrawLayers, List<IDrawData>> additiveDataDict;

		void ILoadable.Load(Mod mod)
		{
			alphaBlendDataDict = new();
			additiveDataDict = new();

			foreach (DrawLayers layer in Enum.GetValues(typeof(DrawLayers)))
			{
				alphaBlendDataDict.Add(layer, new List<IDrawData>());
				additiveDataDict.Add(layer, new List<IDrawData>());
			}

			Main.OnPostDraw += ClearAllData;

			Terraria.On_Main.DoDraw_UpdateCameraPosition += (orig) =>
			{
				orig();

				if (Main.gameMenu) return;

				foreach (var proj in Main.projectile)
				{
					if (proj.active && proj.ModProjectile is IDrawOnDifferentLayers obj)
					{
						obj.DrawOnDifferentLayers(this);
					}
				}
			};

			Terraria.On_Main.DoDraw_WallsAndBlacks += (orig, main) =>
			{
				orig(main);

				var spriteBatch = Main.spriteBatch;
				var spriteBatchInfo = new SpriteBatchInfo(spriteBatch);

				spriteBatch.End();
				DrawLayer(DrawLayers.Walls, Main.spriteBatch);
				spriteBatchInfo.Begin(spriteBatch);
			};

			Terraria.On_Main.DoDraw_Tiles_Solid += (orig, main) =>
			{
				orig(main);
				DrawLayer(DrawLayers.Tiles, Main.spriteBatch);
			};

			Terraria.On_Main.DrawDust += (orig, main) =>
			{
				orig(main);
				DrawLayer(DrawLayers.Dusts, Main.spriteBatch);
			};

			Terraria.On_Main.DrawBackgroundBlackFill += (orig, main) =>
			{
				orig(main);
				UpdateTransformMatrix();
			};
		}

		void ILoadable.Unload()
		{
			Main.OnPostDraw -= ClearAllData;

			ClearAllData();

			alphaBlendDataDict.Clear();
			additiveDataDict.Clear();

			alphaBlendDataDict = null;
			additiveDataDict = null;
		}

		private void DrawLayer(DrawLayers layer, SpriteBatch spriteBatch)
		{
			void DrawLayer(List<IDrawData> list, BlendState blendState)
			{
				if (list.Any())
				{
					spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

					foreach (var data in list)
					{
						data.Draw(spriteBatch);
					}

					spriteBatch.End();
				}
			}

			DrawLayer(alphaBlendDataDict[layer], BlendState.AlphaBlend);
			DrawLayer(additiveDataDict[layer], BlendState.Additive);
		}

		private void ClearAllData(GameTime gameTime = null)
		{
			static void ClearListsInDictionary(ref Dictionary<DrawLayers, List<IDrawData>> dict)
			{
				foreach (var (_, list) in dict)
				{
					list.Clear();
				}
			}

			ClearListsInDictionary(ref alphaBlendDataDict);
			ClearListsInDictionary(ref additiveDataDict);
		}

		private void UpdateTransformMatrix()
		{
			TransformMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up);
			TransformMatrix *= Main.GameViewMatrix.EffectMatrix;
			TransformMatrix *= Matrix.CreateTranslation(Main.screenWidth / 2, Main.screenHeight / -2, 0);
			TransformMatrix *= Matrix.CreateRotationZ(MathHelper.Pi);
			TransformMatrix *= Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1);
			TransformMatrix *= Matrix.CreateOrthographic(Main.screenWidth, Main.screenHeight, 0, 1000);
		}

		public void AddToAlphaBlend(DrawLayers layer, IDrawData data)
			=> alphaBlendDataDict[layer].Add(data);

		public void AddToAdditive(DrawLayers layer, IDrawData data)
			=> additiveDataDict[layer].Add(data);
	}
}