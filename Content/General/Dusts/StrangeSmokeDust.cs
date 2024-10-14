using Microsoft.Xna.Framework;
using OrchidMod.Assets;
using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Dusts
{
	public class StrangeSmokeDust : ModDust
	{
		public override string Texture => OrchidAssets.DustsPath + Name;

		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(0, 5) * 10, 10, 10);
		}

		public override bool Update(Dust dust)
		{
			if (dust.scale < 0.15f) dust.active = false;

			dust.rotation += Math.Sign(Main.rand.Next(-1, 0)) * 0.01f;
			dust.position += dust.velocity;

			dust.scale *= 0.95f;
			dust.velocity *= 0.95f;

			return false;
		}
	}
}
