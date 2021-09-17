using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OrchidMod.Content
{
	public abstract class OrchidBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "OrchidMod/Assets/Textures/Buffs/" + name;
			return true;
		}
	}
}
