using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Dusts
{
	public class ToxicDust : OrchidDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0f;
			dust.noGravity = true;
			dust.noLight = true;
		}
	}
}