using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Common.Hooks
{
	public static partial class HookLoader
	{
		public static void Load(Mod mod)
		{
			On.Terraria.Main.DrawProjectiles += On_Terraria_Main_DrawProjectiles;
			On.Terraria.Main.GUIChatDrawInner += On_Terraria_Main_GUIChatDrawInner;
			On.Terraria.Player.ScrollHotbar += On_Terraria_Player_ScrollHotbar;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += On_Terraria_Projectile_NewProjectile;
		}

		public static void Unload()
		{
			On.Terraria.Main.DrawProjectiles -= On_Terraria_Main_DrawProjectiles;
			On.Terraria.Main.GUIChatDrawInner -= On_Terraria_Main_GUIChatDrawInner;
			On.Terraria.Player.ScrollHotbar -= On_Terraria_Player_ScrollHotbar;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= On_Terraria_Projectile_NewProjectile;
		}
	}
}