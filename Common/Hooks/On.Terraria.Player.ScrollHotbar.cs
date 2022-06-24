using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OrchidMod.Common.Hooks
{
	public partial class HookLoader
	{
		private static void On_Terraria_Player_ScrollHotbar(On.Terraria.Player.orig_ScrollHotbar orig, Player player, int offset)
		{
			if (Main.LocalPlayer.GetModPlayer<OrchidPlayer>().ignoreScrollHotbar) return;
			orig(player, offset);
		}
	}
}
