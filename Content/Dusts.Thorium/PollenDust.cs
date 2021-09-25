using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Dusts
{
	public class PollenDust : OrchidDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.scale *= 1.25f;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noLight)
			{
				float strength = dust.scale * 1.4f;
				if (strength > 1f)
				{
					strength = 1f;
				}

				Lighting.AddLight(dust.position, 0.2f * strength, 0.01f * strength, 0.075f * strength);
			}

			return false;
		}
	}
}