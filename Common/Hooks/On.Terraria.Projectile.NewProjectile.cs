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
		private static int On_Terraria_Projectile_NewProjectile(
		On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig,
		float x, float y, float speedX, float speedY, int type, int damage, float knockBack, int owner, float ai0, float ai1)
		{
			int index = orig(x, y, speedX, speedY, type, damage, knockBack, owner, ai0, ai1);

			var proj = Main.projectile[index];
			if (proj?.modProjectile is Content.OrchidProjectile orchidProj) orchidProj.OnSpawn();

			return index;
		}
	}
}
