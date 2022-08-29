using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.GameContent;

namespace OrchidMod.Common.Graphics.Primitives
{
	public interface IPrimitiveEffect
	{
		abstract Asset<Effect> Effect { get; }
		virtual Matrix Matrix { get => DrawSystem.TransformMatrix; }

		void SetParameters(EffectParameterCollection parameters);

		// ...

		public class Default : IPrimitiveEffect
		{
			public readonly Asset<Texture2D> Texture;
			public readonly bool MultiplyColorByAlpha;

			Asset<Effect> IPrimitiveEffect.Effect => EffectLoader.GetEffect("DefaultPrimitive");

			public Default()
			{
				Texture = TextureAssets.MagicPixel;
				MultiplyColorByAlpha = true;
			}

			public Default(Asset<Texture2D> texture, bool multiplyColorByAlpha)
			{
				Texture = texture ?? TextureAssets.MagicPixel;
				MultiplyColorByAlpha = multiplyColorByAlpha;
			}

			void IPrimitiveEffect.SetParameters(EffectParameterCollection parameters)
			{
				parameters["Texture0"].SetValue(Texture.Value);
				parameters["MultiplyColorByAlpha"].SetValue(MultiplyColorByAlpha);
			}
		}

		public class Custom : IPrimitiveEffect
		{
			public readonly string EffectName;
			private readonly Action<EffectParameterCollection> OnSetParameters;

			Asset<Effect> IPrimitiveEffect.Effect => EffectLoader.GetEffect(EffectName);

			private Custom()
			{

			}

			public Custom(string effectName, Action<EffectParameterCollection> onSetParameters = null)
			{
				EffectName = effectName;
				OnSetParameters = onSetParameters ?? NullAction;
			}

			void IPrimitiveEffect.SetParameters(EffectParameterCollection parameters)
			{
				OnSetParameters.Invoke(parameters);
			}
		}

		// ...

		private static void NullAction(EffectParameterCollection _) { }
	}
}