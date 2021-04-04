using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Dusts
{
	public class StrangeSmokeDust : ModDust
	{
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
