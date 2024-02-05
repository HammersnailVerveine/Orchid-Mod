using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq.Expressions;

namespace OrchidMod.Utilities
{
	public struct SpriteBatchSnapshot
	{
		private static readonly Func<SpriteBatch, SpriteSortMode> sortModeFieldAccessor;
		private static readonly Func<SpriteBatch, BlendState> blendStateFieldAccessor;
		private static readonly Func<SpriteBatch, SamplerState> samplerStateFieldAccessor;
		private static readonly Func<SpriteBatch, DepthStencilState> depthStencilStateFieldAccessor;
		private static readonly Func<SpriteBatch, RasterizerState> rasterizerStateFieldAccessor;
		private static readonly Func<SpriteBatch, Effect> effectFieldAccessor;
		private static readonly Func<SpriteBatch, Matrix> matrixFieldAccessor;

		public SpriteSortMode SortMode;
		public BlendState BlendState;
		public SamplerState SamplerState;
		public DepthStencilState DepthStencilState;
		public RasterizerState RasterizerState;
		public Effect Effect;
		public Matrix Matrix;

		static SpriteBatchSnapshot()
		{
			sortModeFieldAccessor = GetFieldAccessor<SpriteBatch, SpriteSortMode>("sortMode");
			blendStateFieldAccessor = GetFieldAccessor<SpriteBatch, BlendState>("blendState");
			samplerStateFieldAccessor = GetFieldAccessor<SpriteBatch, SamplerState>("samplerState");
			depthStencilStateFieldAccessor = GetFieldAccessor<SpriteBatch, DepthStencilState>("depthStencilState");
			rasterizerStateFieldAccessor = GetFieldAccessor<SpriteBatch, RasterizerState>("rasterizerState");
			effectFieldAccessor = GetFieldAccessor<SpriteBatch, Effect>("customEffect");
			matrixFieldAccessor = GetFieldAccessor<SpriteBatch, Matrix>("transformMatrix");
		}

		public SpriteBatchSnapshot(SpriteBatch spriteBatch)
		{
			if (spriteBatch is null)
				throw new ArgumentNullException(nameof(spriteBatch));

			SortMode = sortModeFieldAccessor(spriteBatch);
			BlendState = blendStateFieldAccessor(spriteBatch);
			SamplerState = samplerStateFieldAccessor(spriteBatch);
			DepthStencilState = depthStencilStateFieldAccessor(spriteBatch);
			RasterizerState = rasterizerStateFieldAccessor(spriteBatch);
			Effect = effectFieldAccessor(spriteBatch);
			Matrix = matrixFieldAccessor(spriteBatch);
		}

		private static Func<T, V> GetFieldAccessor<T, V>(string fieldName)
		{
			var param = Expression.Parameter(typeof(T), "arg");
			var member = Expression.Field(param, fieldName);
			var lambda = Expression.Lambda(typeof(Func<T, V>), member, param);

			return lambda.Compile() as Func<T, V>;
		}

		private static Action<T, V> SetFieldAccessor<T, V>(string fieldName)
		{
			var param = Expression.Parameter(typeof(T), "arg");
			var valueParam = Expression.Parameter(typeof(V), "value");
			var member = Expression.Field(param, fieldName);
			var assign = Expression.Assign(member, valueParam);
			var lambda = Expression.Lambda(typeof(Action<T, V>), assign, param, valueParam);

			return lambda.Compile() as Action<T, V>;
		}
	}

	public static class SpriteBatchSnapshotExtensions
	{
		public static void Begin(this SpriteBatch spriteBatch, SpriteBatchSnapshot spriteBatchSnapshot)
		{
			spriteBatch.Begin
			(
				spriteBatchSnapshot.SortMode, spriteBatchSnapshot.BlendState, spriteBatchSnapshot.SamplerState, spriteBatchSnapshot.DepthStencilState,
				spriteBatchSnapshot.RasterizerState, spriteBatchSnapshot.Effect, spriteBatchSnapshot.Matrix
			);
		}

		public static void End(this SpriteBatch spriteBatch, out SpriteBatchSnapshot spriteBatchSnapshot)
		{
			spriteBatchSnapshot = new SpriteBatchSnapshot(spriteBatch);
			spriteBatch.End();
		}
	}
}