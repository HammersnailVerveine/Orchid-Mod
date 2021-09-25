using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Content
{
	public abstract class OrchidDust : ModDust
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "OrchidMod/Assets/Textures/Dusts/" + name;
			return true;
		}
	}
}
