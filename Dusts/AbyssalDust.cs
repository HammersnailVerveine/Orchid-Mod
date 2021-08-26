using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Dusts
{
	public class AbyssalDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y = 0.1f;
			dust.velocity.X *= 0.1f;
			dust.scale *= 1f;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}

			if (dust.noLight)
			{
				return false;
			}
			Lighting.AddLight(dust.position, 0f, 0f, 0.8f);
			return false;
		}
	}
}