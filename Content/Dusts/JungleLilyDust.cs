using Microsoft.Xna.Framework;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Dusts
{
	public class JungleLilyDust : ModDust
	{
		public override string Texture => OrchidAssets.DustsPath + Name;

		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 6, 6);
		}

		public override bool Update(Dust dust)
		{
			if (dust.scale < 0.1f) dust.active = false;

			dust.velocity = new Vector2((float)Math.Sin(Main.time * 0.007f) * 0.5f, -0.6f * (1 - dust.scale));
			dust.position += dust.velocity;
			dust.scale *= 0.98f;

			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
			=> new Color(255, 192, 0) * MathUtils.MultiLerp<float>(MathHelper.Lerp, 1 - dust.scale, .0f, .7f, .9f, .7f, .3f, .1f, .0f);
	}
}
