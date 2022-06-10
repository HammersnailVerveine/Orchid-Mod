using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Common.Hooks
{
	public partial class HookLoader : ILoadable
	{
		void ILoadable.Load(Mod mod)
		{
			On.Terraria.Main.DrawDust += On_Terraria_Main_DrawDust;
			On.Terraria.Main.DrawProjectiles += On_Terraria_Main_DrawProjectiles;
			On.Terraria.Main.GUIChatDrawInner += On_Terraria_Main_GUIChatDrawInner;
			On.Terraria.Player.ScrollHotbar += On_Terraria_Player_ScrollHotbar;
		}

		void ILoadable.Unload()
		{
			On.Terraria.Main.DrawDust -= On_Terraria_Main_DrawDust;
			On.Terraria.Main.DrawProjectiles -= On_Terraria_Main_DrawProjectiles;
			On.Terraria.Main.GUIChatDrawInner -= On_Terraria_Main_GUIChatDrawInner;
			On.Terraria.Player.ScrollHotbar -= On_Terraria_Player_ScrollHotbar;
		}
	}
}