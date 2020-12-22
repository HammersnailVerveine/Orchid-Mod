using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Dusts.Thorium
{
	public class ToxicDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0f;
			dust.noGravity = true;
			dust.noLight = true;
		}
	}
}