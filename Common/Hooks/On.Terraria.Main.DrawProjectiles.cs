using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OrchidMod.Common.Hooks
{
	public static partial class HookLoader
	{
		private static void On_Terraria_Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			var matrix = PrimitiveTrailSystem.GetTransformMatrix();

			if (PrimitiveTrailSystem.AlphaBlendTrails.Count > 0)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
				{
					foreach (var trail in PrimitiveTrailSystem.AlphaBlendTrails)
					{
						trail.Draw(Main.spriteBatch, matrix);
					}
				}
				Main.spriteBatch.End();
			}

			if (PrimitiveTrailSystem.AdditiveBlendTrails.Count > 0)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
				{
					foreach (var trail in PrimitiveTrailSystem.AdditiveBlendTrails)
					{
						trail.Draw(Main.spriteBatch, matrix);
					}
				}
				Main.spriteBatch.End();
			}

			orig(self);
		}
	}
}
