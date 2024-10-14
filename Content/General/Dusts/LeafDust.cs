using Microsoft.Xna.Framework;
using OrchidMod.Assets;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Dusts
{
	public class LeafDust : ModDust
	{
		public override string Texture => OrchidAssets.DustsPath + Name;

		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(0, 3) * 10, 10, 10);
			dust.fadeIn = Main.rand.NextFloat((float)Math.PI * 2);
			dust.rotation = dust.fadeIn;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			switch ((int)(dust.customData ?? 0)) // 0 - Green, 1 - Orange
			{
				case 1: return new Color(250, 250, 250);
				default: return base.GetAlpha(dust, lightColor);
			}
		}

		public override bool Update(Dust dust)
		{
			if (dust.scale < 0.15f) dust.active = false;

			int style = (int)(dust.customData ?? 0);
			dust.frame = new Rectangle(10 * style, dust.frame.Y, 10, 10);

			dust.scale *= 0.975f;
			dust.position += dust.velocity;
			dust.color *= dust.scale;

			if (!Main.gamePaused) dust.rotation += (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.1f;

			if (style == 1) Lighting.AddLight(dust.position, new Vector3(255 / 255f, 180 / 255f, 0 / 255f) * dust.scale * 0.25f);

			return false;
		}
	}
}
