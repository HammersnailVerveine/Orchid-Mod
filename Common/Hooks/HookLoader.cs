using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common.Hooks
{
	public partial class HookLoader : ILoadable
	{
		void ILoadable.Load(Mod mod)
		{
			if (Main.dedServ) return;

			On.Terraria.UI.ItemSlot.OverrideHover_ItemArray_int_int += On_Terraria_ItemSlot_OverrideHover;
			On.Terraria.Main.DrawDust += On_Terraria_Main_DrawDust;
			On.Terraria.Main.DrawProjectiles += On_Terraria_Main_DrawProjectiles;
		}

		void ILoadable.Unload()
		{
			if (Main.dedServ) return;

			On.Terraria.UI.ItemSlot.OverrideHover_ItemArray_int_int -= On_Terraria_ItemSlot_OverrideHover;
			On.Terraria.Main.DrawDust -= On_Terraria_Main_DrawDust;
			On.Terraria.Main.DrawProjectiles -= On_Terraria_Main_DrawProjectiles;
		}
	}
}