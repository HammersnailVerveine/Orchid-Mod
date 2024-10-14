using OrchidMod.Assets;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Dusts
{
	public class AbyssalBrightDust : ModDust
	{
		public override string Texture => OrchidAssets.DustsPath + Name;

		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y = 0.1f;
			dust.velocity.X *= 0.1f;
			dust.scale *= 0.8f;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}

			if (dust.noLight) return false;

			Lighting.AddLight(dust.position, 0.3f, 0.3f, 0.8f);

			return false;
		}
	}
}