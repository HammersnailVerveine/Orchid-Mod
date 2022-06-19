using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;

namespace OrchidMod.Common
{
	public struct SpriteBatchInfo
	{
		private static readonly FieldInfo sortModeField;
		private static readonly FieldInfo blendStateField;
		private static readonly FieldInfo samplerStateField;
		private static readonly FieldInfo depthStencilStateField;
		private static readonly FieldInfo rasterizerStateField;
		private static readonly FieldInfo effectField;
		private static readonly FieldInfo matrixField;

		public SpriteSortMode SortMode;
		public BlendState BlendState;
		public SamplerState SamplerState;
		public DepthStencilState DepthStencilState;
		public RasterizerState RasterizerState;
		public Effect Effect;
		public Matrix Matrix;

		static SpriteBatchInfo()
		{
			var type = typeof(SpriteBatch);
			var flags = BindingFlags.NonPublic | BindingFlags.Instance;

			sortModeField = type.GetField("sortMode", flags);
			blendStateField = type.GetField("blendState", flags);
			samplerStateField = type.GetField("samplerState", flags);
			depthStencilStateField = type.GetField("depthStencilState", flags);
			rasterizerStateField = type.GetField("rasterizerState", flags);
			effectField = type.GetField("customEffect", flags);
			matrixField = type.GetField("transformMatrix", flags);
		}

		public SpriteBatchInfo(SpriteBatch spriteBatch)
		{
			if (spriteBatch == null) throw new ArgumentNullException(nameof(spriteBatch));

			SortMode = (SpriteSortMode)sortModeField.GetValue(spriteBatch);
			BlendState = (BlendState)blendStateField.GetValue(spriteBatch);
			SamplerState = (SamplerState)samplerStateField.GetValue(spriteBatch);
			DepthStencilState = (DepthStencilState)depthStencilStateField.GetValue(spriteBatch);
			RasterizerState = (RasterizerState)rasterizerStateField.GetValue(spriteBatch);
			Effect = (Effect)effectField.GetValue(spriteBatch);
			Matrix = (Matrix)matrixField.GetValue(spriteBatch);
		}

		public void Begin(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix);
		}

		public void Begin(SpriteBatch spriteBatch, BlendState blendState, Effect effect)
		{
			spriteBatch.Begin(SortMode, blendState ?? BlendState, SamplerState, DepthStencilState, RasterizerState, effect ?? Effect, Matrix);
		}

		public void Begin(SpriteBatch spriteBatch, BlendState blendState, Effect effect, Matrix? matrix)
		{
			spriteBatch.Begin(SortMode, blendState ?? BlendState, SamplerState, DepthStencilState, RasterizerState, effect ?? Effect, matrix ?? Matrix);
		}

		public void Begin(SpriteBatch spriteBatch, SpriteSortMode? sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix? matrix)
		{
			spriteBatch.Begin(sortMode ?? SortMode, blendState ?? BlendState, samplerState ?? SamplerState, depthStencilState ?? DepthStencilState, rasterizerState ?? RasterizerState, effect ?? Effect, matrix ?? Matrix);
		}
	}
}
