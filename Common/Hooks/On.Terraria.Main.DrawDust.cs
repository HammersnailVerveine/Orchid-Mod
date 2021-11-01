using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
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
		private static void On_Terraria_Main_DrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			{
				foreach (var proj in Main.projectile.ToList().FindAll(i => i.active && i.modProjectile is IDrawAdditive))
				{
					try
					{
						(proj.modProjectile as IDrawAdditive).DrawAdditive(Main.spriteBatch);
					}
					catch (Exception e)
					{
						TimeLogger.DrawException(e);
						proj.active = false;
					}
				}
			}
			Main.spriteBatch.End();
		}
	}
}
