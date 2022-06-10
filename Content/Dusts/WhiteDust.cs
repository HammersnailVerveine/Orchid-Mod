using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Dusts
{
	public class WhiteDust : ModDust
	{
		public override string Texture => OrchidAssets.DustsPath + Name;

		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y *= 1f;
			dust.velocity.X *= 1f;
			dust.scale *= 1f;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}

			return false;
		}
	}
}