using OrchidMod.Assets;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Dusts
{
	public class ToxicDust : ModDust
	{
		public override string Texture => OrchidAssets.DustsPath + Name;

		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0f;
			dust.noGravity = true;
			dust.noLight = true;
		}
	}
}