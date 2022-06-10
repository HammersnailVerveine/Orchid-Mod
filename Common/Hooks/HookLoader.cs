using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common.Hooks
{
	public partial class HookLoader : ILoadable
	{
		void ILoadable.Load(Mod mod)
		{
			if (Main.dedServ) return;

			On.Terraria.Main.DrawDust += On_Terraria_Main_DrawDust;
			On.Terraria.Main.DrawProjectiles += On_Terraria_Main_DrawProjectiles;
			On.Terraria.Main.GUIChatDrawInner += On_Terraria_Main_GUIChatDrawInner;
			On.Terraria.Player.ScrollHotbar += On_Terraria_Player_ScrollHotbar;
		}

		void ILoadable.Unload()
		{
			if (Main.dedServ) return;

			On.Terraria.Main.DrawDust -= On_Terraria_Main_DrawDust;
			On.Terraria.Main.DrawProjectiles -= On_Terraria_Main_DrawProjectiles;
			On.Terraria.Main.GUIChatDrawInner -= On_Terraria_Main_GUIChatDrawInner;
			On.Terraria.Player.ScrollHotbar -= On_Terraria_Player_ScrollHotbar;
		}
	}
}