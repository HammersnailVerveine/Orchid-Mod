using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace OrchidMod.Utilities
{
	public static partial class OrchidUtils
	{
		public static void SpawnDustCircle(Vector2 center, float radius, int count, int type, Action<Dust> onSpawn = null)
		{
			for (int i = 0; i < count; i++)
			{
				Vector2 position = center + new Vector2(radius, 0).RotatedBy(i / (float)count * MathHelper.TwoPi);
				var dust = Dust.NewDustPerfect(position, type);
				onSpawn?.Invoke(dust);
			}
		}

		public static void SpawnDustCircle(Vector2 center, float radius, int count, Func<int, int> type, Action<Dust, int, float> onSpawn = null)
		{
			for (int i = 0; i < count; i++)
			{
				float angle = i / (float)count * MathHelper.TwoPi;
				Vector2 position = center + new Vector2(radius, 0).RotatedBy(angle);
				int dustType = type?.Invoke(i) ?? -1;

				if (dustType != -1)
				{
					var dust = Dust.NewDustPerfect(position, dustType);
					onSpawn?.Invoke(dust, i, angle);
				}
			}
		}
	}
}