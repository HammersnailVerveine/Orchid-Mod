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
		private static void On_Terraria_Main_GUIChatDrawInner(On.Terraria.Main.orig_GUIChatDrawInner orig, Main self)
		{
			orig(self);

			var ui = OrchidMod.Instance?.croupierUI;
			if (ui == null) return;

			if (ui.Visible)
			{
				ui.Update();
				ui.Draw(Main.spriteBatch);
			}
		}
	}
}
